namespace WindowsForms
{
    partial class NonConformityDetail
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            codeLabel = new Label(); codeTextBox = new TextBox();
            instrumentLabel = new Label(); instrumentComboBox = new ComboBox();
            calibrationLabel = new Label(); calibrationComboBox = new ComboBox();
            descriptionLabel = new Label(); descriptionTextBox = new TextBox();
            originLabel = new Label(); originComboBox = new ComboBox();
            correctiveActionLabel = new Label(); correctiveActionTextBox = new TextBox();
            statusLabel = new Label(); statusComboBox = new ComboBox();
            openedAtLabel = new Label(); openedAtDateTimePicker = new DateTimePicker();
            detectedByUserLabel = new Label(); detectedByUserComboBox = new ComboBox();
            closedAtLabel = new Label(); closedAtDateTimePicker = new DateTimePicker();
            closedByUserLabel = new Label(); closedByUserComboBox = new ComboBox();
            saveButton = new Button(); cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // codeLabel / codeTextBox
            codeLabel.AutoSize = true; codeLabel.Location = new Point(40, 15); codeLabel.Name = "codeLabel"; codeLabel.TabIndex = 0; codeLabel.Text = "Code";
            codeTextBox.Location = new Point(200, 12); codeTextBox.Name = "codeTextBox"; codeTextBox.Size = new Size(250, 23); codeTextBox.TabIndex = 1;
            // instrumentLabel / instrumentComboBox
            instrumentLabel.AutoSize = true; instrumentLabel.Location = new Point(40, 44); instrumentLabel.Name = "instrumentLabel"; instrumentLabel.TabIndex = 2; instrumentLabel.Text = "Instrument";
            instrumentComboBox.DropDownStyle = ComboBoxStyle.DropDownList; instrumentComboBox.FormattingEnabled = true; instrumentComboBox.Location = new Point(200, 41); instrumentComboBox.Name = "instrumentComboBox"; instrumentComboBox.Size = new Size(250, 23); instrumentComboBox.TabIndex = 3;
            instrumentComboBox.SelectedIndexChanged += instrumentComboBox_SelectedIndexChanged;
            // calibrationLabel / calibrationComboBox
            calibrationLabel.AutoSize = true; calibrationLabel.Location = new Point(40, 73); calibrationLabel.Name = "calibrationLabel"; calibrationLabel.TabIndex = 4; calibrationLabel.Text = "Calibration";
            calibrationComboBox.DropDownStyle = ComboBoxStyle.DropDownList; calibrationComboBox.FormattingEnabled = true; calibrationComboBox.Location = new Point(200, 70); calibrationComboBox.Name = "calibrationComboBox"; calibrationComboBox.Size = new Size(250, 23); calibrationComboBox.TabIndex = 5;
            // descriptionLabel / descriptionTextBox
            descriptionLabel.AutoSize = true; descriptionLabel.Location = new Point(40, 102); descriptionLabel.Name = "descriptionLabel"; descriptionLabel.TabIndex = 6; descriptionLabel.Text = "Description";
            descriptionTextBox.Location = new Point(200, 99); descriptionTextBox.Multiline = true; descriptionTextBox.ScrollBars = ScrollBars.Vertical; descriptionTextBox.Name = "descriptionTextBox"; descriptionTextBox.Size = new Size(250, 60); descriptionTextBox.TabIndex = 7;
            // originLabel / originComboBox
            originLabel.AutoSize = true; originLabel.Location = new Point(40, 171); originLabel.Name = "originLabel"; originLabel.TabIndex = 8; originLabel.Text = "Origin";
            originComboBox.DropDownStyle = ComboBoxStyle.DropDownList; originComboBox.FormattingEnabled = true; originComboBox.Location = new Point(200, 168); originComboBox.Name = "originComboBox"; originComboBox.Size = new Size(250, 23); originComboBox.TabIndex = 9;
            // correctiveActionLabel / correctiveActionTextBox
            correctiveActionLabel.AutoSize = true; correctiveActionLabel.Location = new Point(40, 200); correctiveActionLabel.Name = "correctiveActionLabel"; correctiveActionLabel.TabIndex = 10; correctiveActionLabel.Text = "Corrective Action";
            correctiveActionTextBox.Location = new Point(200, 197); correctiveActionTextBox.Multiline = true; correctiveActionTextBox.ScrollBars = ScrollBars.Vertical; correctiveActionTextBox.Name = "correctiveActionTextBox"; correctiveActionTextBox.Size = new Size(250, 60); correctiveActionTextBox.TabIndex = 11;
            // statusLabel / statusComboBox
            statusLabel.AutoSize = true; statusLabel.Location = new Point(40, 269); statusLabel.Name = "statusLabel"; statusLabel.TabIndex = 12; statusLabel.Text = "Status";
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList; statusComboBox.FormattingEnabled = true; statusComboBox.Location = new Point(200, 266); statusComboBox.Name = "statusComboBox"; statusComboBox.Size = new Size(250, 23); statusComboBox.TabIndex = 13;
            // openedAtLabel / openedAtDateTimePicker
            openedAtLabel.AutoSize = true; openedAtLabel.Location = new Point(40, 298); openedAtLabel.Name = "openedAtLabel"; openedAtLabel.TabIndex = 14; openedAtLabel.Text = "Opened At";
            openedAtDateTimePicker.Format = DateTimePickerFormat.Custom; openedAtDateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm"; openedAtDateTimePicker.Location = new Point(200, 295); openedAtDateTimePicker.Name = "openedAtDateTimePicker"; openedAtDateTimePicker.Size = new Size(250, 23); openedAtDateTimePicker.TabIndex = 15;
            // detectedByUserLabel / detectedByUserComboBox
            detectedByUserLabel.AutoSize = true; detectedByUserLabel.Location = new Point(40, 327); detectedByUserLabel.Name = "detectedByUserLabel"; detectedByUserLabel.TabIndex = 16; detectedByUserLabel.Text = "Detected By";
            detectedByUserComboBox.DropDownStyle = ComboBoxStyle.DropDownList; detectedByUserComboBox.FormattingEnabled = true; detectedByUserComboBox.Location = new Point(200, 324); detectedByUserComboBox.Name = "detectedByUserComboBox"; detectedByUserComboBox.Size = new Size(250, 23); detectedByUserComboBox.TabIndex = 17;
            // closedAtLabel / closedAtDateTimePicker
            closedAtLabel.AutoSize = true; closedAtLabel.Location = new Point(40, 356); closedAtLabel.Name = "closedAtLabel"; closedAtLabel.TabIndex = 18; closedAtLabel.Text = "Closed At";
            closedAtDateTimePicker.Format = DateTimePickerFormat.Custom; closedAtDateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm"; closedAtDateTimePicker.Location = new Point(200, 353); closedAtDateTimePicker.Name = "closedAtDateTimePicker"; closedAtDateTimePicker.Size = new Size(250, 23); closedAtDateTimePicker.TabIndex = 19;
            // closedByUserLabel / closedByUserComboBox
            closedByUserLabel.AutoSize = true; closedByUserLabel.Location = new Point(40, 385); closedByUserLabel.Name = "closedByUserLabel"; closedByUserLabel.TabIndex = 20; closedByUserLabel.Text = "Closed By";
            closedByUserComboBox.DropDownStyle = ComboBoxStyle.DropDownList; closedByUserComboBox.FormattingEnabled = true; closedByUserComboBox.Location = new Point(200, 382); closedByUserComboBox.Name = "closedByUserComboBox"; closedByUserComboBox.Size = new Size(250, 23); closedByUserComboBox.TabIndex = 21;
            // saveButton / cancelButton
            saveButton.Location = new Point(294, 420); saveButton.Name = "saveButton"; saveButton.Size = new Size(75, 23); saveButton.TabIndex = 22; saveButton.Text = "Save"; saveButton.UseVisualStyleBackColor = true; saveButton.Click += saveButton_Click;
            cancelButton.Location = new Point(375, 420); cancelButton.Name = "cancelButton"; cancelButton.Size = new Size(75, 23); cancelButton.TabIndex = 23; cancelButton.Text = "Cancel"; cancelButton.UseVisualStyleBackColor = true; cancelButton.Click += cancelButton_Click;
            // errorProvider
            errorProvider.ContainerControl = this;
            // NonConformityDetail
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 455);
            Controls.AddRange(new Control[] {
                codeLabel, codeTextBox, instrumentLabel, instrumentComboBox, calibrationLabel, calibrationComboBox,
                descriptionLabel, descriptionTextBox, originLabel, originComboBox, correctiveActionLabel, correctiveActionTextBox,
                statusLabel, statusComboBox, openedAtLabel, openedAtDateTimePicker, detectedByUserLabel, detectedByUserComboBox,
                closedAtLabel, closedAtDateTimePicker, closedByUserLabel, closedByUserComboBox, saveButton, cancelButton });
            Name = "NonConformityDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Non conformity";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label codeLabel;
        private TextBox codeTextBox;
        private Label instrumentLabel;
        private ComboBox instrumentComboBox;
        private Label calibrationLabel;
        private ComboBox calibrationComboBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label originLabel;
        private ComboBox originComboBox;
        private Label correctiveActionLabel;
        private TextBox correctiveActionTextBox;
        private Label statusLabel;
        private ComboBox statusComboBox;
        private Label openedAtLabel;
        private DateTimePicker openedAtDateTimePicker;
        private Label detectedByUserLabel;
        private ComboBox detectedByUserComboBox;
        private Label closedAtLabel;
        private DateTimePicker closedAtDateTimePicker;
        private Label closedByUserLabel;
        private ComboBox closedByUserComboBox;
        private Button saveButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}