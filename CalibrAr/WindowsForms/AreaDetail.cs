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
    public partial class AreaDetail : Form
    {
        private readonly AreaApiClient areaApiClient = new(AuthServiceProvider.Instance);
        private readonly LocationApiClient locationApiClient = new(AuthServiceProvider.Instance);

        private AreaDTO area = new();
        private FormMode mode;

        public AreaDTO Area
        {
            get { return area; }
            set
            {
                area = value;
                this.SetArea();
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
        public AreaDetail()
        {
            InitializeComponent();
        }

        public AreaDetail(FormMode mode, AreaDTO area) : this()
        {
            Init(mode, area);
        }

        private async void Init(FormMode mode, AreaDTO area)
        {
            try
            {
                DisableControls();
                await LoadLocations();
                this.Mode = mode;
                this.Area = area;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async Task LoadLocations()
        {
            var locations = await locationApiClient.GetAllAsync();
            locationComboBox.DataSource = locations.ToList();
            locationComboBox.DisplayMember = "Name";
            locationComboBox.ValueMember = "Id";
            locationComboBox.SelectedIndex = -1;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateArea())
            {
                try
                {
                    DisableControls();

                    this.Area.Name = nameTextBox.Text;
                    this.Area.Responsible = responsibleTextBox.Text;
                    this.Area.LocationId = (int)locationComboBox.SelectedValue!;
                    if (this.Mode == FormMode.Update)
                    {
                        await areaApiClient.UpdateAsync(this.Area);
                    }
                    else
                    {
                        await areaApiClient.AddAsync(this.Area);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating area: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SetArea()
        {
            nameTextBox.Text = area.Name;
            responsibleTextBox.Text = area.Responsible;
            locationComboBox.SelectedValue = area.LocationId;
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
        }

        private bool ValidateArea()
        {
            bool isValid = true;

            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(locationComboBox, string.Empty);

            if (this.nameTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(nameTextBox, "Name is required");
            }
            if (this.locationComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(locationComboBox, "Location is required");
            }

            return isValid;
        }

        private void DisableControls()
        {
            nameTextBox.Enabled = false;
            responsibleTextBox.Enabled = false;
            locationComboBox.Enabled = false;
        }

        private void EnableControls()
        {
            nameTextBox.Enabled = true;
            responsibleTextBox.Enabled = true;
            locationComboBox.Enabled = true;
        }

        private void AreaDetail_Load(object sender, EventArgs e)
        {

        }
    }
}
