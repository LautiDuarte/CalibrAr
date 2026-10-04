using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTOs;
using API.Clients;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsForms
{

    public enum FormMode
    {
        Create,
        Update
    }
    public partial class InstrumentDetail : Form
    {
        private InstrumentDTO instrument = new();
        private FormMode mode;

        public InstrumentDTO Instrument
        {
            get { return instrument; }
            set
            {
                instrument = value;
                this.SetInstrument();
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
        public InstrumentDetail()
        {
            InitializeComponent();
        }

        public InstrumentDetail(FormMode mode, InstrumentDTO instrument) : this()
        {
            Init(mode, instrument);
        }

        private async void Init(FormMode mode, InstrumentDTO instrument)
        {
            try
            {
                DisableControls();

                LoadStatus();
                await LoadInstrumentTypes();
                await LoadAreas();

                this.Mode = mode;
                this.Instrument = instrument;
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

        private void LoadStatus()
        {
            statusComboBox.DataSource = Enum.GetValues(typeof(InstrumentStatusDTO));
            statusComboBox.SelectedIndex = -1;
        }

        private async Task LoadInstrumentTypes()
        {
            var instrumentTypes = await InstrumentTypeApiClient.GetAllAsync();
            instrumentTypeComboBox.DataSource = instrumentTypes.ToList();
            instrumentTypeComboBox.DisplayMember = "Name";
            instrumentTypeComboBox.ValueMember = "Id";
            instrumentTypeComboBox.SelectedIndex = -1;
        }

        private async Task LoadAreas()
        {
            var areas = await AreaApiClient.GetAllAsync();
            areaComboBox.DataSource = areas.ToList();
            areaComboBox.DisplayMember = "Name";
            areaComboBox.ValueMember = "Id";
            areaComboBox.SelectedIndex = -1;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateInstrument())
            {
                try
                {
                    DisableControls();

                    this.Instrument.Code = codeTextBox.Text.Trim();
                    this.Instrument.Name = nameTextBox.Text;
                    this.Instrument.SerialNumber = serialNumberTextBox.Text;
                    this.Instrument.Brand = brandTextBox.Text;
                    this.Instrument.Model = modelTextBox.Text;
                    if (this.Mode == FormMode.Update && statusComboBox.SelectedItem != null)
                    {
                        this.Instrument.Status = statusComboBox.SelectedItem.ToString() ?? string.Empty;
                    }
                    else
                    {
                        this.Instrument.Status = InstrumentStatusDTO.Active.ToString();
                    }

                    // MaxAllowedError: si está vacío -> null, si no -> parsear con TryParse
                    if (string.IsNullOrWhiteSpace(maxAllowedErrorTextBox.Text))
                    {
                        this.Instrument.MaxAllowedError = null;
                    }
                    else if (decimal.TryParse(maxAllowedErrorTextBox.Text, out var maxErrValue))
                    {
                        this.Instrument.MaxAllowedError = maxErrValue;
                    }
                    else
                    {
                        MessageBox.Show("Max allowed error must be a number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // CalibrationFrequencyMonths: si está vacío -> null, si no -> parsear con TryParse
                    if (string.IsNullOrWhiteSpace(calibrationFrequencyMonthsTextBox.Text))
                    {
                        this.Instrument.CalibrationFrequencyMonths = null;
                    }
                    else if (int.TryParse(calibrationFrequencyMonthsTextBox.Text, out var calibFreqValue))
                    {
                        this.Instrument.CalibrationFrequencyMonths = calibFreqValue;
                    }
                    else
                    {
                        MessageBox.Show("Calibration frequency (months) must be a number", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.Instrument.InstrumentTypeId = (int)instrumentTypeComboBox.SelectedValue;
                    this.Instrument.AreaId = (int)areaComboBox.SelectedValue;
                    if (this.Mode == FormMode.Update)
                    {
                        await InstrumentApiClient.UpdateAsync(this.Instrument);
                    }
                    else
                    {
                        await InstrumentApiClient.AddAsync(this.Instrument);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating instrument: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SetInstrument()
        {
            this.codeTextBox.Text = this.Instrument.Code;
            this.nameTextBox.Text = this.Instrument.Name;
            this.serialNumberTextBox.Text = this.Instrument.SerialNumber;
            this.brandTextBox.Text = this.Instrument.Brand;
            this.modelTextBox.Text = this.Instrument.Model;
            if (!string.IsNullOrEmpty(this.Instrument.Status))
            {
                this.statusComboBox.SelectedItem =
                    Enum.Parse<InstrumentStatusDTO>(this.Instrument.Status);
            }

            // Manejo null-safe para MaxAllowedError y CalibrationFrequencyMonths
            this.maxAllowedErrorTextBox.Text = this.Instrument.MaxAllowedError.HasValue
                ? this.Instrument.MaxAllowedError.Value.ToString()
                : string.Empty;

            this.calibrationFrequencyMonthsTextBox.Text = this.Instrument.CalibrationFrequencyMonths.HasValue
                ? this.Instrument.CalibrationFrequencyMonths.Value.ToString()
                : string.Empty;

            this.instrumentTypeComboBox.SelectedValue = this.Instrument.InstrumentTypeId;
            this.areaComboBox.SelectedValue = this.Instrument.AreaId;
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;

            if (Mode == FormMode.Create)
            {
                statusLabel.Visible = false;
                statusComboBox.Visible = false;
            }

            if (Mode == FormMode.Update)
            {
                statusLabel.Visible = true;
                statusComboBox.Visible = true;
                instrumentTypeLabel.Visible = false;
                instrumentTypeComboBox.Visible = false;
                InstrumentTypeWarningLabel.Visible = false;
            }
        }

        private bool ValidateInstrument()
        {
            bool isValid = true;

            errorProvider.SetError(codeTextBox, string.Empty);
            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(statusComboBox, string.Empty);
            errorProvider.SetError(instrumentTypeComboBox, string.Empty);
            errorProvider.SetError(areaComboBox, string.Empty);

            if (this.codeTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(codeTextBox, "Code is required");
            }

            if (this.nameTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(nameTextBox, "Name is required");
            }

            if (this.statusComboBox.SelectedItem == null && Mode == FormMode.Update)
            {
                isValid = false;
                errorProvider.SetError(statusComboBox, "Status is required");
            }

            if (this.instrumentTypeComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(instrumentTypeComboBox, "Instrument Type is required");
            }

            if (this.areaComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(areaComboBox, "Area is required");
            }

            return isValid;
        }

        private void DisableControls()
        {
            codeTextBox.Enabled = false;
            nameTextBox.Enabled = false;
            serialNumberTextBox.Enabled = false;
            brandTextBox.Enabled = false;
            modelTextBox.Enabled = false;
            statusComboBox.Enabled = false;
            maxAllowedErrorTextBox.Enabled = false;
            calibrationFrequencyMonthsTextBox.Enabled = false;
            instrumentTypeComboBox.Enabled = false;
            areaComboBox.Enabled = false;
        }

        private void EnableControls()
        {
            codeTextBox.Enabled = true;
            nameTextBox.Enabled = true;
            serialNumberTextBox.Enabled = true;
            brandTextBox.Enabled = true;
            modelTextBox.Enabled = true;
            statusComboBox.Enabled = true;
            maxAllowedErrorTextBox.Enabled = true;
            calibrationFrequencyMonthsTextBox.Enabled = true;
            instrumentTypeComboBox.Enabled = true;
            areaComboBox.Enabled = true;
        }

        private void IntrumentDetail_Load(object sender, EventArgs e)
        {

        }

    }
}