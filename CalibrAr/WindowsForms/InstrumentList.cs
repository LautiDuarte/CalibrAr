using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class InstrumentList : Form
    {
        public InstrumentList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            this.instrumentsDataGridView.AutoGenerateColumns = false;

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Code",
                HeaderText = "Code",
                DataPropertyName = "Code",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "Serial number",
                DataPropertyName = "SerialNumber",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Brand",
                HeaderText = "Brand",
                DataPropertyName = "Brand",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Model",
                HeaderText = "Model",
                DataPropertyName = "Model",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaxAllowedError",
                HeaderText = "Max allowed error",
                DataPropertyName = "MaxAllowedError",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalibrationFrequencyMonths",
                HeaderText = "Calibration frequency months",
                DataPropertyName = "CalibrationFrequencyMonths",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LastCalibrationDate",
                HeaderText = "Last calibration date",
                DataPropertyName = "LastCalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextCalibrationDate",
                HeaderText = "Next calibration date",
                DataPropertyName = "NextCalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InstrumentTypeName",
                HeaderText = "Instrument Type",
                DataPropertyName = "InstrumentTypeName",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AreaName",
                HeaderText = "Area",
                DataPropertyName = "AreaName",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is active",
                DataPropertyName = "IsActive",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpdatedAt",
                HeaderText = "Updated at",
                DataPropertyName = "UpdatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
        }

        private async void InstrumentList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadInstruments();
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = authService.HasPermission("Instruments.create");
            bool canUpdate = authService.HasPermission("Instruments.update");
            bool canDelete = authService.HasPermission("Instruments.delete");

            // Configurar visibilidad de botones según permisos
            createButton.Visible = canCreate;
            updateButton.Visible = canUpdate;
            deleteButton.Visible = canDelete;

            // Guardar permisos en Tag para uso posterior
            createButton.Tag = canCreate;
            updateButton.Tag = canUpdate;
            deleteButton.Tag = canDelete;
        }

        private async void createButton_Click(object sender, EventArgs e)
        {
            InstrumentDTO newInstrument = new InstrumentDTO();
            InstrumentDetail instrumentDetail = new InstrumentDetail(FormMode.Create, newInstrument);

            instrumentDetail.ShowDialog();
            await LoadInstruments();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                DisableControls();

                var selected = this.SelectedItem();
                if (selected == null)
                {
                    MessageBox.Show("Seleccioná una fila primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                InstrumentDTO instrument = await InstrumentApiClient.GetAsync(selected.Id);

                InstrumentDetail instrumentDetail = new InstrumentDetail(FormMode.Update, instrument);
                instrumentDetail.ShowDialog();
                await LoadInstruments();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating instrument: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            var instrument = this.SelectedItem();
            if (instrument == null)
            {
                MessageBox.Show("Seleccioná una fila primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"¿Are you sure you want to delete this item: {instrument.Code}{instrument.Name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await InstrumentApiClient.DeleteAsync(instrument.Id);
                    await LoadInstruments();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting instrument: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private async Task LoadInstruments()
        {
            try
            {
                DisableControls();
                this.instrumentsDataGridView.DataSource = null;

                IEnumerable<InstrumentDTO> instruments = await InstrumentApiClient.GetAllAsync();

                this.instrumentsDataGridView.DataSource = instruments;
                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.instrumentsDataGridView.Rows.Count > 0)
                {
                    this.instrumentsDataGridView.Rows[0].Selected = true;

                    // Solo habilitar si tiene permisos Y hay elementos
                    if (canDelete) this.deleteButton.Enabled = true;
                    if (canUpdate) this.updateButton.Enabled = true;
                }
                else
                {
                    // Solo deshabilitar si son visibles
                    if (canDelete) this.deleteButton.Enabled = false;
                    if (canUpdate) this.updateButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading instruments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private InstrumentDTO? SelectedItem()
        {
            if (instrumentsDataGridView.SelectedRows.Count == 0)
                return null;

            return (InstrumentDTO)instrumentsDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            instrumentsDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            instrumentsDataGridView.Enabled = true;
        }
    }
}
