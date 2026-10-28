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
using API.Auth.WindowsForms;
using DTOs;

namespace WindowsForms
{
    public partial class InstrumentList : Form
    {
        private readonly InstrumentApiClient instrumentApiClient = new(AuthServiceProvider.Instance);
        private readonly AreaApiClient areaApiClient = new(AuthServiceProvider.Instance);

        public InstrumentList()
        {
            InitializeComponent();
            ConfigureColumns();
            EnumText.ApplyTo(instrumentsDataGridView, "Status");
        }

        private void ConfigureColumns()
        {
            this.instrumentsDataGridView.AutoGenerateColumns = false;

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80,
                Visible = false
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Code",
                HeaderText = "Código",
                DataPropertyName = "Code",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Nombre",
                DataPropertyName = "Name",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "N° de serie",
                DataPropertyName = "SerialNumber",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Brand",
                HeaderText = "Marca",
                DataPropertyName = "Brand",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Model",
                HeaderText = "Modelo",
                DataPropertyName = "Model",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Estado",
                DataPropertyName = "Status",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaxAllowedError",
                HeaderText = "Error máximo",
                DataPropertyName = "MaxAllowedError",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalibrationFrequencyMonths",
                HeaderText = "Frecuencia (meses)",
                DataPropertyName = "CalibrationFrequencyMonths",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LastCalibrationDate",
                HeaderText = "Última calibración",
                DataPropertyName = "LastCalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextCalibrationDate",
                HeaderText = "Próxima calibración",
                DataPropertyName = "NextCalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InstrumentTypeName",
                HeaderText = "Tipo de instrumento",
                DataPropertyName = "InstrumentTypeName",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AreaName",
                HeaderText = "Área",
                DataPropertyName = "AreaName",
                Width = 200
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Activo",
                DataPropertyName = "IsActive",
                Width = 80
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Fecha de alta",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.instrumentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UpdatedAt",
                HeaderText = "Última modificación",
                DataPropertyName = "UpdatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
        }

        private async void InstrumentList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadFilters();
            await LoadInstruments();
        }

        private async Task LoadFilters()
        {
            try
            {
                EnumText.ApplyTo(statusFilterComboBox);
                statusFilterComboBox.DataSource = Enum.GetValues(typeof(InstrumentStatusDTO));
                statusFilterComboBox.SelectedIndex = -1;

                var areas = await areaApiClient.GetAllAsync();
                areaFilterComboBox.DataSource = areas
                    .Select(a => new
                    {
                        a.Id,
                        DisplayName = $"{a.Name} ({a.LocationName})"
                    })
                    .ToList();
                areaFilterComboBox.DisplayMember = "DisplayName";
                areaFilterComboBox.ValueMember = "Id";
                areaFilterComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los filtros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await LoadInstruments();
        }

        private async void ClearButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            statusFilterComboBox.SelectedIndex = -1;
            areaFilterComboBox.SelectedIndex = -1;
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

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            InstrumentDTO newInstrument = new InstrumentDTO();
            InstrumentDetail instrumentDetail = new InstrumentDetail(FormMode.Create, newInstrument);

            instrumentDetail.ShowDialog(this);
            await LoadInstruments();
        }

        private async void UpdateButton_Click(object sender, EventArgs e)
        {
            InstrumentDTO? selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                InstrumentDTO instrument = await instrumentApiClient.GetAsync(selected.Id);

                InstrumentDetail instrumentDetail = new InstrumentDetail(FormMode.Update, instrument);
                instrumentDetail.ShowDialog(this);
                await LoadInstruments();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo modificar el instrumento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            InstrumentDTO? instrument = this.SelectedItem();
            if (instrument == null) return;

            var result = MessageBox.Show($"Se van a eliminar también las calibraciones asociadas. ¿Eliminar {instrument.Code} - {instrument.Name}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await instrumentApiClient.DeleteAsync(instrument.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo eliminar el instrumento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    await LoadInstruments();
                }
            }
        }

        private async Task LoadInstruments()
        {
            try
            {
                DisableControls();
                this.instrumentsDataGridView.DataSource = null;

                // El filtrado lo resuelve el servidor; un criterio en null significa "no filtrar por eso"
                string? text = string.IsNullOrWhiteSpace(searchTextBox.Text) ? null : searchTextBox.Text.Trim();
                string? status = statusFilterComboBox.SelectedItem?.ToString();
                int? areaId = areaFilterComboBox.SelectedValue as int?;

                IEnumerable<InstrumentDTO> instruments = await instrumentApiClient.SearchAsync(text, status, areaId);

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
                MessageBox.Show($"No se pudieron cargar los instrumentos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private InstrumentDTO? SelectedItem()
        {
            if (instrumentsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Primero seleccione un instrumento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return instrumentsDataGridView.SelectedRows[0].DataBoundItem as InstrumentDTO;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            instrumentsDataGridView.Enabled = false;
            searchTextBox.Enabled = false;
            statusFilterComboBox.Enabled = false;
            areaFilterComboBox.Enabled = false;
            searchButton.Enabled = false;
            clearButton.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            instrumentsDataGridView.Enabled = true;
            searchTextBox.Enabled = true;
            statusFilterComboBox.Enabled = true;
            areaFilterComboBox.Enabled = true;
            searchButton.Enabled = true;
            clearButton.Enabled = true;
        }
    }
}