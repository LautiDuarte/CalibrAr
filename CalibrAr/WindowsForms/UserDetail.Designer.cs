namespace WindowsForms
{
    partial class UserDetail
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
            fullNameLabel = new Label(); fullNameTextBox = new TextBox();
            emailLabel = new Label(); emailTextBox = new TextBox();
            roleLabel = new Label(); roleComboBox = new ComboBox();
            passwordLabel = new Label(); passwordTextBox = new TextBox();
            isActiveCheckBox = new CheckBox();
            saveButton = new Button(); cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // fullName
            fullNameLabel.AutoSize = true; fullNameLabel.Location = new Point(40, 15); fullNameLabel.Name = "fullNameLabel"; fullNameLabel.TabIndex = 0; fullNameLabel.Text = "Full Name";
            fullNameTextBox.Location = new Point(200, 12); fullNameTextBox.MaxLength = 100; fullNameTextBox.Name = "fullNameTextBox"; fullNameTextBox.Size = new Size(250, 23); fullNameTextBox.TabIndex = 1;
            // email
            emailLabel.AutoSize = true; emailLabel.Location = new Point(40, 44); emailLabel.Name = "emailLabel"; emailLabel.TabIndex = 2; emailLabel.Text = "Email";
            emailTextBox.Location = new Point(200, 41); emailTextBox.MaxLength = 100; emailTextBox.Name = "emailTextBox"; emailTextBox.Size = new Size(250, 23); emailTextBox.TabIndex = 3;
            // role
            roleLabel.AutoSize = true; roleLabel.Location = new Point(40, 73); roleLabel.Name = "roleLabel"; roleLabel.TabIndex = 4; roleLabel.Text = "Role";
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList; roleComboBox.FormattingEnabled = true; roleComboBox.Location = new Point(200, 70); roleComboBox.Name = "roleComboBox"; roleComboBox.Size = new Size(250, 23); roleComboBox.TabIndex = 5;
            // password (solo Create)
            passwordLabel.AutoSize = true; passwordLabel.Location = new Point(40, 102); passwordLabel.Name = "passwordLabel"; passwordLabel.TabIndex = 6; passwordLabel.Text = "Password";
            passwordTextBox.Location = new Point(200, 99); passwordTextBox.MaxLength = 30; passwordTextBox.Name = "passwordTextBox"; passwordTextBox.PasswordChar = '*'; passwordTextBox.Size = new Size(250, 23); passwordTextBox.TabIndex = 7;
            // isActive (solo Update)
            isActiveCheckBox.AutoSize = true; isActiveCheckBox.Location = new Point(200, 101); isActiveCheckBox.Name = "isActiveCheckBox"; isActiveCheckBox.TabIndex = 8; isActiveCheckBox.Text = "Is Active"; isActiveCheckBox.UseVisualStyleBackColor = true;
            // buttons
            saveButton.Location = new Point(294, 142); saveButton.Name = "saveButton"; saveButton.Size = new Size(75, 23); saveButton.TabIndex = 9; saveButton.Text = "Save"; saveButton.UseVisualStyleBackColor = true; saveButton.Click += saveButton_Click;
            cancelButton.Location = new Point(375, 142); cancelButton.Name = "cancelButton"; cancelButton.Size = new Size(75, 23); cancelButton.TabIndex = 10; cancelButton.Text = "Cancel"; cancelButton.UseVisualStyleBackColor = true; cancelButton.Click += cancelButton_Click;
            // errorProvider
            errorProvider.ContainerControl = this;
            // UserDetail
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 180);
            Controls.AddRange(new Control[] {
                fullNameLabel, fullNameTextBox, emailLabel, emailTextBox, roleLabel, roleComboBox,
                passwordLabel, passwordTextBox, isActiveCheckBox, saveButton, cancelButton });
            Name = "UserDetail";
            StartPosition = FormStartPosition.CenterParent;
            Text = "User";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label fullNameLabel;
        private TextBox fullNameTextBox;
        private Label emailLabel;
        private TextBox emailTextBox;
        private Label roleLabel;
        private ComboBox roleComboBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private CheckBox isActiveCheckBox;
        private Button saveButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}