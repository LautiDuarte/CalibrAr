namespace WindowsForms
{
    partial class InstrumentDetail
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
            codeTextBox = new TextBox();
            codeLabel = new Label();
            nameLabel = new Label();
            nameTextBox = new TextBox();
            serialNumberTextBox = new TextBox();
            serialNumberLabel = new Label();
            brandLabel = new Label();
            brandTextBox = new TextBox();
            modelLabel = new Label();
            modelTextBox = new TextBox();
            statusLabel = new Label();
            statusComboBox = new ComboBox();
            maxAllowedErrorLabel = new Label();
            calibrationFrequencyMonthsLabel = new Label();
            maxAllowedErrorTextBox = new TextBox();
            calibrationFrequencyMonthsTextBox = new TextBox();
            instrumentTypeLabel = new Label();
            instrumentTypeComboBox = new ComboBox();
            areaLabel = new Label();
            areaComboBox = new ComboBox();
            cancelButton = new Button();
            saveButton = new Button();
            errorProvider = new ErrorProvider(components);
            InstrumentTypeWarningLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // codeTextBox
            // 
            codeTextBox.Location = new Point(85, 12);
            codeTextBox.Name = "codeTextBox";
            codeTextBox.Size = new Size(166, 23);
            codeTextBox.TabIndex = 0;
            // 
            // codeLabel
            // 
            codeLabel.AutoSize = true;
            codeLabel.Location = new Point(40, 15);
            codeLabel.Name = "codeLabel";
            codeLabel.Size = new Size(35, 15);
            codeLabel.TabIndex = 1;
            codeLabel.Text = "Code";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(40, 44);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(39, 15);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(85, 41);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(166, 23);
            nameTextBox.TabIndex = 3;
            // 
            // serialNumberTextBox
            // 
            serialNumberTextBox.Location = new Point(128, 73);
            serialNumberTextBox.Name = "serialNumberTextBox";
            serialNumberTextBox.Size = new Size(166, 23);
            serialNumberTextBox.TabIndex = 5;
            // 
            // serialNumberLabel
            // 
            serialNumberLabel.AutoSize = true;
            serialNumberLabel.Location = new Point(40, 76);
            serialNumberLabel.Name = "serialNumberLabel";
            serialNumberLabel.Size = new Size(82, 15);
            serialNumberLabel.TabIndex = 6;
            serialNumberLabel.Text = "Serial Number";
            // 
            // brandLabel
            // 
            brandLabel.AutoSize = true;
            brandLabel.Location = new Point(40, 102);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(38, 15);
            brandLabel.TabIndex = 8;
            brandLabel.Text = "Brand";
            // 
            // brandTextBox
            // 
            brandTextBox.Location = new Point(85, 102);
            brandTextBox.Name = "brandTextBox";
            brandTextBox.Size = new Size(100, 23);
            brandTextBox.TabIndex = 9;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(40, 132);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(41, 15);
            modelLabel.TabIndex = 10;
            modelLabel.Text = "Model";
            // 
            // modelTextBox
            // 
            modelTextBox.Location = new Point(85, 129);
            modelTextBox.Name = "modelTextBox";
            modelTextBox.Size = new Size(100, 23);
            modelTextBox.TabIndex = 11;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(40, 161);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 15);
            statusLabel.TabIndex = 12;
            statusLabel.Text = "Status";
            // 
            // statusComboBox
            // 
            statusComboBox.FormattingEnabled = true;
            statusComboBox.Location = new Point(85, 158);
            statusComboBox.Name = "statusComboBox";
            statusComboBox.Size = new Size(121, 23);
            statusComboBox.TabIndex = 13;
            // 
            // maxAllowedErrorLabel
            // 
            maxAllowedErrorLabel.AutoSize = true;
            maxAllowedErrorLabel.Location = new Point(40, 194);
            maxAllowedErrorLabel.Name = "maxAllowedErrorLabel";
            maxAllowedErrorLabel.Size = new Size(104, 15);
            maxAllowedErrorLabel.TabIndex = 14;
            maxAllowedErrorLabel.Text = "Max Allowed Error";
            // 
            // calibrationFrequencyMonthsLabel
            // 
            calibrationFrequencyMonthsLabel.AutoSize = true;
            calibrationFrequencyMonthsLabel.Location = new Point(40, 222);
            calibrationFrequencyMonthsLabel.Name = "calibrationFrequencyMonthsLabel";
            calibrationFrequencyMonthsLabel.Size = new Size(175, 15);
            calibrationFrequencyMonthsLabel.TabIndex = 15;
            calibrationFrequencyMonthsLabel.Text = "Calibration Frequency (Months)";
            // 
            // maxAllowedErrorTextBox
            // 
            maxAllowedErrorTextBox.Location = new Point(151, 191);
            maxAllowedErrorTextBox.Name = "maxAllowedErrorTextBox";
            maxAllowedErrorTextBox.Size = new Size(100, 23);
            maxAllowedErrorTextBox.TabIndex = 16;
            // 
            // calibrationFrequencyMonthsTextBox
            // 
            calibrationFrequencyMonthsTextBox.Location = new Point(222, 219);
            calibrationFrequencyMonthsTextBox.Name = "calibrationFrequencyMonthsTextBox";
            calibrationFrequencyMonthsTextBox.Size = new Size(100, 23);
            calibrationFrequencyMonthsTextBox.TabIndex = 17;
            // 
            // instrumentTypeLabel
            // 
            instrumentTypeLabel.AutoSize = true;
            instrumentTypeLabel.Location = new Point(40, 285);
            instrumentTypeLabel.Name = "instrumentTypeLabel";
            instrumentTypeLabel.Size = new Size(92, 15);
            instrumentTypeLabel.TabIndex = 21;
            instrumentTypeLabel.Text = "Instrument Type";
            // 
            // instrumentTypeComboBox
            // 
            instrumentTypeComboBox.FormattingEnabled = true;
            instrumentTypeComboBox.Location = new Point(138, 282);
            instrumentTypeComboBox.Name = "instrumentTypeComboBox";
            instrumentTypeComboBox.Size = new Size(121, 23);
            instrumentTypeComboBox.TabIndex = 22;
            // 
            // areaLabel
            // 
            areaLabel.AutoSize = true;
            areaLabel.Location = new Point(40, 249);
            areaLabel.Name = "areaLabel";
            areaLabel.Size = new Size(31, 15);
            areaLabel.TabIndex = 23;
            areaLabel.Text = "Area";
            // 
            // areaComboBox
            // 
            areaComboBox.FormattingEnabled = true;
            areaComboBox.Location = new Point(77, 246);
            areaComboBox.Name = "areaComboBox";
            areaComboBox.Size = new Size(182, 23);
            areaComboBox.TabIndex = 24;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(721, 333);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 25;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(640, 333);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 26;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // InstrumentTypeWarningLabel
            // 
            InstrumentTypeWarningLabel.AutoSize = true;
            InstrumentTypeWarningLabel.Font = new Font("Segoe UI", 6F, FontStyle.Italic);
            InstrumentTypeWarningLabel.ForeColor = Color.Red;
            InstrumentTypeWarningLabel.Location = new Point(40, 308);
            InstrumentTypeWarningLabel.Name = "InstrumentTypeWarningLabel";
            InstrumentTypeWarningLabel.Size = new Size(177, 11);
            InstrumentTypeWarningLabel.TabIndex = 27;
            InstrumentTypeWarningLabel.Text = "Instrument type cannot be changed after creation.";
            // 
            // InstrumentDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 359);
            Controls.Add(InstrumentTypeWarningLabel);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(areaComboBox);
            Controls.Add(areaLabel);
            Controls.Add(instrumentTypeComboBox);
            Controls.Add(instrumentTypeLabel);
            Controls.Add(calibrationFrequencyMonthsTextBox);
            Controls.Add(maxAllowedErrorTextBox);
            Controls.Add(calibrationFrequencyMonthsLabel);
            Controls.Add(maxAllowedErrorLabel);
            Controls.Add(statusComboBox);
            Controls.Add(statusLabel);
            Controls.Add(modelTextBox);
            Controls.Add(modelLabel);
            Controls.Add(brandTextBox);
            Controls.Add(brandLabel);
            Controls.Add(serialNumberLabel);
            Controls.Add(serialNumberTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
            Controls.Add(codeLabel);
            Controls.Add(codeTextBox);
            Name = "InstrumentDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "IntrumentDetail";
            Load += IntrumentDetail_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox codeTextBox;
        private Label codeLabel;
        private Label nameLabel;
        private TextBox nameTextBox;
        private TextBox serialNumberTextBox;
        private Label serialNumberLabel;
        private Label brandLabel;
        private TextBox brandTextBox;
        private Label modelLabel;
        private TextBox modelTextBox;
        private Label statusLabel;
        private ComboBox statusComboBox;
        private Label maxAllowedErrorLabel;
        private Label calibrationFrequencyMonthsLabel;
        private TextBox maxAllowedErrorTextBox;
        private TextBox calibrationFrequencyMonthsTextBox;
        private Label instrumentTypeLabel;
        private ComboBox instrumentTypeComboBox;
        private Label areaLabel;
        private ComboBox areaComboBox;
        private Button cancelButton;
        private Button saveButton;
        private ErrorProvider errorProvider;
        private Label InstrumentTypeWarningLabel;
    }
}