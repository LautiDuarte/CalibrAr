using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class InstrumentTypeDetail : Form
    {
        private readonly InstrumentTypeApiClient instrumentTypeApiClient = new(AuthServiceProvider.Instance);

        private InstrumentTypeDTO instrumentType = new();
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

        private void Init(FormMode mode, InstrumentTypeDTO instrumentType)
        {
            try
            {
                DisableControls();
                this.Mode = mode;
                this.InstrumentType = instrumentType;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls();
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateInstrumentType(out decimal maxAllowedError, out int calibrationFrequencyMonths))
            {
                try
                {
                    DisableControls();

                    this.InstrumentType.Name = nameTextBox.Text;
                    this.InstrumentType.Description = descriptionTextBox.Text;
                    this.InstrumentType.MeasurementUnit = measurementUnitTextBox.Text;
                    this.InstrumentType.MaxAllowedError = maxAllowedError;
                    this.InstrumentType.CalibrationFrequencyMonths = calibrationFrequencyMonths;
                    if (this.Mode == FormMode.Update)
                    {
                        await instrumentTypeApiClient.UpdateAsync(this.InstrumentType);
                    }
                    else
                    {
                        await instrumentTypeApiClient.AddAsync(this.InstrumentType);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo crear el tipo de instrumento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool ValidateInstrumentType(out decimal maxAllowedError, out int calibrationFrequencyMonths)
        {
            bool isValid = true;
            maxAllowedError = 0;
            calibrationFrequencyMonths = 0;
            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(measurementUnitTextBox, string.Empty);
            errorProvider.SetError(maxAllowedErrorTextBox, string.Empty);
            errorProvider.SetError(calibrationFrequencyMonthsTextBox, string.Empty);

            if (this.nameTextBox.Text == string.Empty)
            {
                errorProvider.SetError(nameTextBox, "El nombre es obligatorio.");
                isValid = false;
            }

            if (this.measurementUnitTextBox.Text == string.Empty)
            {
                errorProvider.SetError(measurementUnitTextBox, "La unidad de medida es obligatoria.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(this.maxAllowedErrorTextBox.Text))
            {
                errorProvider.SetError(maxAllowedErrorTextBox, "El error máximo es obligatorio.");
                isValid = false;
            }
            else if (!DecimalParser.TryParse(this.maxAllowedErrorTextBox.Text, out maxAllowedError))
            {
                errorProvider.SetError(maxAllowedErrorTextBox, "El error máximo debe ser un número.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(this.calibrationFrequencyMonthsTextBox.Text))
            {
                errorProvider.SetError(calibrationFrequencyMonthsTextBox, "La frecuencia (meses) es obligatoria.");
                isValid = false;
            }
            else if (!int.TryParse(this.calibrationFrequencyMonthsTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out calibrationFrequencyMonths))
            {
                errorProvider.SetError(calibrationFrequencyMonthsTextBox, "La frecuencia (meses) debe ser un número entero.");
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