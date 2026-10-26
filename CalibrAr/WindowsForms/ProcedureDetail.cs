using System;
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
using API.Auth.WindowsForms;

namespace WindowsForms
{
    public partial class ProcedureDetail : Form
    {
        private readonly InstrumentTypeApiClient instrumentTypeApiClient = new(AuthServiceProvider.Instance);
        private readonly ProcedureApiClient procedureApiClient = new(AuthServiceProvider.Instance);

        private ProcedureDTO procedure;
        private FormMode mode;

        public ProcedureDTO Procedure
        {
            get { return procedure; }
            set
            {
                procedure = value;
                this.SetProcedure();
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
        public ProcedureDetail()
        {
            InitializeComponent();
            approvedAtDateTimePicker.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
        }

        public ProcedureDetail(FormMode mode, ProcedureDTO procedure) : this()
        {
            Init(mode, procedure);
        }

        private async void Init(FormMode mode, ProcedureDTO procedure)
        {
            try
            {
                DisableControls();

                await LoadInstrumentTypes();

                this.Mode = mode;
                this.Procedure = procedure;
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

        private async Task LoadInstrumentTypes()
        {
            var instrumentTypes = await instrumentTypeApiClient.GetAllAsync();
            instrumentTypeComboBox.DataSource = instrumentTypes.ToList();
            instrumentTypeComboBox.DisplayMember = "Name";
            instrumentTypeComboBox.ValueMember = "Id";
            instrumentTypeComboBox.SelectedIndex = -1;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateProcedure())
            {
                try
                {
                    DisableControls();

                    this.Procedure.Code = codeTextBox.Text;
                    this.Procedure.Name = nameTextBox.Text;
                    this.Procedure.VersionNumber = versionNumberTextBox.Text;
                    this.Procedure.ApprovedAt = approvedAtDateTimePicker.Value;
                    this.Procedure.InstrumentTypeId = (int)instrumentTypeComboBox.SelectedValue;
                    if (this.Mode == FormMode.Update)
                    {
                        await procedureApiClient.UpdateAsync(this.Procedure);
                    }
                    else
                    {
                        await procedureApiClient.AddAsync(this.Procedure);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo crear el procedimiento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SetProcedure()
        {
            this.codeTextBox.Text = this.Procedure.Code;
            this.nameTextBox.Text = this.Procedure.Name;
            this.versionNumberTextBox.Text = this.Procedure.VersionNumber;

            if (this.Procedure.ApprovedAt != default(DateTime))
            {
                this.approvedAtDateTimePicker.Value = this.Procedure.ApprovedAt;
            }

            this.instrumentTypeComboBox.SelectedValue = this.Procedure.InstrumentTypeId;
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
        }

        private bool ValidateProcedure()
        {
            bool isValid = true;

            errorProvider.SetError(codeTextBox, string.Empty);
            errorProvider.SetError(nameTextBox, string.Empty);
            errorProvider.SetError(versionNumberTextBox, string.Empty);
            errorProvider.SetError(approvedAtDateTimePicker, string.Empty);
            errorProvider.SetError(instrumentTypeComboBox, string.Empty);

            if (this.codeTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(codeTextBox, "El código es obligatorio");
            }

            if (this.nameTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(nameTextBox, "El nombre es obligatorio");
            }

            if (this.versionNumberTextBox.Text == string.Empty)
            {
                isValid = false;
                errorProvider.SetError(versionNumberTextBox, "La versión es obligatoria");
            }

            if (this.approvedAtDateTimePicker.Checked == false)
            {
                isValid = false;
                errorProvider.SetError(approvedAtDateTimePicker, "La fecha de aprobación es obligatoria");
            }

            if (this.instrumentTypeComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(instrumentTypeComboBox, "El tipo de instrumento es obligatorio");
            }

            return isValid;
        }

        private void DisableControls()
        {
            codeTextBox.Enabled = false;
            nameTextBox.Enabled = false;
            versionNumberTextBox.Enabled = false;
            approvedAtDateTimePicker.Enabled = false;
            instrumentTypeComboBox.Enabled = false;
        }

        private void EnableControls()
        {
            codeTextBox.Enabled = true;
            nameTextBox.Enabled = true;
            versionNumberTextBox.Enabled = true;
            approvedAtDateTimePicker.Enabled = true;
            instrumentTypeComboBox.Enabled = true;
        }

        private void ProcedureDetail_Load(object sender, EventArgs e)
        {

        }
    }
}