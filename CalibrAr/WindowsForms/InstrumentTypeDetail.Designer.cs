namespace WindowsForms
{
    partial class InstrumentTypeDetail
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
            nameLabel = new Label();
            nameTextBox = new TextBox();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            measurementUnitLabel = new Label();
            measurementUnitTextBox = new TextBox();
            maxAllowedErrorLabel = new Label();
            maxAllowedErrorTextBox = new TextBox();
            calibrationFrequencyMonthsLabel = new Label();
            calibrationFrequencyMonthsTextBox = new TextBox();
            saveButton = new Button();
            cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(39, 26);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(39, 15);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(84, 22);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(100, 23);
            nameTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(39, 57);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(67, 15);
            descriptionLabel.TabIndex = 2;
            descriptionLabel.Text = "Description";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(112, 54);
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(100, 23);
            descriptionTextBox.TabIndex = 3;
            // 
            // measurementUnitLabel
            // 
            measurementUnitLabel.AutoSize = true;
            measurementUnitLabel.Location = new Point(39, 89);
            measurementUnitLabel.Name = "measurementUnitLabel";
            measurementUnitLabel.Size = new Size(105, 15);
            measurementUnitLabel.TabIndex = 4;
            measurementUnitLabel.Text = "Measurement Unit";
            // 
            // measurementUnitTextBox
            // 
            measurementUnitTextBox.Location = new Point(150, 86);
            measurementUnitTextBox.Name = "measurementUnitTextBox";
            measurementUnitTextBox.Size = new Size(100, 23);
            measurementUnitTextBox.TabIndex = 5;
            // 
            // maxAllowedErrorLabel
            // 
            maxAllowedErrorLabel.AutoSize = true;
            maxAllowedErrorLabel.Location = new Point(39, 121);
            maxAllowedErrorLabel.Name = "maxAllowedErrorLabel";
            maxAllowedErrorLabel.Size = new Size(104, 15);
            maxAllowedErrorLabel.TabIndex = 6;
            maxAllowedErrorLabel.Text = "Max Allowed Error";
            // 
            // maxAllowedErrorTextBox
            // 
            maxAllowedErrorTextBox.Location = new Point(149, 118);
            maxAllowedErrorTextBox.Name = "maxAllowedErrorTextBox";
            maxAllowedErrorTextBox.Size = new Size(100, 23);
            maxAllowedErrorTextBox.TabIndex = 7;
            // 
            // calibrationFrequencyMonthsLabel
            // 
            calibrationFrequencyMonthsLabel.AutoSize = true;
            calibrationFrequencyMonthsLabel.Location = new Point(39, 156);
            calibrationFrequencyMonthsLabel.Name = "calibrationFrequencyMonthsLabel";
            calibrationFrequencyMonthsLabel.Size = new Size(175, 15);
            calibrationFrequencyMonthsLabel.TabIndex = 8;
            calibrationFrequencyMonthsLabel.Text = "Calibration Frequency (Months)";
            // 
            // calibrationFrequencyMonthsTextBox
            // 
            calibrationFrequencyMonthsTextBox.Location = new Point(220, 153);
            calibrationFrequencyMonthsTextBox.Name = "calibrationFrequencyMonthsTextBox";
            calibrationFrequencyMonthsTextBox.Size = new Size(100, 23);
            calibrationFrequencyMonthsTextBox.TabIndex = 9;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(328, 179);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 10;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(409, 179);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 11;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // InstrumentTypeDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 209);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(calibrationFrequencyMonthsTextBox);
            Controls.Add(calibrationFrequencyMonthsLabel);
            Controls.Add(maxAllowedErrorTextBox);
            Controls.Add(maxAllowedErrorLabel);
            Controls.Add(measurementUnitTextBox);
            Controls.Add(measurementUnitLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
            Name = "InstrumentTypeDetail";
            Text = "InstrumentTypeDetail";
            Load += InstrumentTypeDetail_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nameLabel;
        private TextBox nameTextBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label measurementUnitLabel;
        private TextBox measurementUnitTextBox;
        private Label maxAllowedErrorLabel;
        private TextBox maxAllowedErrorTextBox;
        private Label calibrationFrequencyMonthsLabel;
        private TextBox calibrationFrequencyMonthsTextBox;
        private Button saveButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}