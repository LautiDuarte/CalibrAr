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
    public partial class InstrumentTypeList : Form
    {
        public InstrumentTypeList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            this.instrumentTypesDataGridView.AutoGenerateColumns = false;
            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 200
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 300
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MeasurementUnit",
                HeaderText = "Measurement unit",
                DataPropertyName = "MeasurementUnit",
                Width = 150
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaxAllowedError",
                HeaderText = "Max allowed error",
                DataPropertyName = "MaxAllowedError",
                Width = 150
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalibrationFrequencyMonths",
                HeaderText = "Calibration frequency (months)",
                DataPropertyName = "CalibrationFrequencyMonths",
                Width = 200
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is active",
                DataPropertyName = "IsActive",
                Width = 80
            });

            this.instrumentTypesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

        }

        private async void InstrumentTypeList_Load(object sender, EventArgs e)
        {
            await ConfigureButtonPermissions();
            await this.LoadInstrumentTypes();
        }


        private async Task ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = await authService.HasPermissionAsync("InstrumentTypes.create");
            bool canUpdate = await authService.HasPermissionAsync("InstrumentTypes.update");
            bool canDelete = await authService.HasPermissionAsync("InstrumentTypes.delete");

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
            InstrumentTypeDTO newInstrumentType = new InstrumentTypeDTO();
            InstrumentTypeDetail instrumentTypeDetail = new InstrumentTypeDetail(FormMode.Create, newInstrumentType);

            instrumentTypeDetail.ShowDialog(this);
            await this.LoadInstrumentTypes();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            InstrumentTypeDTO? selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                InstrumentTypeDTO instrumentType = await InstrumentTypeApiClient.GetAsync(selected.Id);

                InstrumentTypeDetail instrumentTypeDetail = new InstrumentTypeDetail(FormMode.Update, instrumentType);
                instrumentTypeDetail.ShowDialog(this);
                await this.LoadInstrumentTypes();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating instrument type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            InstrumentTypeDTO instrumentType = this.SelectedItem();
            if (instrumentType == null) return;

            var result = MessageBox.Show($"¿Are you sure you want to delete this item: {instrumentType.Name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await InstrumentTypeApiClient.DeleteAsync(instrumentType.Id);
                    await this.LoadInstrumentTypes();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting instrument type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private async Task LoadInstrumentTypes()
        {
            try
            {
                DisableControls();
                this.instrumentTypesDataGridView.DataSource = null;

                IEnumerable<InstrumentTypeDTO> instrumentTypes = await InstrumentTypeApiClient.GetAllAsync();

                this.instrumentTypesDataGridView.DataSource = instrumentTypes;

                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.instrumentTypesDataGridView.Rows.Count > 0)
                {
                    this.instrumentTypesDataGridView.Rows[0].Selected = true;

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
                MessageBox.Show($"Error loading instrument types: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private InstrumentTypeDTO SelectedItem()
        {
            InstrumentTypeDTO instrumentType;
            if(instrumentTypesDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an instrument type first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            instrumentType = (InstrumentTypeDTO)instrumentTypesDataGridView.SelectedRows[0].DataBoundItem;
            return instrumentType;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            instrumentTypesDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            instrumentTypesDataGridView.Enabled = true;
        }
    }
}
