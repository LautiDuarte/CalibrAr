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
    public partial class CalibrationList : Form
    {
        private readonly CalibrationApiClient calibrationApiClient = new(AuthServiceProvider.Instance);
        private readonly InstrumentApiClient instrumentApiClient = new(AuthServiceProvider.Instance);

        public CalibrationList()
        {
            InitializeComponent();
            ConfigureColumns();
            EnumText.ApplyTo(calibrationsDataGridView, "InterventionType", "Result");

        }

        private void ConfigureColumns()
        {
            this.calibrationsDataGridView.AutoGenerateColumns = false;

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80,
                Visible = false
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InstrumentCode",
                HeaderText = "Instrumento",
                DataPropertyName = "InstrumentCode",
                Width = 120
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProcedureName",
                HeaderText = "Procedimiento",
                DataPropertyName = "ProcedureName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalibrationDate",
                HeaderText = "Fecha",
                DataPropertyName = "CalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InterventionType",
                HeaderText = "Tipo de intervención",
                DataPropertyName = "InterventionType",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsExternal",
                HeaderText = "Externa",
                DataPropertyName = "IsExternal",
                Width = 80
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExternalLab",
                HeaderText = "Laboratorio externo",
                DataPropertyName = "ExternalLab",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CertificateNumber",
                HeaderText = "N° de certificado",
                DataPropertyName = "CertificateNumber",
                Width = 150
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Result",
                HeaderText = "Resultado",
                DataPropertyName = "Result",
                Width = 150
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RestrictionDetail",
                HeaderText = "Detalle de restricciones",
                DataPropertyName = "RestrictionDetail",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PerformedByUserName",
                HeaderText = "Realizada por",
                DataPropertyName = "PerformedByUserName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ApprovedByUserName",
                HeaderText = "Aprobada por",
                DataPropertyName = "ApprovedByUserName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Notas",
                DataPropertyName = "Notes",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Fecha de alta",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
            this.calibrationsDataGridView.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Approve",
                HeaderText = string.Empty,
                Text = "Aprobar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });
        }

        private async void CalibrationList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadFilters();
            await LoadCalibrations();
        }

        private async Task LoadFilters()
        {
            try
            {
                EnumText.ApplyTo(resultFilterComboBox);
                resultFilterComboBox.DataSource = Enum.GetValues(typeof(ResultDTO));
                resultFilterComboBox.SelectedIndex = -1;

                var instruments = await instrumentApiClient.GetAllAsync();
                instrumentFilterComboBox.DataSource = instruments.ToList();
                instrumentFilterComboBox.DisplayMember = "Code";
                instrumentFilterComboBox.ValueMember = "Id";
                instrumentFilterComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los filtros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            if (fromDateTimePicker.Checked && toDateTimePicker.Checked
                && fromDateTimePicker.Value.Date > toDateTimePicker.Value.Date)
            {
                MessageBox.Show("La fecha \"Desde\" no puede ser posterior a \"Hasta\".", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadCalibrations();
        }

        private async void ClearButton_Click(object sender, EventArgs e)
        {
            fromDateTimePicker.Checked = false;
            toDateTimePicker.Checked = false;
            resultFilterComboBox.SelectedIndex = -1;
            instrumentFilterComboBox.SelectedIndex = -1;
            await LoadCalibrations();
        }


        private static bool CanApprove(CalibrationDTO calibration)
        {
            if (calibration.ApprovedByUserId.HasValue)
                return false;

            var authService = AuthServiceProvider.Instance;

            if (!authService.HasPermission("Calibrations.update"))
                return false;

            string? role = authService.GetRole();
            bool isAdmin = role == "Administrador";

            if (!isAdmin && role != "Responsable")
                return false;

            // Solo un administrador puede aprobar lo que él mismo realizó
            return isAdmin || calibration.PerformedByUserId != authService.GetUserId();
        }

        private void CalibrationsDataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (calibrationsDataGridView.Columns[e.ColumnIndex].Name != "Approve") return;

            // El DataGridView no permite ocultar el botón por fila: si no se puede aprobar, se pinta la celda vacía
            if (calibrationsDataGridView.Rows[e.RowIndex].DataBoundItem is CalibrationDTO calibration && !CanApprove(calibration))
            {
                e.PaintBackground(e.ClipBounds, (e.State & DataGridViewElementStates.Selected) != 0);
                e.Handled = true;
            }
        }

        private async void CalibrationsDataGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (calibrationsDataGridView.Columns[e.ColumnIndex].Name != "Approve") return;
            if (calibrationsDataGridView.Rows[e.RowIndex].DataBoundItem is not CalibrationDTO calibration) return;
            if (!CanApprove(calibration)) return;

            int? approverId = AuthServiceProvider.Instance.GetUserId();
            if (approverId == null) return;
            var result = MessageBox.Show(
                $"¿Aprobar la calibración de {calibration.InstrumentCode} {calibration.CalibrationDate:dd/MM/yyyy}?",
                "Confirmar aprobación",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK) return;

            try
            {
                DisableControls();
                await calibrationApiClient.ApproveAsync(calibration.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo aprobar la calibración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadCalibrations();
            }
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = authService.HasPermission("Calibrations.create");
            bool canUpdate = authService.HasPermission("Calibrations.update");
            bool canDelete = authService.HasPermission("Calibrations.delete");

            // Configurar visibilidad de botones según permisos
            createButton.Visible = canCreate;
            updateButton.Visible = canUpdate;
            deleteButton.Visible = canDelete;
            calibrationsDataGridView.Columns["Approve"].Visible = canUpdate;

            // Guardar permisos en Tag para uso posterior
            createButton.Tag = canCreate;
            updateButton.Tag = canUpdate;
            deleteButton.Tag = canDelete;
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            CalibrationDTO newCalibration = new CalibrationDTO();
            CalibrationDetail calibrationDetail = new CalibrationDetail(FormMode.Create, newCalibration);

            calibrationDetail.ShowDialog(this);
            await LoadCalibrations();
        }

        private async void UpdateButton_Click(object sender, EventArgs e)
        {
            CalibrationDTO? selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                CalibrationDTO calibration = await calibrationApiClient.GetAsync(selected.Id);

                CalibrationDetail calibrationDetail = new CalibrationDetail(FormMode.Update, calibration);
                calibrationDetail.ShowDialog(this);
                await LoadCalibrations();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo modificar la calibración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            CalibrationDTO? calibration = this.SelectedItem();
            if (calibration == null) return;

            var result = MessageBox.Show($"¿Eliminar {calibration.InstrumentCode} {calibration.CalibrationDate:dd/MM/yyyy}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await calibrationApiClient.DeleteAsync(calibration.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo eliminar la calibración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    await LoadCalibrations();
                }
            }
        }

        private async Task LoadCalibrations()
        {
            try
            {
                DisableControls();
                this.calibrationsDataGridView.DataSource = null;

                // El filtrado lo resuelve el servidor; un criterio en null significa "no filtrar por eso".
                // "Hasta" se manda como el final de ese día para que incluya las calibraciones del mismo día.
                DateTime? from = fromDateTimePicker.Checked ? fromDateTimePicker.Value.Date : null;
                DateTime? to = toDateTimePicker.Checked ? toDateTimePicker.Value.Date.AddDays(1).AddTicks(-1) : null;
                string? result = resultFilterComboBox.SelectedItem?.ToString();
                int? instrumentId = instrumentFilterComboBox.SelectedValue as int?;

                IEnumerable<CalibrationDTO> calibrations = await calibrationApiClient.SearchAsync(from, to, result, instrumentId);

                this.calibrationsDataGridView.DataSource = calibrations;
                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.calibrationsDataGridView.Rows.Count > 0)
                {
                    this.calibrationsDataGridView.Rows[0].Selected = true;

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
                MessageBox.Show($"No se pudieron cargar las calibraciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private CalibrationDTO? SelectedItem()
        {
            CalibrationDTO? calibration;
            if (calibrationsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Primero seleccione una calibración.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            calibration = calibrationsDataGridView.SelectedRows[0].DataBoundItem as CalibrationDTO;
            return calibration;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            calibrationsDataGridView.Enabled = false;
            fromDateTimePicker.Enabled = false;
            toDateTimePicker.Enabled = false;
            resultFilterComboBox.Enabled = false;
            instrumentFilterComboBox.Enabled = false;
            searchButton.Enabled = false;
            clearButton.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            calibrationsDataGridView.Enabled = true;
            fromDateTimePicker.Enabled = true;
            toDateTimePicker.Enabled = true;
            resultFilterComboBox.Enabled = true;
            instrumentFilterComboBox.Enabled = true;
            searchButton.Enabled = true;
            clearButton.Enabled = true;
        }
    }
}