using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using API.Auth.WindowsForms;
using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class UserList : Form
    {
        private readonly UserApiClient userApiClient = new(AuthServiceProvider.Instance);

        public UserList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void AddColumn(string name, string header, int width, string? format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width };
            if (format != null) column.DefaultCellStyle.Format = format;
            usersDataGridView.Columns.Add(column);
        }

        private void ConfigureColumns()
        {
            usersDataGridView.AutoGenerateColumns = false;
            AddColumn("FullName", "Full name", 200);
            AddColumn("Email", "Email", 220);
            AddColumn("Role", "Role", 110);
            AddColumn("GroupName", "Permission group", 130);
            AddColumn("IsActive", "Active", 60);
            AddColumn("LastLoginAt", "Last login", 140, "dd/MM/yyyy HH:mm");
            AddColumn("CreatedAt", "Created at", 140, "dd/MM/yyyy HH:mm");
        }

        private async void UserList_Load(object sender, EventArgs e)
        {
            ConfigureButtonPermissions();
            await LoadUsers();
        }

        private void ConfigureButtonPermissions()
        {
            var authService = AuthServiceProvider.Instance;

            bool canCreate = authService.HasPermission("Users.create");
            bool canUpdate = authService.HasPermission("Users.update");
            bool canDelete = authService.HasPermission("Users.delete");

            createButton.Visible = canCreate;
            updateButton.Visible = canUpdate;
            deleteButton.Visible = canDelete;

            updateButton.Tag = canUpdate;
            deleteButton.Tag = canDelete;
        }

        private async void createButton_Click(object sender, EventArgs e)
        {
            UserDetail detail = new UserDetail(FormMode.Create, new UserDTO());
            detail.ShowDialog(this);
            await LoadUsers();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            UserDTO? selected = SelectedItem();
            if (selected == null) return;
            try
            {
                DisableControls();
                UserDTO user = await userApiClient.GetAsync(selected.Id);
                UserDetail detail = new UserDetail(FormMode.Update, user);
                detail.ShowDialog(this);
                await LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            UserDTO? user = SelectedItem();
            if (user == null) return;

            int? sessionUserId = AuthServiceProvider.Instance.GetUserId();
            if (user.Id == sessionUserId)
            {
                MessageBox.Show("You cannot delete your own user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete this item: {user.FullName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                DisableControls();
                await userApiClient.DeleteAsync(user.Id, sessionUserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadUsers();
            }
        }

        private async Task LoadUsers()
        {
            try
            {
                DisableControls();
                usersDataGridView.DataSource = null;
                usersDataGridView.DataSource = await userApiClient.GetAllAsync();

                bool canUpdate = updateButton.Tag is bool u && u;
                bool canDelete = deleteButton.Tag is bool d && d;
                bool hasRows = usersDataGridView.Rows.Count > 0;

                if (hasRows) usersDataGridView.Rows[0].Selected = true;
                if (canUpdate) updateButton.Enabled = hasRows;
                if (canDelete) deleteButton.Enabled = hasRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private UserDTO? SelectedItem()
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return (UserDTO)usersDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void DisableControls()
        {
            createButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            usersDataGridView.Enabled = false;
        }

        private void EnableControls()
        {
            createButton.Enabled = true;
            usersDataGridView.Enabled = true;
        }
    }
}
