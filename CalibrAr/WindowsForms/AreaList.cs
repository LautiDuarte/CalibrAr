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
    public partial class AreaList : Form
    {
        public AreaList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            this.areasDataGridView.AutoGenerateColumns = false;
            this.areasDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });
            this.areasDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 200
            });
            this.areasDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Responsible",
                HeaderText = "Responsible",
                DataPropertyName = "Responsible",
                Width = 200
            });
            this.areasDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is active",
                DataPropertyName = "IsActive",
                Width = 80
            });
            this.areasDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
            this.areasDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LocationName",
                HeaderText = "Location",
                DataPropertyName = "LocationName",
                Width = 200
            });
        }

        private async void AreaList_Load(object sender, EventArgs e)
        {
            ConfigureButonPermissions();
            await LoadAreas();
        }

        private void ConfigureButonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = authService.HasPermission("Areas.create");
            bool canUpdate = authService.HasPermission("Areas.update");
            bool canDelete = authService.HasPermission("Areas.delete");

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
            AreaDTO newArea = new AreaDTO();
            AreaDetail areaDetail = new AreaDetail(FormMode.Create, newArea);

            areaDetail.ShowDialog(this);
            await LoadAreas();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            AreaDTO selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                AreaDTO area = await AreaApiClient.GetAsync(selected.Id);

                AreaDetail areaDetail = new AreaDetail(FormMode.Update, area);
                areaDetail.ShowDialog(this);
                await LoadAreas();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating area: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            AreaDTO area = this.SelectedItem();
            if(area == null) return;

            var result = MessageBox.Show($"¿Are you sure you want to delete this item:{area.Name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await AreaApiClient.DeleteAsync(area.Id);
                    await LoadAreas();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting area: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private async Task LoadAreas()
        {
            try
            {
                DisableControls();
                this.areasDataGridView.DataSource = null;

                IEnumerable<AreaDTO> areas = await AreaApiClient.GetAllAsync();

                this.areasDataGridView.DataSource = areas;
                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.areasDataGridView.Rows.Count > 0)
                {
                    this.areasDataGridView.Rows[0].Selected = true;

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
                MessageBox.Show($"Error loading areas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private AreaDTO? SelectedItem()
        {
            AreaDTO area;
            if (areasDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an area first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return (AreaDTO)areasDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            areasDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            areasDataGridView.Enabled = true;
        }
    }
}
