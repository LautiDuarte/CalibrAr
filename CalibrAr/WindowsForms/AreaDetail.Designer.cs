namespace WindowsForms
{
    partial class AreaDetail
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
            responsibleLabel = new Label();
            responsibleTextBox = new TextBox();
            locationLabel = new Label();
            locationComboBox = new ComboBox();
            saveButton = new Button();
            cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(27, 18);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(39, 15);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(72, 15);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(100, 23);
            nameTextBox.TabIndex = 1;
            // 
            // responsibleLabel
            // 
            responsibleLabel.AutoSize = true;
            responsibleLabel.Location = new Point(27, 47);
            responsibleLabel.Name = "responsibleLabel";
            responsibleLabel.Size = new Size(70, 15);
            responsibleLabel.TabIndex = 2;
            responsibleLabel.Text = "Responsible";
            // 
            // responsibleTextBox
            // 
            responsibleTextBox.Location = new Point(103, 44);
            responsibleTextBox.Name = "responsibleTextBox";
            responsibleTextBox.Size = new Size(100, 23);
            responsibleTextBox.TabIndex = 3;
            // 
            // locationLabel
            // 
            locationLabel.AutoSize = true;
            locationLabel.Location = new Point(27, 75);
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new Size(53, 15);
            locationLabel.TabIndex = 4;
            locationLabel.Text = "Location";
            // 
            // locationComboBox
            // 
            locationComboBox.FormattingEnabled = true;
            locationComboBox.Location = new Point(82, 73);
            locationComboBox.Name = "locationComboBox";
            locationComboBox.Size = new Size(121, 23);
            locationComboBox.TabIndex = 5;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(183, 143);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(264, 143);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // AreaDetail
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 173);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(locationComboBox);
            Controls.Add(locationLabel);
            Controls.Add(responsibleTextBox);
            Controls.Add(responsibleLabel);
            Controls.Add(nameTextBox);
            Controls.Add(nameLabel);
            Name = "AreaDetail";
            Text = "AreaDetail";
            Load += AreaDetail_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nameLabel;
        private TextBox nameTextBox;
        private Label responsibleLabel;
        private TextBox responsibleTextBox;
        private Label locationLabel;
        private ComboBox locationComboBox;
        private Button saveButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}