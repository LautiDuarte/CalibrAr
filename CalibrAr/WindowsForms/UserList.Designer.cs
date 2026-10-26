namespace WindowsForms
{
    partial class UserList
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
            usersDataGridView = new DataGridView();
            createButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)usersDataGridView).BeginInit();
            SuspendLayout();
            // 
            // usersDataGridView
            // 
            usersDataGridView.AllowUserToAddRows = false;
            usersDataGridView.AllowUserToDeleteRows = false;
            usersDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            usersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usersDataGridView.Location = new Point(12, 12);
            usersDataGridView.MultiSelect = false;
            usersDataGridView.Name = "usersDataGridView";
            usersDataGridView.ReadOnly = true;
            usersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersDataGridView.Size = new Size(1036, 410);
            usersDataGridView.TabIndex = 0;
            // 
            // createButton
            // 
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            createButton.Location = new Point(974, 435);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 1;
            createButton.Text = "Crear";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // updateButton
            // 
            updateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            updateButton.Location = new Point(893, 435);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 2;
            updateButton.Text = "Modificar";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            deleteButton.Location = new Point(812, 435);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 3;
            deleteButton.Text = "Eliminar";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // UserList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1060, 470);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(usersDataGridView);
            Name = "UserList";
            Text = "Usuarios";
            Load += UserList_Load;
            ((System.ComponentModel.ISupportInitialize)usersDataGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private DataGridView usersDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
    }
}