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
    public partial class InstrumentTypeDetail : Form
    {
        private InstrumentTypeDTO instrumentType;
        private FormMode mode;

        public InstrumentTypeDTO InstrumentType
        {
            get { return instrumentType; }
            set
            {
                instrumentType = value;
                this.SetInstrumentType();
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
        public InstrumentTypeDetail()
        {
            InitializeComponent();
        }

        public InstrumentTypeDetail(FormMode mode, InstrumentTypeDTO instrumentType) : this()
        {
            Init(mode, instrumentType);
        }

        private async void Init(FormMode mode, InstrumentTypeDTO instrumentType)
        {
            try
            {
                DisableControls();
                this.Mode = mode;
                this.InstrumentType = instrumentType;
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
            if (this.ValidateInstrumentType())
            {
                try
                {
                    DisableControls();

                    this.InstrumentType.Name = nameTextBox.Text;
                    this.InstrumentType.Description = descriptionTextBox.Text;
                    this.InstrumentType.MeasurementUnit = measurementUnitTextBox.Text;
                    this.InstrumentType.MaxAllowedError = decimal.Parse(maxAllowedErrorTextBox.Text);
                    this.InstrumentType.CalibrationFrequencyMonths = int.Parse(calibrationFrequencyMonthsTextBox.Text);
                    if (this.Mode == FormMode.Update)
                    {
                        await InstrumentTypeApiClient.UpdateAsync(this.InstrumentType);
                    }
                    else
                    {
                        await InstrumentTypeApiClient.AddAsync(this.InstrumentType);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating instrument type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SetInstrumentType()
        {
            this.nameTextBox.Text = this.InstrumentType.Name;
            this.descriptionTextBox.Text = this.InstrumentType.Description;
            this.measurementUnitTextBox.Text = this.InstrumentType.MeasurementUnit;
            this.maxAllowedErrorTextBox.Text = this.InstrumentType.MaxAllowedError.ToString();
            this.calibrationFrequencyMonthsTextBox.Text = this.InstrumentType.CalibrationFrequencyMonths.ToString();
        }

        private bool ValidateInstrumentType()
        {
            bool isValid = true;
            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(measurementUnitTextBox, string.Empty);
            errorProvider.SetError(maxAllowedErrorTextBox, string.Empty);
            errorProvider.SetError(calibrationFrequencyMonthsTextBox, string.Empty);

            if (this.nameTextBox.Text == string.Empty)
            {
                errorProvider.SetError(nameTextBox, "Name is required.");
                isValid = false;
            }

            if (this.measurementUnitTextBox.Text == string.Empty)
            {
                errorProvider.SetError(measurementUnitTextBox, "Measurement Unit is required.");
                isValid = false;
            }

            if (this.maxAllowedErrorTextBox.Text == string.Empty)
            {
                errorProvider.SetError(maxAllowedErrorTextBox, "Max Allowed Error is required.");
                isValid = false;
            }

            if (this.calibrationFrequencyMonthsTextBox.Text == string.Empty)
            {
                errorProvider.SetError(calibrationFrequencyMonthsTextBox, "Calibration Frequency (Months) is required.");
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
            descriptionTextBox.Enabled = false;
            measurementUnitTextBox.Enabled = false;
            maxAllowedErrorTextBox.Enabled = false;
            calibrationFrequencyMonthsTextBox.Enabled = false;
        }
        private void EnableControls()
        {
            nameTextBox.Enabled = true;
            descriptionTextBox.Enabled = true;
            measurementUnitTextBox.Enabled = true;
            maxAllowedErrorTextBox.Enabled = true;
            calibrationFrequencyMonthsTextBox.Enabled = true;
        }

        private void InstrumentTypeDetail_Load(object sender, EventArgs e)
        {

        }
    }
}
