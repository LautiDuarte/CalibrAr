using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Mail;
using API.Auth.WindowsForms;
using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class UserDetail : Form
    {
        private readonly UserApiClient userApiClient = new(AuthServiceProvider.Instance);

        private UserDTO user = new();
        private FormMode mode;
        private bool isSelf;

        public UserDTO User
        {
            get { return user; }
            set
            {
                user = value;
                SetUser();
            }
        }

        public FormMode Mode
        {
            get { return mode; }
            set { SetFormMode(value); }
        }

        public UserDetail()
        {
            InitializeComponent();
        }

        public UserDetail(FormMode mode, UserDTO user) : this()
        {
            roleComboBox.DataSource = Enum.GetValues(typeof(UserRoleDTO));
            roleComboBox.SelectedIndex = -1;

            this.Mode = mode;
            this.User = user;
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
            bool isUpdate = mode == FormMode.Update;

            // La contraseña se define solo al crear; el estado activo se edita solo al actualizar
            passwordLabel.Visible = !isUpdate;
            passwordTextBox.Visible = !isUpdate;
            isActiveCheckBox.Visible = isUpdate;
        }

        private void SetUser()
        {
            fullNameTextBox.Text = User.FullName;
            emailTextBox.Text = User.Email;

            if (!string.IsNullOrEmpty(User.Role))
            {
                roleComboBox.SelectedItem = Enum.Parse<UserRoleDTO>(User.Role);
            }

            isActiveCheckBox.Checked = User.IsActive;

            // Nadie puede cambiarse el rol ni desactivarse a sí mismo
            isSelf = this.Mode == FormMode.Update && User.Id == AuthServiceProvider.Instance.GetUserId();
            roleComboBox.Enabled = !isSelf;
            isActiveCheckBox.Enabled = !isSelf;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateUser()) return;

            try
            {
                DisableControls();

                User.FullName = fullNameTextBox.Text.Trim();
                User.Email = emailTextBox.Text.Trim();
                User.Role = roleComboBox.SelectedItem!.ToString()!;

                if (this.Mode == FormMode.Update)
                {
                    User.IsActive = isActiveCheckBox.Checked;
                    User.RequestedByUserId = AuthServiceProvider.Instance.GetUserId();
                    await userApiClient.UpdateAsync(User);
                }
                else
                {
                    User.Password = passwordTextBox.Text;
                    await userApiClient.AddAsync(User);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static bool IsValidEmail(string email)
        {
            return MailAddress.TryCreate(email, out var address) && address.Address == email && address.Host.Contains('.');
        }

        private bool ValidateUser()
        {
            bool isValid = true;

            errorProvider.SetError(fullNameTextBox, string.Empty);
            errorProvider.SetError(emailTextBox, string.Empty);
            errorProvider.SetError(roleComboBox, string.Empty);
            errorProvider.SetError(passwordTextBox, string.Empty);

            if (string.IsNullOrWhiteSpace(fullNameTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(fullNameTextBox, "El nombre completo es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(emailTextBox, "El email es obligatorio");
            }
            else if (!IsValidEmail(emailTextBox.Text.Trim()))
            {
                isValid = false;
                errorProvider.SetError(emailTextBox, "El formato del email no es válido");
            }

            if (roleComboBox.SelectedItem == null)
            {
                isValid = false;
                errorProvider.SetError(roleComboBox, "El rol es obligatorio");
            }

            if (this.Mode == FormMode.Create && string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(passwordTextBox, "La contraseña es obligatoria");
            }

            return isValid;
        }

        private void DisableControls()
        {
            fullNameTextBox.Enabled = false;
            emailTextBox.Enabled = false;
            roleComboBox.Enabled = false;
            passwordTextBox.Enabled = false;
            isActiveCheckBox.Enabled = false;
        }

        private void EnableControls()
        {
            fullNameTextBox.Enabled = true;
            emailTextBox.Enabled = true;
            roleComboBox.Enabled = !isSelf;
            passwordTextBox.Enabled = true;
            isActiveCheckBox.Enabled = !isSelf;
        }
    }
}
