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
    public partial class ReferenceStandardDetail : Form
    {
        private readonly ReferenceStandardApiClient referenceStandardApiClient = new(AuthServiceProvider.Instance);

        private ReferenceStandardDTO referenceStandard = new();
        private FormMode mode;

        public ReferenceStandardDTO ReferenceStandard
        {
            get { return referenceStandard; }
            set
            {
                referenceStandard = value;
                SetReferenceStandard();
            }
        }

        public FormMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public ReferenceStandardDetail()
        {
            InitializeComponent();
        }

        public ReferenceStandardDetail(FormMode mode, ReferenceStandardDTO referenceStandard) : this()
        {
            this.Mode = mode;
            this.ReferenceStandard = referenceStandard;
        }

        private void SetReferenceStandard()
        {
            descriptionTextBox.Text = ReferenceStandard.Description;
            certifyingBodyTextBox.Text = ReferenceStandard.CertifyingBody;
            certificateNumberTextBox.Text = ReferenceStandard.CertificateNumber;

            if (ReferenceStandard.CertificateIssuedAt != default)
                certificateIssuedAtDateTimePicker.Value = ReferenceStandard.CertificateIssuedAt;
            if (ReferenceStandard.CertificateExpiresAt != default)
                certificateExpiresAtDateTimePicker.Value = ReferenceStandard.CertificateExpiresAt;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateReferenceStandard()) return;

            try
            {
                DisableControls();

                ReferenceStandard.Description = descriptionTextBox.Text.Trim();
                ReferenceStandard.CertifyingBody = certifyingBodyTextBox.Text.Trim();
                ReferenceStandard.CertificateNumber = certificateNumberTextBox.Text.Trim();
                ReferenceStandard.CertificateIssuedAt = certificateIssuedAtDateTimePicker.Value.Date;
                ReferenceStandard.CertificateExpiresAt = certificateExpiresAtDateTimePicker.Value.Date;

                if (this.Mode == FormMode.Update)
                {
                    await referenceStandardApiClient.UpdateAsync(ReferenceStandard);
                }
                else
                {
                    await referenceStandardApiClient.AddAsync(ReferenceStandard);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el patrón de referencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool ValidateReferenceStandard()
        {
            bool isValid = true;

            errorProvider.SetError(descriptionTextBox, string.Empty);
            errorProvider.SetError(certifyingBodyTextBox, string.Empty);
            errorProvider.SetError(certificateNumberTextBox, string.Empty);
            errorProvider.SetError(certificateExpiresAtDateTimePicker, string.Empty);

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(descriptionTextBox, "La descripción es obligatoria");
            }

            if (string.IsNullOrWhiteSpace(certifyingBodyTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(certifyingBodyTextBox, "El organismo certificante es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(certificateNumberTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(certificateNumberTextBox, "El N° de certificado es obligatorio");
            }

            if (certificateExpiresAtDateTimePicker.Value.Date <= certificateIssuedAtDateTimePicker.Value.Date)
            {
                isValid = false;
                errorProvider.SetError(certificateExpiresAtDateTimePicker, "La fecha de vencimiento debe ser posterior a la de emisión");
            }

            return isValid;
        }

        private void DisableControls()
        {
            descriptionTextBox.Enabled = false;
            certifyingBodyTextBox.Enabled = false;
            certificateNumberTextBox.Enabled = false;
            certificateIssuedAtDateTimePicker.Enabled = false;
            certificateExpiresAtDateTimePicker.Enabled = false;
        }

        private void EnableControls()
        {
            descriptionTextBox.Enabled = true;
            certifyingBodyTextBox.Enabled = true;
            certificateNumberTextBox.Enabled = true;
            certificateIssuedAtDateTimePicker.Enabled = true;
            certificateExpiresAtDateTimePicker.Enabled = true;
        }
    }
}
