namespace WindowsForms
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            emailLabel = new Label();
            emailTextBox = new TextBox();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            cancelButton = new Button();
            titleLabel = new Label();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(27, 42);
            emailLabel.Margin = new Padding(2, 0, 2, 0);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(39, 15);
            emailLabel.TabIndex = 1;
            emailLabel.Text = "Email:";
            // 
            // emailTextBox
            // 
            emailTextBox.Location = new Point(27, 59);
            emailTextBox.Margin = new Padding(2, 1, 2, 1);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(142, 23);
            emailTextBox.TabIndex = 2;
            emailTextBox.TextChanged += emailTextBox_TextChanged;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(27, 84);
            passwordLabel.Margin = new Padding(2, 0, 2, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(60, 15);
            passwordLabel.TabIndex = 3;
            passwordLabel.Text = "Password:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(27, 101);
            passwordTextBox.Margin = new Padding(2, 1, 2, 1);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(142, 23);
            passwordTextBox.TabIndex = 4;
            passwordTextBox.KeyPress += passwordTextBox_KeyPress;
            // 
            // loginButton
            // 
            loginButton.Location = new Point(27, 131);
            loginButton.Margin = new Padding(2, 1, 2, 1);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(65, 21);
            loginButton.TabIndex = 5;
            loginButton.Text = "Log In";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(102, 131);
            cancelButton.Margin = new Padding(2, 1, 2, 1);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(65, 21);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(27, 10);
            titleLabel.Margin = new Padding(2, 0, 2, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(69, 25);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Log In";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // LoginForm
            // 
            AcceptButton = loginButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(194, 169);
            Controls.Add(cancelButton);
            Controls.Add(loginButton);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(emailTextBox);
            Controls.Add(emailLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 1, 2, 1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CalibrAr - Login";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label emailLabel;
        private TextBox emailTextBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}
