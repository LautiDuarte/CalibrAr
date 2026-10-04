using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using API.Clients;
using API.Auth.WindowsForms;
using DTOs;

namespace WindowsForms
{
    public partial class ProcedureList : Form
    {
        private readonly ProcedureApiClient procedureApiClient = new(AuthServiceProvider.Instance);

        public ProcedureList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            this.proceduresDataGridView.AutoGenerateColumns = false;

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Code",
                HeaderText = "Code",
                DataPropertyName = "Code",
                Width = 120
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 250
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VersionNumber",
                HeaderText = "Version number",
                DataPropertyName = "VersionNumber",
                Width = 120
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ApprovedAt",
                HeaderText = "Approved at",
                DataPropertyName = "ApprovedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InstrumentTypeName",
                HeaderText = "Instrument Type",
                DataPropertyName = "InstrumentTypeName",
                Width = 200
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is active",
                DataPropertyName = "IsActive",
                Width = 80
            });

            this.proceduresDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
        }

        private async void ProcedureList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadProcedures();
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = authService.HasPermission("Procedures.create");
            bool canUpdate = authService.HasPermission("Procedures.update");
            bool canDelete = authService.HasPermission("Procedures.delete");

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
            ProcedureDTO newProcedure = new ProcedureDTO();
            ProcedureDetail procedureDetail = new ProcedureDetail(FormMode.Create, newProcedure);

            procedureDetail.ShowDialog(this);
            await LoadProcedures();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            ProcedureDTO? selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                ProcedureDTO procedure = await procedureApiClient.GetAsync(selected.Id);

                ProcedureDetail procedureDetail = new ProcedureDetail(FormMode.Update, procedure);
                procedureDetail.ShowDialog(this);
                await LoadProcedures();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating procedure: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            ProcedureDTO? procedure = this.SelectedItem();
            if (procedure == null) return;


            var result = MessageBox.Show($"¿Are you sure you want to delete this item: {procedure.Code}{procedure.Name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await procedureApiClient.DeleteAsync(procedure.Id);
                    await LoadProcedures();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting procedure: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private async Task LoadProcedures()
        {
            try
            {
                DisableControls();
                this.proceduresDataGridView.DataSource = null;

                IEnumerable<ProcedureDTO> procedures = await procedureApiClient.GetAllAsync();

                this.proceduresDataGridView.DataSource = procedures;
                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.proceduresDataGridView.Rows.Count > 0)
                {
                    this.proceduresDataGridView.Rows[0].Selected = true;

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
                MessageBox.Show($"Error loading procedures: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private ProcedureDTO SelectedItem()
        {
            ProcedureDTO procedure;
            if (proceduresDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a procedure first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            procedure = (ProcedureDTO)proceduresDataGridView.SelectedRows[0].DataBoundItem;
            return procedure;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            proceduresDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            proceduresDataGridView.Enabled = true;
        }
    }
}
