namespace WindowsForms
{
    partial class CalibrationDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            instrumentLabel = new Label();
            instrumentComboBox = new ComboBox();
            procedureLabel = new Label();
            procedureComboBox = new ComboBox();
            calibrationDateLabel = new Label();
            calibrationDateTimePicker = new DateTimePicker();
            interventionTypeLabel = new Label();
            interventionTypeComboBox = new ComboBox();
            isExternalCheckBox = new CheckBox();
            externalLabLabel = new Label();
            externalLabTextBox = new TextBox();
            certificateNumberLabel = new Label();
            certificateNumberTextBox = new TextBox();
            resultLabel = new Label();
            resultComboBox = new ComboBox();
            restrictionDetailLabel = new Label();
            restrictionDetailTextBox = new TextBox();
            performedByUserLabel = new Label();
            performedByUserComboBox = new ComboBox();
            approvedByUserLabel = new Label();
            approvedByUserComboBox = new ComboBox();
            notesLabel = new Label();
            notesTextBox = new TextBox();
            cancelButton = new Button();
            saveButton = new Button();
            measurementsLabel = new Label();
            measurementsDataGridView = new DataGridView();
            addMeasurementButton = new Button();
            updateMeasurementButton = new Button();
            deleteMeasurementButton = new Button();
            totalMeasurementsLabel = new Label();
            outOfToleranceLabel = new Label();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)measurementsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // instrumentLabel
            // 
            instrumentLabel.AutoSize = true;
            instrumentLabel.Location = new Point(40, 15);
            instrumentLabel.Name = "instrumentLabel";
            instrumentLabel.Size = new Size(65, 15);
            instrumentLabel.TabIndex = 0;
            instrumentLabel.Text = "Instrument";
            // 
            // instrumentComboBox
            // 
            instrumentComboBox.FormattingEnabled = true;
            instrumentComboBox.Location = new Point(200, 12);
            instrumentComboBox.Name = "instrumentComboBox";
            instrumentComboBox.Size = new Size(200, 23);
            instrumentComboBox.TabIndex = 1;
            instrumentComboBox.SelectedIndexChanged += instrumentComboBox_SelectedIndex;
            // 
            // procedureLabel
            // 
            procedureLabel.AutoSize = true;
            procedureLabel.Location = new Point(40, 44);
            procedureLabel.Name = "procedureLabel";
            procedureLabel.Size = new Size(61, 15);
            procedureLabel.TabIndex = 2;
            procedureLabel.Text = "Procedure";
            // 
            // procedureComboBox
            // 
            procedureComboBox.FormattingEnabled = true;
            procedureComboBox.Location = new Point(200, 41);
            procedureComboBox.Name = "procedureComboBox";
            procedureComboBox.Size = new Size(200, 23);
            procedureComboBox.TabIndex = 3;
            // 
            // calibrationDateLabel
            // 
            calibrationDateLabel.AutoSize = true;
            calibrationDateLabel.Location = new Point(40, 73);
            calibrationDateLabel.Name = "calibrationDateLabel";
            calibrationDateLabel.Size = new Size(92, 15);
            calibrationDateLabel.TabIndex = 4;
            calibrationDateLabel.Text = "Calibration Date";
            // 
            // calibrationDateTimePicker
            // 
            calibrationDateTimePicker.Location = new Point(200, 70);
            calibrationDateTimePicker.Name = "calibrationDateTimePicker";
            calibrationDateTimePicker.Size = new Size(200, 23);
            calibrationDateTimePicker.TabIndex = 5;
            // 
            // interventionTypeLabel
            // 
            interventionTypeLabel.AutoSize = true;
            interventionTypeLabel.Location = new Point(40, 102);
            interventionTypeLabel.Name = "interventionTypeLabel";
            interventionTypeLabel.Size = new Size(98, 15);
            interventionTypeLabel.TabIndex = 6;
            interventionTypeLabel.Text = "Intervention Type";
            // 
            // interventionTypeComboBox
            // 
            interventionTypeComboBox.FormattingEnabled = true;
            interventionTypeComboBox.Location = new Point(200, 99);
            interventionTypeComboBox.Name = "interventionTypeComboBox";
            interventionTypeComboBox.Size = new Size(200, 23);
            interventionTypeComboBox.TabIndex = 7;
            // 
            // isExternalCheckBox
            // 
            isExternalCheckBox.AutoSize = true;
            isExternalCheckBox.Location = new Point(200, 131);
            isExternalCheckBox.Name = "isExternalCheckBox";
            isExternalCheckBox.Size = new Size(79, 19);
            isExternalCheckBox.TabIndex = 8;
            isExternalCheckBox.Text = "Is External";
            isExternalCheckBox.UseVisualStyleBackColor = true;
            isExternalCheckBox.CheckedChanged += isExternalCheckBox_Checked;
            // 
            // externalLabLabel
            // 
            externalLabLabel.AutoSize = true;
            externalLabLabel.Location = new Point(40, 160);
            externalLabLabel.Name = "externalLabLabel";
            externalLabLabel.Size = new Size(71, 15);
            externalLabLabel.TabIndex = 9;
            externalLabLabel.Text = "External Lab";
            // 
            // externalLabTextBox
            // 
            externalLabTextBox.Location = new Point(200, 157);
            externalLabTextBox.Name = "externalLabTextBox";
            externalLabTextBox.Size = new Size(200, 23);
            externalLabTextBox.TabIndex = 10;
            // 
            // certificateNumberLabel
            // 
            certificateNumberLabel.AutoSize = true;
            certificateNumberLabel.Location = new Point(40, 189);
            certificateNumberLabel.Name = "certificateNumberLabel";
            certificateNumberLabel.Size = new Size(108, 15);
            certificateNumberLabel.TabIndex = 11;
            certificateNumberLabel.Text = "Certificate Number";
            // 
            // certificateNumberTextBox
            // 
            certificateNumberTextBox.Location = new Point(200, 186);
            certificateNumberTextBox.Name = "certificateNumberTextBox";
            certificateNumberTextBox.Size = new Size(200, 23);
            certificateNumberTextBox.TabIndex = 12;
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(40, 218);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(39, 15);
            resultLabel.TabIndex = 13;
            resultLabel.Text = "Result";
            // 
            // resultComboBox
            // 
            resultComboBox.FormattingEnabled = true;
            resultComboBox.Location = new Point(200, 215);
            resultComboBox.Name = "resultComboBox";
            resultComboBox.Size = new Size(200, 23);
            resultComboBox.TabIndex = 14;
            // 
            // restrictionDetailLabel
            // 
            restrictionDetailLabel.AutoSize = true;
            restrictionDetailLabel.Location = new Point(40, 247);
            restrictionDetailLabel.Name = "restrictionDetailLabel";
            restrictionDetailLabel.Size = new Size(96, 15);
            restrictionDetailLabel.TabIndex = 15;
            restrictionDetailLabel.Text = "Restriction Detail";
            // 
            // restrictionDetailTextBox
            // 
            restrictionDetailTextBox.Location = new Point(200, 244);
            restrictionDetailTextBox.Name = "restrictionDetailTextBox";
            restrictionDetailTextBox.Size = new Size(200, 23);
            restrictionDetailTextBox.TabIndex = 16;
            // 
            // performedByUserLabel
            // 
            performedByUserLabel.AutoSize = true;
            performedByUserLabel.Location = new Point(40, 279);
            performedByUserLabel.Name = "performedByUserLabel";
            performedByUserLabel.Size = new Size(79, 15);
            performedByUserLabel.TabIndex = 19;
            performedByUserLabel.Text = "Performed By";
            // 
            // performedByUserComboBox
            // 
            performedByUserComboBox.FormattingEnabled = true;
            performedByUserComboBox.Location = new Point(200, 276);
            performedByUserComboBox.Name = "performedByUserComboBox";
            performedByUserComboBox.Size = new Size(200, 23);
            performedByUserComboBox.TabIndex = 20;
            // 
            // approvedByUserLabel
            // 
            approvedByUserLabel.AutoSize = true;
            approvedByUserLabel.Location = new Point(40, 308);
            approvedByUserLabel.Name = "approvedByUserLabel";
            approvedByUserLabel.Size = new Size(75, 15);
            approvedByUserLabel.TabIndex = 21;
            approvedByUserLabel.Text = "Approved By";
            // 
            // approvedByUserComboBox
            // 
            approvedByUserComboBox.FormattingEnabled = true;
            approvedByUserComboBox.Location = new Point(200, 305);
            approvedByUserComboBox.Name = "approvedByUserComboBox";
            approvedByUserComboBox.Size = new Size(200, 23);
            approvedByUserComboBox.TabIndex = 22;
            // 
            // notesLabel
            // 
            notesLabel.AutoSize = true;
            notesLabel.Location = new Point(40, 337);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new Size(38, 15);
            notesLabel.TabIndex = 23;
            notesLabel.Text = "Notes";
            // 
            // notesTextBox
            // 
            notesTextBox.Location = new Point(200, 334);
            notesTextBox.Name = "notesTextBox";
            notesTextBox.Size = new Size(200, 23);
            notesTextBox.TabIndex = 24;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(405, 616);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 25;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(324, 616);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 26;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // measurementsLabel
            // 
            measurementsLabel.AutoSize = true;
            measurementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            measurementsLabel.Location = new Point(40, 366);
            measurementsLabel.Name = "measurementsLabel";
            measurementsLabel.Size = new Size(90, 15);
            measurementsLabel.TabIndex = 27;
            measurementsLabel.Text = "Measurements";
            // 
            // measurementsDataGridView
            // 
            measurementsDataGridView.AllowUserToAddRows = false;
            measurementsDataGridView.AllowUserToDeleteRows = false;
            measurementsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            measurementsDataGridView.Location = new Point(40, 391);
            measurementsDataGridView.MultiSelect = false;
            measurementsDataGridView.Name = "measurementsDataGridView";
            measurementsDataGridView.ReadOnly = true;
            measurementsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            measurementsDataGridView.Size = new Size(420, 160);
            measurementsDataGridView.TabIndex = 28;
            // 
            // addMeasurementButton
            // 
            addMeasurementButton.Location = new Point(205, 558);
            addMeasurementButton.Name = "addMeasurementButton";
            addMeasurementButton.Size = new Size(75, 23);
            addMeasurementButton.TabIndex = 29;
            addMeasurementButton.Text = "Add";
            addMeasurementButton.UseVisualStyleBackColor = true;
            addMeasurementButton.Click += addMeasurementButton_Click;
            // 
            // updateMeasurementButton
            // 
            updateMeasurementButton.Location = new Point(286, 558);
            updateMeasurementButton.Name = "updateMeasurementButton";
            updateMeasurementButton.Size = new Size(75, 23);
            updateMeasurementButton.TabIndex = 30;
            updateMeasurementButton.Text = "Update";
            updateMeasurementButton.UseVisualStyleBackColor = true;
            updateMeasurementButton.Click += updateMeasurementButton_Click;
            // 
            // deleteMeasurementButton
            // 
            deleteMeasurementButton.Location = new Point(367, 558);
            deleteMeasurementButton.Name = "deleteMeasurementButton";
            deleteMeasurementButton.Size = new Size(75, 23);
            deleteMeasurementButton.TabIndex = 31;
            deleteMeasurementButton.Text = "Delete";
            deleteMeasurementButton.UseVisualStyleBackColor = true;
            deleteMeasurementButton.Click += deleteMeasurementButton_Click;
            // 
            // totalMeasurementsLabel
            // 
            totalMeasurementsLabel.AutoSize = true;
            totalMeasurementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            totalMeasurementsLabel.Location = new Point(40, 558);
            totalMeasurementsLabel.Name = "totalMeasurementsLabel";
            totalMeasurementsLabel.Size = new Size(123, 15);
            totalMeasurementsLabel.TabIndex = 32;
            totalMeasurementsLabel.Text = "Total measurements:";
            // 
            // outOfToleranceLabel
            // 
            outOfToleranceLabel.AutoSize = true;
            outOfToleranceLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            outOfToleranceLabel.Location = new Point(40, 581);
            outOfToleranceLabel.Name = "outOfToleranceLabel";
            outOfToleranceLabel.Size = new Size(102, 15);
            outOfToleranceLabel.TabIndex = 33;
            outOfToleranceLabel.Text = "Out of tolerance:";
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // CalibrationDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 644);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(notesTextBox);
            Controls.Add(notesLabel);
            Controls.Add(approvedByUserComboBox);
            Controls.Add(approvedByUserLabel);
            Controls.Add(performedByUserComboBox);
            Controls.Add(performedByUserLabel);
            Controls.Add(restrictionDetailTextBox);
            Controls.Add(restrictionDetailLabel);
            Controls.Add(resultComboBox);
            Controls.Add(resultLabel);
            Controls.Add(certificateNumberTextBox);
            Controls.Add(certificateNumberLabel);
            Controls.Add(externalLabTextBox);
            Controls.Add(externalLabLabel);
            Controls.Add(isExternalCheckBox);
            Controls.Add(interventionTypeComboBox);
            Controls.Add(interventionTypeLabel);
            Controls.Add(calibrationDateTimePicker);
            Controls.Add(calibrationDateLabel);
            Controls.Add(procedureComboBox);
            Controls.Add(procedureLabel);
            Controls.Add(instrumentComboBox);
            Controls.Add(instrumentLabel);
            Controls.Add(outOfToleranceLabel);
            Controls.Add(totalMeasurementsLabel);
            Controls.Add(deleteMeasurementButton);
            Controls.Add(updateMeasurementButton);
            Controls.Add(addMeasurementButton);
            Controls.Add(measurementsDataGridView);
            Controls.Add(measurementsLabel);
            Name = "CalibrationDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CalibrationDetail";
            Load += CalibrationDetail_Load;
            ((System.ComponentModel.ISupportInitialize)measurementsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label instrumentLabel;
        private ComboBox instrumentComboBox;
        private Label procedureLabel;
        private ComboBox procedureComboBox;
        private Label calibrationDateLabel;
        private DateTimePicker calibrationDateTimePicker;
        private Label interventionTypeLabel;
        private ComboBox interventionTypeComboBox;
        private CheckBox isExternalCheckBox;
        private Label externalLabLabel;
        private TextBox externalLabTextBox;
        private Label certificateNumberLabel;
        private TextBox certificateNumberTextBox;
        private Label resultLabel;
        private ComboBox resultComboBox;
        private Label restrictionDetailLabel;
        private TextBox restrictionDetailTextBox;
        private Label performedByUserLabel;
        private ComboBox performedByUserComboBox;
        private Label approvedByUserLabel;
        private ComboBox approvedByUserComboBox;
        private Label notesLabel;
        private TextBox notesTextBox;
        private Button cancelButton;
        private Button saveButton;
        private ErrorProvider errorProvider;
        private Label measurementsLabel;
        private DataGridView measurementsDataGridView;
        private Button addMeasurementButton;
        private Button updateMeasurementButton;
        private Button deleteMeasurementButton;
        private Label totalMeasurementsLabel;
        private Label outOfToleranceLabel;
    }
}