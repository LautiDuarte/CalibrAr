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
        private readonly UserApiClient userApiClient = new(AuthServiceProvider.Instance);
        private UserDTO? currentUser;

        public CalibrationList()
        {
            InitializeComponent();
            ConfigureColumns();

        }

        private void ConfigureColumns()
        {
            this.calibrationsDataGridView.AutoGenerateColumns = false;

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InstrumentCode",
                HeaderText = "Instrument",
                DataPropertyName = "InstrumentCode",
                Width = 120
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProcedureName",
                HeaderText = "Procedure",
                DataPropertyName = "ProcedureName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalibrationDate",
                HeaderText = "Calibration date",
                DataPropertyName = "CalibrationDate",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InterventionType",
                HeaderText = "Intervention type",
                DataPropertyName = "InterventionType",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsExternal",
                HeaderText = "Is external",
                DataPropertyName = "IsExternal",
                Width = 80
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExternalLab",
                HeaderText = "External lab",
                DataPropertyName = "ExternalLab",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CertificateNumber",
                HeaderText = "Certificate number",
                DataPropertyName = "CertificateNumber",
                Width = 150
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Result",
                HeaderText = "Result",
                DataPropertyName = "Result",
                Width = 150
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RestrictionDetail",
                HeaderText = "Restriction detail",
                DataPropertyName = "RestrictionDetail",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PerformedByUserName",
                HeaderText = "Performed by",
                DataPropertyName = "PerformedByUserName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ApprovedByUserName",
                HeaderText = "Approved by",
                DataPropertyName = "ApprovedByUserName",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Notes",
                DataPropertyName = "Notes",
                Width = 200
            });

            this.calibrationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
            this.calibrationsDataGridView.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Approve",
                HeaderText = string.Empty,
                Text = "Approve",
                UseColumnTextForButtonValue = true,
                Width = 80
            });
        }

        private async void CalibrationList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadCurrentUser();
            await LoadCalibrations();
        }

        private async Task LoadCurrentUser()
        {
            try
            {
                string? sessionEmail = AuthServiceProvider.Instance.GetEmail();
                var users = await userApiClient.GetAllAsync();
                currentUser = users.FirstOrDefault(u => u.Email == sessionEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading current user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CanApprove(CalibrationDTO calibration)
        {
            if (calibration.ApprovedByUserId.HasValue || currentUser == null)
                return false;

            if (!AuthServiceProvider.Instance.HasPermission("Calibrations.update"))
                return false;

            string role = currentUser.Role.ToString();
            bool isAdmin = role == "Administrador";

            if (!isAdmin && role != "Responsable")
                return false;

            // Solo un administrador puede aprobar lo que él mismo realizó
            return isAdmin || calibration.PerformedByUserId != currentUser.Id;
        }

        private void calibrationsDataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
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

        private async void calibrationsDataGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (calibrationsDataGridView.Columns[e.ColumnIndex].Name != "Approve") return;
            if (calibrationsDataGridView.Rows[e.RowIndex].DataBoundItem is not CalibrationDTO calibration) return;
            if (!CanApprove(calibration)) return;

            var result = MessageBox.Show(
                $"Are you sure you want to approve this calibration: {calibration.InstrumentCode} {calibration.CalibrationDate:dd/MM/yyyy}?",
                "Confirm approval",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK) return;

            try
            {
                DisableControls();
                await calibrationApiClient.ApproveAsync(calibration.Id, currentUser!.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving calibration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void createButton_Click(object sender, EventArgs e)
        {
            CalibrationDTO newCalibration = new CalibrationDTO();
            CalibrationDetail calibrationDetail = new CalibrationDetail(FormMode.Create, newCalibration);

            calibrationDetail.ShowDialog(this);
            await LoadCalibrations();
        }

        private async void updateButton_Click(object sender, EventArgs e)
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
                MessageBox.Show($"Error updating calibration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            CalibrationDTO? calibration = this.SelectedItem();
            if (calibration == null) return;

            var result = MessageBox.Show($"¿Are you sure you want to delete this item: {calibration.InstrumentCode} {calibration.CalibrationDate:dd/MM/yyyy}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await calibrationApiClient.DeleteAsync(calibration.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting calibration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                IEnumerable<CalibrationDTO> calibrations = await calibrationApiClient.GetAllAsync();

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
                MessageBox.Show($"Error loading calibrations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private CalibrationDTO SelectedItem()
        {
            CalibrationDTO calibration;
            if (calibrationsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an calibration first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            calibration = (CalibrationDTO)calibrationsDataGridView.SelectedRows[0].DataBoundItem;
            return calibration;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            calibrationsDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            calibrationsDataGridView.Enabled = true;
        }
    }
}
