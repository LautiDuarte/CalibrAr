namespace WindowsForms
{
    partial class CalibrationMeasurementDetail
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
            nominalValueLabel = new Label();
            nominalValueTextBox = new TextBox();
            measuredValueLabel = new Label();
            measuredValueTextBox = new TextBox();
            errorLabel = new Label();
            errorTextBox = new TextBox();
            isWithinToleranceCheckBox = new CheckBox();
            notesLabel = new Label();
            notesTextBox = new TextBox();
            cancelButton = new Button();
            saveButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // nominalValueLabel
            // 
            nominalValueLabel.AutoSize = true;
            nominalValueLabel.Location = new Point(30, 15);
            nominalValueLabel.Name = "nominalValueLabel";
            nominalValueLabel.Size = new Size(84, 15);
            nominalValueLabel.TabIndex = 0;
            nominalValueLabel.Text = "Nominal Value";
            // 
            // nominalValueTextBox
            // 
            nominalValueTextBox.Location = new Point(150, 12);
            nominalValueTextBox.Name = "nominalValueTextBox";
            nominalValueTextBox.Size = new Size(120, 23);
            nominalValueTextBox.TabIndex = 1;
            nominalValueTextBox.TextChanged += valueTextBox_TextChanged;
            // 
            // measuredValueLabel
            // 
            measuredValueLabel.AutoSize = true;
            measuredValueLabel.Location = new Point(30, 44);
            measuredValueLabel.Name = "measuredValueLabel";
            measuredValueLabel.Size = new Size(90, 15);
            measuredValueLabel.TabIndex = 2;
            measuredValueLabel.Text = "Measured Value";
            // 
            // measuredValueTextBox
            // 
            measuredValueTextBox.Location = new Point(150, 41);
            measuredValueTextBox.Name = "measuredValueTextBox";
            measuredValueTextBox.Size = new Size(120, 23);
            measuredValueTextBox.TabIndex = 3;
            measuredValueTextBox.TextChanged += valueTextBox_TextChanged;
            // 
            // errorLabel
            // 
            errorLabel.AutoSize = true;
            errorLabel.Location = new Point(30, 73);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(32, 15);
            errorLabel.TabIndex = 4;
            errorLabel.Text = "Error";
            // 
            // errorTextBox
            // 
            errorTextBox.Location = new Point(150, 70);
            errorTextBox.Name = "errorTextBox";
            errorTextBox.ReadOnly = true;
            errorTextBox.Size = new Size(120, 23);
            errorTextBox.TabIndex = 5;
            errorTextBox.TabStop = false;
            // 
            // isWithinToleranceCheckBox
            // 
            isWithinToleranceCheckBox.AutoCheck = false;
            isWithinToleranceCheckBox.AutoSize = true;
            isWithinToleranceCheckBox.Location = new Point(150, 102);
            isWithinToleranceCheckBox.Name = "isWithinToleranceCheckBox";
            isWithinToleranceCheckBox.Size = new Size(115, 19);
            isWithinToleranceCheckBox.TabIndex = 6;
            isWithinToleranceCheckBox.TabStop = false;
            isWithinToleranceCheckBox.Text = "Within Tolerance";
            isWithinToleranceCheckBox.UseVisualStyleBackColor = true;
            // 
            // notesLabel
            // 
            notesLabel.AutoSize = true;
            notesLabel.Location = new Point(30, 133);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new Size(38, 15);
            notesLabel.TabIndex = 7;
            notesLabel.Text = "Notes";
            // 
            // notesTextBox
            // 
            notesTextBox.Location = new Point(150, 130);
            notesTextBox.Name = "notesTextBox";
            notesTextBox.Size = new Size(220, 23);
            notesTextBox.TabIndex = 8;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(315, 180);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(234, 180);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 10;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // CalibrationMeasurementDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 220);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(notesTextBox);
            Controls.Add(notesLabel);
            Controls.Add(isWithinToleranceCheckBox);
            Controls.Add(errorTextBox);
            Controls.Add(errorLabel);
            Controls.Add(measuredValueTextBox);
            Controls.Add(measuredValueLabel);
            Controls.Add(nominalValueTextBox);
            Controls.Add(nominalValueLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CalibrationMeasurementDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Calibration Measurement - Detail";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nominalValueLabel;
        private TextBox nominalValueTextBox;
        private Label measuredValueLabel;
        private TextBox measuredValueTextBox;
        private Label errorLabel;
        private TextBox errorTextBox;
        private CheckBox isWithinToleranceCheckBox;
        private Label notesLabel;
        private TextBox notesTextBox;
        private Button cancelButton;
        private Button saveButton;
        private ErrorProvider errorProvider;
    }
}