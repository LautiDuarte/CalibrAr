namespace WindowsForms
{
    partial class ProcedureDetail
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
            versionNumberLabel = new Label();
            versionNumberTextBox = new TextBox();
            approvedAtLabel = new Label();
            approvedAtDateTimePicker = new DateTimePicker();
            instrumentTypeLabel = new Label();
            instrumentTypeComboBox = new ComboBox();
            cancelButton = new Button();
            saveButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            StartPosition = FormStartPosition.CenterParent;
            SuspendLayout();
            // 
            // codeTextBox
            // 
            codeTextBox.Location = new Point(160, 12);
            codeTextBox.Name = "codeTextBox";
            codeTextBox.Size = new Size(200, 23);
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
            nameTextBox.Location = new Point(160, 41);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(200, 23);
            nameTextBox.TabIndex = 3;
            // 
            // versionNumberLabel
            // 
            versionNumberLabel.AutoSize = true;
            versionNumberLabel.Location = new Point(40, 73);
            versionNumberLabel.Name = "versionNumberLabel";
            versionNumberLabel.Size = new Size(92, 15);
            versionNumberLabel.TabIndex = 4;
            versionNumberLabel.Text = "Version Number";
            // 
            // versionNumberTextBox
            // 
            versionNumberTextBox.Location = new Point(160, 70);
            versionNumberTextBox.Name = "versionNumberTextBox";
            versionNumberTextBox.Size = new Size(200, 23);
            versionNumberTextBox.TabIndex = 5;
            // 
            // approvedAtLabel
            // 
            approvedAtLabel.AutoSize = true;
            approvedAtLabel.Location = new Point(40, 102);
            approvedAtLabel.Name = "approvedAtLabel";
            approvedAtLabel.Size = new Size(74, 15);
            approvedAtLabel.TabIndex = 6;
            approvedAtLabel.Text = "Approved At";
            // 
            // approvedAtDateTimePicker
            // 
            approvedAtDateTimePicker.Location = new Point(160, 99);
            approvedAtDateTimePicker.Name = "approvedAtDateTimePicker";
            approvedAtDateTimePicker.Size = new Size(200, 23);
            approvedAtDateTimePicker.TabIndex = 7;
            // 
            // instrumentTypeLabel
            // 
            instrumentTypeLabel.AutoSize = true;
            instrumentTypeLabel.Location = new Point(40, 131);
            instrumentTypeLabel.Name = "instrumentTypeLabel";
            instrumentTypeLabel.Size = new Size(92, 15);
            instrumentTypeLabel.TabIndex = 8;
            instrumentTypeLabel.Text = "Instrument Type";
            // 
            // instrumentTypeComboBox
            // 
            instrumentTypeComboBox.FormattingEnabled = true;
            instrumentTypeComboBox.Location = new Point(160, 128);
            instrumentTypeComboBox.Name = "instrumentTypeComboBox";
            instrumentTypeComboBox.Size = new Size(200, 23);
            instrumentTypeComboBox.TabIndex = 9;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(385, 185);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(304, 185);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 11;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // ProcedureDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 225);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(instrumentTypeComboBox);
            Controls.Add(instrumentTypeLabel);
            Controls.Add(approvedAtDateTimePicker);
            Controls.Add(approvedAtLabel);
            Controls.Add(versionNumberTextBox);
            Controls.Add(versionNumberLabel);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
            Controls.Add(codeLabel);
            Controls.Add(codeTextBox);
            Name = "ProcedureDetail";
            Text = "ProcedureDetail";
            Load += ProcedureDetail_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox codeTextBox;
        private Label codeLabel;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Label versionNumberLabel;
        private TextBox versionNumberTextBox;
        private Label approvedAtLabel;
        private DateTimePicker approvedAtDateTimePicker;
        private Label instrumentTypeLabel;
        private ComboBox instrumentTypeComboBox;
        private Button cancelButton;
        private Button saveButton;
        private ErrorProvider errorProvider;
    }
}