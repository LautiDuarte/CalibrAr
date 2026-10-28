namespace WindowsForms
{
    partial class ReferenceStandardDetail
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
            descriptionLabel = new Label(); descriptionTextBox = new TextBox();
            certifyingBodyLabel = new Label(); certifyingBodyTextBox = new TextBox();
            certificateNumberLabel = new Label(); certificateNumberTextBox = new TextBox();
            certificateIssuedAtLabel = new Label(); certificateIssuedAtDateTimePicker = new DateTimePicker();
            certificateExpiresAtLabel = new Label(); certificateExpiresAtDateTimePicker = new DateTimePicker();
            saveButton = new Button(); cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // description
            descriptionLabel.AutoSize = true; descriptionLabel.Location = new Point(40, 15); descriptionLabel.Name = "descriptionLabel"; descriptionLabel.TabIndex = 0; descriptionLabel.Text = "Descripción";
            descriptionTextBox.Location = new Point(200, 12); descriptionTextBox.Name = "descriptionTextBox"; descriptionTextBox.Size = new Size(250, 23); descriptionTextBox.TabIndex = 1;
            // certifyingBody
            certifyingBodyLabel.AutoSize = true; certifyingBodyLabel.Location = new Point(40, 44); certifyingBodyLabel.Name = "certifyingBodyLabel"; certifyingBodyLabel.TabIndex = 2; certifyingBodyLabel.Text = "Organismo certificante";
            certifyingBodyTextBox.Location = new Point(200, 41); certifyingBodyTextBox.Name = "certifyingBodyTextBox"; certifyingBodyTextBox.Size = new Size(250, 23); certifyingBodyTextBox.TabIndex = 3;
            // certificateNumber
            certificateNumberLabel.AutoSize = true; certificateNumberLabel.Location = new Point(40, 73); certificateNumberLabel.Name = "certificateNumberLabel"; certificateNumberLabel.TabIndex = 4; certificateNumberLabel.Text = "N° de certificado";
            certificateNumberTextBox.Location = new Point(200, 70); certificateNumberTextBox.Name = "certificateNumberTextBox"; certificateNumberTextBox.Size = new Size(250, 23); certificateNumberTextBox.TabIndex = 5;
            // certificateIssuedAt
            certificateIssuedAtLabel.AutoSize = true; certificateIssuedAtLabel.Location = new Point(40, 102); certificateIssuedAtLabel.Name = "certificateIssuedAtLabel"; certificateIssuedAtLabel.TabIndex = 6; certificateIssuedAtLabel.Text = "Fecha de emisión";
            certificateIssuedAtDateTimePicker.Format = DateTimePickerFormat.Short; certificateIssuedAtDateTimePicker.Location = new Point(200, 99); certificateIssuedAtDateTimePicker.Name = "certificateIssuedAtDateTimePicker"; certificateIssuedAtDateTimePicker.Size = new Size(250, 23); certificateIssuedAtDateTimePicker.TabIndex = 7;
            // certificateExpiresAt
            certificateExpiresAtLabel.AutoSize = true; certificateExpiresAtLabel.Location = new Point(40, 131); certificateExpiresAtLabel.Name = "certificateExpiresAtLabel"; certificateExpiresAtLabel.TabIndex = 8; certificateExpiresAtLabel.Text = "Fecha de vencimiento";
            certificateExpiresAtDateTimePicker.Format = DateTimePickerFormat.Short; certificateExpiresAtDateTimePicker.Location = new Point(200, 128); certificateExpiresAtDateTimePicker.Name = "certificateExpiresAtDateTimePicker"; certificateExpiresAtDateTimePicker.Size = new Size(250, 23); certificateExpiresAtDateTimePicker.TabIndex = 9;
            // buttons
            saveButton.Location = new Point(294, 172); saveButton.Name = "saveButton"; saveButton.Size = new Size(75, 23); saveButton.TabIndex = 10; saveButton.Text = "Guardar"; saveButton.UseVisualStyleBackColor = true; saveButton.Click += saveButton_Click;
            cancelButton.Location = new Point(375, 172); cancelButton.Name = "cancelButton"; cancelButton.Size = new Size(75, 23); cancelButton.TabIndex = 11; cancelButton.Text = "Cancelar"; cancelButton.UseVisualStyleBackColor = true; cancelButton.Click += cancelButton_Click;
            // errorProvider
            errorProvider.ContainerControl = this;
            // ReferenceStandardDetail
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 210);
            Controls.AddRange(new Control[] {
                descriptionLabel, descriptionTextBox, certifyingBodyLabel, certifyingBodyTextBox,
                certificateNumberLabel, certificateNumberTextBox, certificateIssuedAtLabel, certificateIssuedAtDateTimePicker,
                certificateExpiresAtLabel, certificateExpiresAtDateTimePicker, saveButton, cancelButton });
            Name = "ReferenceStandardDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patrón de referencia";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label certifyingBodyLabel;
        private TextBox certifyingBodyTextBox;
        private Label certificateNumberLabel;
        private TextBox certificateNumberTextBox;
        private Label certificateIssuedAtLabel;
        private DateTimePicker certificateIssuedAtDateTimePicker;
        private Label certificateExpiresAtLabel;
        private DateTimePicker certificateExpiresAtDateTimePicker;
        private Button saveButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}