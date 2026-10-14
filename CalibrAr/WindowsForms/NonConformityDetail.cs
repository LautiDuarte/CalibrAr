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
    public partial class NonConformityDetail : Form
    {
        private readonly NonConformityApiClient nonConformityApiClient = new(AuthServiceProvider.Instance);
        private readonly InstrumentApiClient instrumentApiClient = new(AuthServiceProvider.Instance);
        private readonly CalibrationApiClient calibrationApiClient = new(AuthServiceProvider.Instance);
        private readonly UserApiClient userApiClient = new(AuthServiceProvider.Instance);

        private static readonly string[] rolesThatCanDetect = { "Administrador", "Responsable", "Operador" };
        private static readonly string[] rolesThatCanClose = { "Administrador", "Responsable" };

        private NonConformityDTO nonConformity = new();
        private FormMode mode;
        private List<CalibrationDTO> allCalibrations = new();
        private List<UserDTO> allUsers = new();
        private bool isClosed;

        public NonConformityDTO NonConformity
        {
            get { return nonConformity; }
            set
            {
                nonConformity = value;
                SetNonConformity();
            }
        }

        public FormMode Mode
        {
            get { return mode; }
            set { SetFormMode(value); }
        }

        public NonConformityDetail()
        {
            InitializeComponent();
        }

        public NonConformityDetail(FormMode mode, NonConformityDTO nonConformity) : this()
        {
            Init(mode, nonConformity);
        }

        private async void Init(FormMode mode, NonConformityDTO nonConformity)
        {
            try
            {
                DisableControls();
                this.Mode = mode;

                LoadOrigins();
                LoadStatuses();
                await LoadInstruments();
                await LoadCalibrations();
                await LoadUsers();

                this.NonConformity = nonConformity;
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

        private void LoadOrigins()
        {
            originComboBox.DataSource = Enum.GetValues(typeof(OriginDTO));
            originComboBox.SelectedIndex = -1;
        }

        private void LoadStatuses()
        {
            statusComboBox.DataSource = Enum.GetValues(typeof(NonConformityStatusDTO));
            statusComboBox.SelectedIndex = -1;
        }

        private async Task LoadInstruments()
        {
            var instruments = await instrumentApiClient.GetAllAsync();
            instrumentComboBox.DataSource = instruments.ToList();
            instrumentComboBox.DisplayMember = "Code";
            instrumentComboBox.ValueMember = "Id";
            instrumentComboBox.SelectedIndex = -1;
        }

        private async Task LoadCalibrations()
        {
            allCalibrations = await calibrationApiClient.GetAllAsync();
            FilterCalibrations();
        }

        // Solo se cargan los usuarios en Update: en Create el usuario sale de la sesión
        private async Task LoadUsers()
        {
            if (this.Mode == FormMode.Update)
            {
                allUsers = (await userApiClient.GetAllAsync()).ToList();
            }
        }

        // Sin instrumento no hay calibraciones; con instrumento, solo las de ese instrumento
        private void FilterCalibrations()
        {
            InstrumentDTO? selectedInstrument = instrumentComboBox.SelectedItem as InstrumentDTO;

            if (selectedInstrument == null)
            {
                calibrationComboBox.DataSource = new List<CalibrationDTO>();
                calibrationComboBox.DisplayMember = "Id";
                calibrationComboBox.ValueMember = "Id";
                calibrationComboBox.SelectedIndex = -1;
                calibrationComboBox.Enabled = false;
                return;
            }

            var options = allCalibrations
                .Where(c => c.InstrumentId == selectedInstrument.Id)
                .Select(c => new { c.Id, DisplayName = $"{c.CalibrationDate:dd/MM/yyyy HH:mm} - {c.InterventionType}" })
                .Prepend(new { Id = 0, DisplayName = "(None)" })
                .ToList();

            calibrationComboBox.DataSource = options;
            calibrationComboBox.DisplayMember = "DisplayName";
            calibrationComboBox.ValueMember = "Id";
            calibrationComboBox.SelectedIndex = 0;
            calibrationComboBox.Enabled = true;
        }

        private void instrumentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterCalibrations();
        }

        private void SetNonConformity()
        {
            codeTextBox.Text = NonConformity.Code;
            instrumentComboBox.SelectedValue = NonConformity.InstrumentId; // dispara FilterCalibrations
            calibrationComboBox.SelectedValue = NonConformity.CalibrationId ?? 0;
            descriptionTextBox.Text = NonConformity.Description;
            correctiveActionTextBox.Text = NonConformity.CorrectiveAction;

            if (!string.IsNullOrEmpty(NonConformity.Origin))
            {
                originComboBox.SelectedItem = Enum.Parse<OriginDTO>(NonConformity.Origin);
            }

            if (this.Mode != FormMode.Update) return;

            isClosed = NonConformity.ClosedByUserId.HasValue;
            openedAtDateTimePicker.Value = NonConformity.OpenedAt;

            // Abierta: el estado no puede ser Closed (se cierra con el botón Close o desde ClosedBy).
            // Cerrada: se muestra Closed deshabilitado; al reabrir con "(None)" el back lo pasa a Open.
            if (!isClosed)
            {
                statusComboBox.DataSource = Enum.GetValues<NonConformityStatusDTO>()
                    .Where(s => s != NonConformityStatusDTO.Closed).ToList();
            }
            statusComboBox.SelectedItem = Enum.Parse<NonConformityStatusDTO>(NonConformity.Status);

            detectedByUserComboBox.DataSource = allUsers
                .Where(u => rolesThatCanDetect.Contains(u.Role.ToString()) || u.Id == NonConformity.DetectedByUserId)
                .ToList();
            detectedByUserComboBox.DisplayMember = "FullName";
            detectedByUserComboBox.ValueMember = "Id";
            detectedByUserComboBox.SelectedValue = NonConformity.DetectedByUserId;

            closedAtLabel.Visible = isClosed;
            closedAtDateTimePicker.Visible = isClosed;
            closedByUserLabel.Visible = isClosed;
            closedByUserComboBox.Visible = isClosed;

            if (isClosed)
            {
                var closers = allUsers
                    .Where(u => rolesThatCanClose.Contains(u.Role.ToString()) || u.Id == NonConformity.ClosedByUserId)
                    .ToList();
                closers.Insert(0, new UserDTO { Id = 0, FullName = "(None)" });

                closedByUserComboBox.DataSource = closers;
                closedByUserComboBox.DisplayMember = "FullName";
                closedByUserComboBox.ValueMember = "Id";
                closedByUserComboBox.SelectedValue = NonConformity.ClosedByUserId ?? 0;
                closedAtDateTimePicker.Value = NonConformity.ClosedAt ?? DateTime.Now;
            }

            EnableControls();
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
            bool isUpdate = mode == FormMode.Update;

            // En Create la entidad nace abierta: estado, fechas y usuarios los asigna el sistema
            statusLabel.Visible = isUpdate;
            statusComboBox.Visible = isUpdate;
            openedAtLabel.Visible = isUpdate;
            openedAtDateTimePicker.Visible = isUpdate;
            detectedByUserLabel.Visible = isUpdate;
            detectedByUserComboBox.Visible = isUpdate;

            // ClosedAt / ClosedBy solo se muestran si está cerrada (se decide en SetNonConformity)
            closedAtLabel.Visible = false;
            closedAtDateTimePicker.Visible = false;
            closedByUserLabel.Visible = false;
            closedByUserComboBox.Visible = false;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateNonConformity()) return;

            int? sessionUserId = AuthServiceProvider.Instance.GetUserId();
            if (this.Mode == FormMode.Create && sessionUserId == null)
            {
                MessageBox.Show("Current user not found. Please check your session.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DisableControls();

                NonConformity.Code = codeTextBox.Text.Trim();
                NonConformity.InstrumentId = (int)instrumentComboBox.SelectedValue;
                NonConformity.CalibrationId = calibrationComboBox.SelectedValue is int calibrationId && calibrationId != 0
                    ? calibrationId
                    : null;
                NonConformity.Description = descriptionTextBox.Text;
                NonConformity.Origin = originComboBox.SelectedItem!.ToString()!;
                NonConformity.CorrectiveAction = correctiveActionTextBox.Text;

                if (this.Mode == FormMode.Create)
                {
                    NonConformity.DetectedByUserId = sessionUserId!.Value;
                    NonConformity.Status = NonConformityStatusDTO.Open.ToString();
                }
                else
                {
                    NonConformity.OpenedAt = openedAtDateTimePicker.Value;
                    NonConformity.DetectedByUserId = (int)detectedByUserComboBox.SelectedValue;

                    if (isClosed)
                    {
                        // "(None)" = 0 => reabrir. El back limpia ClosedAt y pasa el estado a Open
                        int closedById = (int)closedByUserComboBox.SelectedValue;
                        NonConformity.ClosedByUserId = closedById == 0 ? null : closedById;
                        NonConformity.ClosedAt = closedById == 0 ? null : closedAtDateTimePicker.Value;
                    }
                    else
                    {
                        NonConformity.Status = statusComboBox.SelectedItem!.ToString()!;
                    }
                }

                if (this.Mode == FormMode.Update)
                {
                    await nonConformityApiClient.UpdateAsync(NonConformity);
                }
                else
                {
                    await nonConformityApiClient.AddAsync(NonConformity);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving non conformity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool ValidateNonConformity()
        {
            bool isValid = true;

            errorProvider.SetError(codeTextBox, string.Empty);
            errorProvider.SetError(instrumentComboBox, string.Empty);
            errorProvider.SetError(descriptionTextBox, string.Empty);
            errorProvider.SetError(originComboBox, string.Empty);
            errorProvider.SetError(statusComboBox, string.Empty);
            errorProvider.SetError(detectedByUserComboBox, string.Empty);
            errorProvider.SetError(closedAtDateTimePicker, string.Empty);

            if (string.IsNullOrWhiteSpace(codeTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(codeTextBox, "Code is required");
            }

            if (instrumentComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(instrumentComboBox, "Instrument is required");
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                isValid = false;
                errorProvider.SetError(descriptionTextBox, "Description is required");
            }

            if (originComboBox.SelectedItem == null)
            {
                isValid = false;
                errorProvider.SetError(originComboBox, "Origin is required");
            }

            if (this.Mode == FormMode.Update)
            {
                if (!isClosed && statusComboBox.SelectedItem == null)
                {
                    isValid = false;
                    errorProvider.SetError(statusComboBox, "Status is required");
                }

                if (detectedByUserComboBox.SelectedValue == null)
                {
                    isValid = false;
                    errorProvider.SetError(detectedByUserComboBox, "Detected by is required");
                }

                if (isClosed && closedByUserComboBox.SelectedValue is int closedById && closedById != 0
                    && closedAtDateTimePicker.Value < openedAtDateTimePicker.Value)
                {
                    isValid = false;
                    errorProvider.SetError(closedAtDateTimePicker, "Closed at cannot be earlier than Opened at");
                }
            }

            return isValid;
        }

        private void DisableControls()
        {
            codeTextBox.Enabled = false;
            instrumentComboBox.Enabled = false;
            calibrationComboBox.Enabled = false;
            descriptionTextBox.Enabled = false;
            originComboBox.Enabled = false;
            correctiveActionTextBox.Enabled = false;
            statusComboBox.Enabled = false;
            openedAtDateTimePicker.Enabled = false;
            detectedByUserComboBox.Enabled = false;
            closedAtDateTimePicker.Enabled = false;
            closedByUserComboBox.Enabled = false;
        }

        private void EnableControls()
        {
            codeTextBox.Enabled = true;
            instrumentComboBox.Enabled = true;
            calibrationComboBox.Enabled = instrumentComboBox.SelectedItem != null;
            descriptionTextBox.Enabled = true;
            originComboBox.Enabled = true;
            correctiveActionTextBox.Enabled = true;
            statusComboBox.Enabled = !isClosed;
            openedAtDateTimePicker.Enabled = true;
            detectedByUserComboBox.Enabled = true;
            closedAtDateTimePicker.Enabled = true;
            closedByUserComboBox.Enabled = true;
        }
    }
}
