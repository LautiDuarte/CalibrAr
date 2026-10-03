using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class CalibrationMeasurementDetail : Form
    {
        private CalibrationMeasurementDTO measurement;
        private FormMode mode;
        private decimal maxAllowedError;

        public CalibrationMeasurementDTO Measurement
        {
            get { return measurement; }
            set
            {
                measurement = value;
                this.SetMeasurement();
            }
        }

        public FormMode Mode
        {
            get { return mode; }
            set
            {
                SetFormMode(value);
            }
        }

        public CalibrationMeasurementDetail()
        {
            InitializeComponent();
        }

        public CalibrationMeasurementDetail(FormMode mode, CalibrationMeasurementDTO measurement, decimal maxAllowedError) : this()
        {
            this.maxAllowedError = maxAllowedError;
            Init(mode, measurement);
        }

        private void Init(FormMode mode, CalibrationMeasurementDTO measurement)
        {
            try
            {
                DisableControls();

                this.Mode = mode;
                this.Measurement = measurement;
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateMeasurement())
            {
                this.Measurement.NominalValue = decimal.Parse(nominalValueTextBox.Text);
                this.Measurement.MeasuredValue = decimal.Parse(measuredValueTextBox.Text);
                this.Measurement.Error = this.Measurement.MeasuredValue - this.Measurement.NominalValue;
                this.Measurement.IsWithinTolerance = Math.Abs(this.Measurement.Error) <= maxAllowedError;
                this.Measurement.Notes = notesTextBox.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetMeasurement()
        {
            this.nominalValueTextBox.Text = this.Measurement.NominalValue.ToString("F3");
            this.measuredValueTextBox.Text = this.Measurement.MeasuredValue.ToString("F3");
            this.notesTextBox.Text = this.Measurement.Notes;
            UpdateCalculatedValues();
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
        }

        private bool ValidateMeasurement()
        {
            bool isValid = true;

            errorProvider.SetError(nominalValueTextBox, string.Empty);
            errorProvider.SetError(measuredValueTextBox, string.Empty);

            if (string.IsNullOrWhiteSpace(this.nominalValueTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(nominalValueTextBox, "Nominal value is required");
            }
            else if (!decimal.TryParse(this.nominalValueTextBox.Text, out decimal nominal))
            {
                isValid = false;
                errorProvider.SetError(nominalValueTextBox, "Nominal value must be a number");
            }

            if (string.IsNullOrWhiteSpace(this.measuredValueTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(measuredValueTextBox, "Measured value is required");
            }
            else if (!decimal.TryParse(this.measuredValueTextBox.Text, out decimal measured))
            {
                isValid = false;
                errorProvider.SetError(measuredValueTextBox, "Measured value must be a number");
            }

            return isValid;
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            // Auto-completar Error e IsWithinTolerance en base a los valores ingresados
            UpdateCalculatedValues();
        }

        private void UpdateCalculatedValues()
        {
            if (decimal.TryParse(nominalValueTextBox.Text, out decimal nominal)
                && decimal.TryParse(measuredValueTextBox.Text, out decimal measured))
            {
                decimal error = measured - nominal;
                errorTextBox.Text = error.ToString("F3");
                isWithinToleranceCheckBox.Checked = Math.Abs(error) <= maxAllowedError;
            }
            else
            {
                errorTextBox.Text = string.Empty;
                isWithinToleranceCheckBox.Checked = false;
            }
        }

        private void DisableControls()
        {
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
            nominalValueTextBox.Enabled = false;
            measuredValueTextBox.Enabled = false;
            notesTextBox.Enabled = false;
        }

        private void EnableControls()
        {
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
            nominalValueTextBox.Enabled = true;
            measuredValueTextBox.Enabled = true;
            notesTextBox.Enabled = true;
        }
    }
}