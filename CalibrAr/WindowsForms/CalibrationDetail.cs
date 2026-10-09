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
    public partial class CalibrationDetail : Form
    {
        private readonly CalibrationApiClient calibrationApiClient = new(AuthServiceProvider.Instance);
        private readonly InstrumentApiClient instrumentApiClient = new(AuthServiceProvider.Instance);
        private readonly InstrumentTypeApiClient instrumentTypeApiClient = new(AuthServiceProvider.Instance);
        private readonly ProcedureApiClient procedureApiClient = new(AuthServiceProvider.Instance);
        private readonly UserApiClient userApiClient = new(AuthServiceProvider.Instance);
        private static readonly string[] rolesThatCanPerform = { "Administrador", "Responsable", "Operador" };
        private static readonly string[] rolesThatCanApprove = { "Administrador", "Responsable" };

        private CalibrationDTO calibration;
        private UserDTO? currentUser;
        private FormMode mode;
        private List<ProcedureDTO> allProcedures = new List<ProcedureDTO>();
        private List<UserDTO> allUsers = new List<UserDTO>();
        private List<InstrumentTypeDTO> instrumentTypes = new List<InstrumentTypeDTO>();
        private List<CalibrationMeasurementDTO> measurementsLocales = new List<CalibrationMeasurementDTO>();


        public CalibrationDTO Calibration
        {
            get { return calibration; }
            set
            {
                calibration = value;
                this.SetCalibration();
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
        public CalibrationDetail()
        {
            InitializeComponent();
            calibrationDateTimePicker.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);

        }

        public CalibrationDetail(FormMode mode, CalibrationDTO calibration) : this()
        {
            Init(mode, calibration);
        }

        private async void Init(FormMode mode, CalibrationDTO calibration)
        {
            try
            {
                DisableControls();
                this.Mode = mode;

                ConfigureColumns();
                measurementsLocales = new List<CalibrationMeasurementDTO>();
                LoadInterventionTypes();
                LoadResults();
                await LoadInstrumentTypes();
                await LoadInstruments();
                await LoadProcedures();
                await LoadUsers();

                
                this.Calibration = calibration;
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

        private void ConfigureColumns()
        {
            this.measurementsDataGridView.AutoGenerateColumns = false;

            this.measurementsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NominalValue",
                HeaderText = "Nominal value",
                DataPropertyName = "NominalValue",
                Width = 80,
                DefaultCellStyle = { Format = "N3" }
            });

            this.measurementsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MeasuredValue",
                HeaderText = "Measured value",
                DataPropertyName = "MeasuredValue",
                Width = 80,
                DefaultCellStyle = { Format = "N3" }
            });

            this.measurementsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Error",
                HeaderText = "Error",
                DataPropertyName = "Error",
                Width = 70,
                DefaultCellStyle = { Format = "N3" }
            });

            this.measurementsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsWithinTolerance",
                HeaderText = "Within tolerance",
                DataPropertyName = "IsWithinTolerance",
                Width = 80
            });

            this.measurementsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Notes",
                DataPropertyName = "Notes",
                Width = 100
            });
        }

        private void LoadInterventionTypes()
        {
            interventionTypeComboBox.DataSource = Enum.GetValues(typeof(InterventionTypeDTO));
            interventionTypeComboBox.SelectedIndex = -1;
        }

        private void LoadResults()
        {
            resultComboBox.DataSource = Enum.GetValues(typeof(ResultDTO));
            resultComboBox.SelectedIndex = -1;
        }

        private async Task LoadInstrumentTypes()
        {
            var types = await instrumentTypeApiClient.GetAllAsync();
            instrumentTypes = types.ToList();
        }

        private async Task LoadInstruments()
        {
            var instruments = await instrumentApiClient.GetAllAsync();
            instrumentComboBox.DataSource = instruments.ToList();
            instrumentComboBox.DisplayMember = "Code";
            instrumentComboBox.ValueMember = "Id";
            instrumentComboBox.SelectedIndex = -1;
        }

        private async Task LoadProcedures()
        {
            var procedures = await procedureApiClient.GetAllAsync();
            allProcedures = procedures.ToList();
            FilterProcedures();
        }

        private void FilterProcedures()
        {
            InstrumentDTO? selectedInstrument = instrumentComboBox.SelectedItem as InstrumentDTO;

            if (selectedInstrument == null)
            {
                procedureComboBox.DataSource = new List<ProcedureDTO>();
                procedureComboBox.DisplayMember = "Name";
                procedureComboBox.ValueMember = "Id";
                procedureComboBox.SelectedIndex = -1;
                procedureComboBox.Enabled = false;
                return;
            }

            var filteredProcedures = allProcedures
                .Where(p => p.InstrumentTypeId == selectedInstrument.InstrumentTypeId)
                .ToList();

            procedureComboBox.DataSource = filteredProcedures;
            procedureComboBox.DisplayMember = "Name";
            procedureComboBox.ValueMember = "Id";
            procedureComboBox.SelectedIndex = -1;
            procedureComboBox.Enabled = true;
        }

        private async Task LoadUsers()
        {
            allUsers = (await userApiClient.GetAllAsync()).ToList();

            if (this.Mode == FormMode.Create)
            {
                string? sessionEmail = AuthServiceProvider.Instance.GetEmail();
                currentUser = allUsers.FirstOrDefault(u => u.Email == sessionEmail);

                if (currentUser == null)
                {
                    MessageBox.Show("Current user not found. Please check your session.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void instrumentComboBox_SelectedIndex(object sender, EventArgs e)
        {
            FilterProcedures();
        }

        private void isExternalCheckBox_Checked(object sender, EventArgs e)
        {
            externalLabTextBox.Enabled = isExternalCheckBox.Checked;

            if (!isExternalCheckBox.Checked)
            {
                externalLabTextBox.Text = string.Empty;
            }
        }


        private decimal? GetMaxAllowedError()
        {
            InstrumentDTO? selectedInstrument = instrumentComboBox.SelectedItem as InstrumentDTO;

            if (selectedInstrument == null)
            {
                return null;
            }

            // El error del instrumento sobreescribe el del tipo si está definido
            return selectedInstrument.MaxAllowedError
                ?? instrumentTypes.FirstOrDefault(t => t.Id == selectedInstrument.InstrumentTypeId)?.MaxAllowedError;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateCalibration())
            {
                try
                {
                    DisableControls();

                    this.Calibration.InterventionType = interventionTypeComboBox.SelectedItem.ToString();
                    this.Calibration.IsExternal = isExternalCheckBox.Checked;
                    this.Calibration.ExternalLab = externalLabTextBox.Text;
                    this.Calibration.CertificateNumber = certificateNumberTextBox.Text;
                    this.Calibration.Result = resultComboBox.SelectedItem.ToString();
                    this.Calibration.RestrictionDetail = restrictionDetailTextBox.Text;
                    this.Calibration.Notes = notesTextBox.Text;
                    this.Calibration.CalibrationDate = this.Mode == FormMode.Create ? DateTime.Now : calibrationDateTimePicker.Value;
                    this.Calibration.InstrumentId = (int)instrumentComboBox.SelectedValue;
                    this.Calibration.Measurements = measurementsLocales.ToList();

                    this.Calibration.ProcedureId = procedureComboBox.SelectedValue == null
                        ? null
                        : (int?)(int)procedureComboBox.SelectedValue;

                    if (this.Mode == FormMode.Create)
                    {
                        this.Calibration.PerformedByUserId = currentUser?.Id;
                        this.Calibration.ApprovedByUserId = null;
                    }
                    else
                    {
                        this.Calibration.PerformedByUserId = performedByUserComboBox.SelectedValue == null
                            ? null
                            : (int?)(int)performedByUserComboBox.SelectedValue;

                        // Solo si el combo está visible (calibración aprobada). "(None)" = 0 => desaprobar
                        if (approvedByUserComboBox.Visible)
                        {
                            int approvedId = (int)approvedByUserComboBox.SelectedValue;
                            this.Calibration.ApprovedByUserId = approvedId == 0 ? null : approvedId;
                        }
                    }

                    if (this.Mode == FormMode.Update)
                    {
                        await calibrationApiClient.UpdateAsync(this.Calibration);
                    }
                    else
                    {
                        await calibrationApiClient.AddAsync(this.Calibration);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating calibration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void addMeasurementButton_Click(object sender, EventArgs e)
        {
            decimal? maxAllowedError = GetMaxAllowedError();

            if (maxAllowedError == null)
            {
                MessageBox.Show("Please select an instrument first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CalibrationMeasurementDTO newMeasurement = new CalibrationMeasurementDTO();
            CalibrationMeasurementDetail measurementDetail = new CalibrationMeasurementDetail(FormMode.Create, newMeasurement, maxAllowedError.Value);

            if (measurementDetail.ShowDialog(this) == DialogResult.OK)
            {
                measurementsLocales.Add(measurementDetail.Measurement);
                RefreshMeasurementsGrid();
            }
        }

        private void updateMeasurementButton_Click(object sender, EventArgs e)
        {
            CalibrationMeasurementDTO? selectedMeasurement = this.SelectedMeasurement();
            if (selectedMeasurement == null) return;

            decimal? maxAllowedError = GetMaxAllowedError();

            if (maxAllowedError == null)
            {
                MessageBox.Show("Please select an instrument first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clonar measurement para edición
            CalibrationMeasurementDTO measurementCopy = new CalibrationMeasurementDTO
            {
                Id = selectedMeasurement.Id,
                CalibrationId = selectedMeasurement.CalibrationId,
                NominalValue = selectedMeasurement.NominalValue,
                MeasuredValue = selectedMeasurement.MeasuredValue,
                Error = selectedMeasurement.Error,
                IsWithinTolerance = selectedMeasurement.IsWithinTolerance,
                Notes = selectedMeasurement.Notes
            };

            CalibrationMeasurementDetail measurementDetail = new CalibrationMeasurementDetail(FormMode.Update, measurementCopy, maxAllowedError.Value);

            if (measurementDetail.ShowDialog(this) == DialogResult.OK)
            {
                // Actualizar measurement en la lista
                int index = measurementsLocales.IndexOf(selectedMeasurement);
                if (index >= 0)
                {
                    measurementsLocales[index] = measurementDetail.Measurement;
                    RefreshMeasurementsGrid();
                }
            }
        }

        private void deleteMeasurementButton_Click(object sender, EventArgs e)
        {
            CalibrationMeasurementDTO? selectedMeasurement = this.SelectedMeasurement();
            if (selectedMeasurement == null) return;

            var result = MessageBox.Show($"¿Are you sure you want to delete the measurement {selectedMeasurement.NominalValue:N3}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                measurementsLocales.Remove(selectedMeasurement);
                RefreshMeasurementsGrid();
            }
        }

        private CalibrationMeasurementDTO? SelectedMeasurement()
        {
            if (measurementsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a measurement first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return (CalibrationMeasurementDTO)measurementsDataGridView.SelectedRows[0].DataBoundItem;
        }

        private void RefreshMeasurementsGrid()
        {
            measurementsDataGridView.DataSource = null;
            measurementsDataGridView.DataSource = measurementsLocales;

            // Habilitar/deshabilitar botones
            bool hasMeasurements = measurementsLocales.Count > 0;
            updateMeasurementButton.Enabled = hasMeasurements;
            deleteMeasurementButton.Enabled = hasMeasurements;

            if (hasMeasurements)
            {
                measurementsDataGridView.Rows[0].Selected = true;
            }

            // Actualizar totales en footer
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            int totalMeasurements = measurementsLocales.Count;
            int outOfTolerance = measurementsLocales.Count(m => !m.IsWithinTolerance);

            totalMeasurementsLabel.Text = $"Total measurements: {totalMeasurements}";
            outOfToleranceLabel.Text = $"Out of tolerance: {outOfTolerance}";
        }

        private void SetCalibration()
        {
            if (this.Calibration.CalibrationDate != default(DateTime))
            {
                this.calibrationDateTimePicker.Value = this.Calibration.CalibrationDate;
            }

            if (!string.IsNullOrEmpty(this.Calibration.InterventionType))
            {
                this.interventionTypeComboBox.SelectedItem =
                    Enum.Parse<InterventionTypeDTO>(this.Calibration.InterventionType);
            }

            this.isExternalCheckBox.Checked = this.Calibration.IsExternal;
            this.externalLabTextBox.Text = this.Calibration.ExternalLab;
            this.certificateNumberTextBox.Text = this.Calibration.CertificateNumber;

            if (!string.IsNullOrEmpty(this.Calibration.Result))
            {
                this.resultComboBox.SelectedItem =
                    Enum.Parse<ResultDTO>(this.Calibration.Result);
            }

            this.restrictionDetailTextBox.Text = this.Calibration.RestrictionDetail;
            this.notesTextBox.Text = this.Calibration.Notes;
            this.instrumentComboBox.SelectedValue = this.Calibration.InstrumentId;

            // Manejo null-safe para ProcedureId, PerformedByUserId y ApprovedByUserId
            if (this.Calibration.ProcedureId.HasValue)
            {
                this.procedureComboBox.SelectedValue = this.Calibration.ProcedureId.Value;
            }

            if (this.Mode == FormMode.Update)
            {
                int? performedById = this.Calibration.PerformedByUserId;
                int? approvedById = this.Calibration.ApprovedByUserId;

                // Performed by: usuarios que pueden crear (más el actual, por si cambió de rol)
                performedByUserComboBox.DataSource = allUsers
                    .Where(u => rolesThatCanPerform.Contains(u.Role.ToString()) || u.Id == performedById)
                    .ToList();
                performedByUserComboBox.DisplayMember = "FullName";
                performedByUserComboBox.ValueMember = "Id";
                performedByUserComboBox.SelectedValue = performedById ?? -1;

                // Approved by: usuarios que pueden aprobar, sin quien realizó la calibración,
                // más el aprobador actual, y "(None)" para desaprobar
                var approvers = allUsers
                    .Where(u => (rolesThatCanApprove.Contains(u.Role.ToString())
                                 && (u.Id != performedById || u.Role.ToString() == "Administrador"))
                                || u.Id == approvedById)
                    .ToList();
                approvers.Insert(0, new UserDTO { Id = 0, FullName = "(None)" });

                approvedByUserComboBox.DataSource = approvers;
                approvedByUserComboBox.DisplayMember = "FullName";
                approvedByUserComboBox.ValueMember = "Id";
                approvedByUserComboBox.SelectedValue = approvedById ?? 0;

                bool isApproved = approvedById.HasValue;
                approvedByUserLabel.Visible = isApproved;
                approvedByUserComboBox.Visible = isApproved;
            }

            // Clonar measurements para modelo local
            measurementsLocales = (this.Calibration.Measurements ?? new List<CalibrationMeasurementDTO>())
                .Select(measurement => new CalibrationMeasurementDTO
                {
                    Id = measurement.Id,
                    CalibrationId = measurement.CalibrationId,
                    NominalValue = measurement.NominalValue,
                    MeasuredValue = measurement.MeasuredValue,
                    Error = measurement.Error,
                    IsWithinTolerance = measurement.IsWithinTolerance,
                    Notes = measurement.Notes
                }).ToList();

            RefreshMeasurementsGrid();
        }

        private void SetFormMode(FormMode value)
        {
            mode = value;
            if (mode == FormMode.Update)
            {
                calibrationDateTimePicker.Visible = true;
                calibrationDateLabel.Visible = true;
                performedByUserComboBox.Visible = true;
                performedByUserLabel.Visible = true;
                approvedByUserComboBox.Visible = true;
                approvedByUserLabel.Visible = true;
            } else
            {
                calibrationDateTimePicker.Visible = false;
                calibrationDateLabel.Visible = false;
                performedByUserComboBox.Visible = false;
                performedByUserLabel.Visible = false;
                approvedByUserComboBox.Visible = false;
                approvedByUserLabel.Visible = false;
            }
        }

        private bool ValidateCalibration()
        {
            bool isValid = true;

            errorProvider.SetError(instrumentComboBox, string.Empty);
            errorProvider.SetError(calibrationDateTimePicker, string.Empty);
            errorProvider.SetError(interventionTypeComboBox, string.Empty);
            errorProvider.SetError(resultComboBox, string.Empty);
            errorProvider.SetError(measurementsDataGridView, string.Empty);

            if (this.instrumentComboBox.SelectedValue == null)
            {
                isValid = false;
                errorProvider.SetError(instrumentComboBox, "Instrument is required");
            }

            if (this.calibrationDateTimePicker.Checked == false)
            {
                isValid = false;
                errorProvider.SetError(calibrationDateTimePicker, "Calibration Date is required");
            }

            if (this.interventionTypeComboBox.SelectedItem == null)
            {
                isValid = false;
                errorProvider.SetError(interventionTypeComboBox, "Intervention Type is required");
            }

            if (this.resultComboBox.SelectedItem == null)
            {
                isValid = false;
                errorProvider.SetError(resultComboBox, "Result is required");
            }

            // Las calibraciones internas requieren al menos una medición
            if (!this.isExternalCheckBox.Checked && measurementsLocales.Count == 0)
            {
                isValid = false;
                errorProvider.SetError(measurementsDataGridView, "At least one measurement is required for internal calibrations");
            }

            return isValid;
        }

        private void DisableControls()
        {
            instrumentComboBox.Enabled = false;
            procedureComboBox.Enabled = false;
            calibrationDateTimePicker.Enabled = false;
            interventionTypeComboBox.Enabled = false;
            isExternalCheckBox.Enabled = false;
            externalLabTextBox.Enabled = false;
            certificateNumberTextBox.Enabled = false;
            resultComboBox.Enabled = false;
            restrictionDetailTextBox.Enabled = false;
            performedByUserComboBox.Enabled = false;
            approvedByUserComboBox.Enabled = false;
            notesTextBox.Enabled = false;
            addMeasurementButton.Enabled = false;
            updateMeasurementButton.Enabled = false;
            deleteMeasurementButton.Enabled = false;
        }

        private void EnableControls()
        {
            instrumentComboBox.Enabled = true;
            procedureComboBox.Enabled = instrumentComboBox.SelectedItem != null;
            calibrationDateTimePicker.Enabled = true;
            interventionTypeComboBox.Enabled = true;
            isExternalCheckBox.Enabled = true;
            externalLabTextBox.Enabled = isExternalCheckBox.Checked;
            certificateNumberTextBox.Enabled = true;
            resultComboBox.Enabled = true;
            restrictionDetailTextBox.Enabled = true;
            performedByUserComboBox.Enabled = true;
            approvedByUserComboBox.Enabled = true;
            notesTextBox.Enabled = true;
            addMeasurementButton.Enabled = true;
            updateMeasurementButton.Enabled = measurementsLocales.Count > 0;
            deleteMeasurementButton.Enabled = measurementsLocales.Count > 0;
        }

        private void CalibrationDetail_Load(object sender, EventArgs e)
        {

        }
    }
}