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
    public partial class LocationList : Form
    {
        public LocationList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            this.locationsDataGridView.AutoGenerateColumns = false;
            this.locationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });
            this.locationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 200
            });
            this.locationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                HeaderText = "Address",
                DataPropertyName = "Address",
                Width = 300
            });
            this.locationsDataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                HeaderText = "Is active",
                DataPropertyName = "IsActive",
                Width = 80
            });

            this.locationsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Created at",
                DataPropertyName = "CreatedAt",
                Width = 150,
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm:ss" }
            });
        }

        private async void LocationList_Load(object sender, EventArgs e)
        {
            ConfigureButonPermissions();
            await LoadLocations();
        }

        private void ConfigureButonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            // Verificar permisos para cada botón
            bool canCreate = authService.HasPermission("Locations.create");
            bool canUpdate = authService.HasPermission("Locations.update");
            bool canDelete = authService.HasPermission("Locations.delete");

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
            LocationDTO newLocation = new LocationDTO();
            LocationDetail locationDetail = new LocationDetail(FormMode.Create, newLocation);

            locationDetail.ShowDialog(this);
            await LoadLocations();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            LocationDTO? selected = this.SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();

                LocationDTO location = await LocationApiClient.GetAsync(selected.Id);

                LocationDetail locationDetail = new LocationDetail(FormMode.Update, location);
                locationDetail.ShowDialog(this);
                await LoadLocations();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating location: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            LocationDTO? location = this.SelectedItem();
            if (location == null) return;

            var result = MessageBox.Show($"¿Are you sure you want to delete this item: {location.Name} {location.Address}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DisableControls();
                    await LocationApiClient.DeleteAsync(location.Id);
                    await LoadLocations();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting location: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private async Task LoadLocations()
        {
            try
            {
                DisableControls();
                this.locationsDataGridView.DataSource = null;

                IEnumerable<LocationDTO> locations = await LocationApiClient.GetAllAsync();

                this.locationsDataGridView.DataSource = locations;

                // Solo manejar Enabled/Disabled si los botones son visibles (tienen permisos)
                bool canUpdate = updateButton.Tag is bool updatePermission && updatePermission;
                bool canDelete = deleteButton.Tag is bool deletePermission && deletePermission;

                if (this.locationsDataGridView.Rows.Count > 0)
                {
                    this.locationsDataGridView.Rows[0].Selected = true;

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
                MessageBox.Show($"Error loading locations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private LocationDTO? SelectedItem()
        {
            LocationDTO location;
            if (locationsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a location first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return (LocationDTO)locationsDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            locationsDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            locationsDataGridView.Enabled = true;
        }
    }
}

