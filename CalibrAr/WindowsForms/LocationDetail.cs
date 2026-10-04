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
    public partial class LocationDetail : Form
    {
        private readonly LocationApiClient locationApiClient = new(AuthServiceProvider.Instance);


        private LocationDTO location = new();
        private FormMode mode;

        public new LocationDTO Location
        {
            get { return location; }
            set
            {
                location = value;
                this.SetLocation();
            }
        }

        public FormMode Mode
        {
            get { return mode; }
            set
            {
                this.SetFormMode(value);
            }
        }
        public LocationDetail()
        {
            InitializeComponent();
        }

        public LocationDetail(FormMode mode, LocationDTO location) : this()
        {
            Init(mode, location);
        }

        private void Init(FormMode mode, LocationDTO location)
        {
            try
            {
                DisableControls();
                this.Mode = mode;
                this.Location = location;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateLocation())
            {
                try
                {
                    DisableControls();

                    this.Location.Name = nameTextBox.Text;
                    this.Location.Address = addressTextBox.Text;
                    if (this.Mode == FormMode.Update)
                    {
                        await locationApiClient.UpdateAsync(this.Location);
                    }
                    else
                    {
                        await locationApiClient.AddAsync(this.Location);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating location: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EnableControls();
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetLocation()
        {
            this.nameTextBox.Text = location.Name;
            this.addressTextBox.Text = location.Address;
        }

        private bool ValidateLocation()
        {
            bool isValid = true;
            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(addressTextBox, string.Empty);


            if (this.nameTextBox.Text == string.Empty)
            {
                errorProvider.SetError(nameTextBox, "Name is required.");
                isValid = false;
            }

            if (this.addressTextBox.Text == string.Empty)
            {
                errorProvider.SetError(addressTextBox, "Address is required.");
                isValid = false;
            }
            return isValid;
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
        }

        private void DisableControls()
        {
            nameTextBox.Enabled = false;
            addressTextBox.Enabled = false;
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
        }

        private void EnableControls()
        {
            nameTextBox.Enabled = true;
            addressTextBox.Enabled = true;
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void LocationDetail_Load(object sender, EventArgs e)
        {

        }
    }
}
