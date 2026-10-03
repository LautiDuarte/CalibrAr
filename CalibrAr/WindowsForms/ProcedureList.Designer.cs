namespace WindowsForms
{
    partial class ProcedureList
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
            errorProvider1 = new ErrorProvider(components);
            proceduresDataGridView = new DataGridView();
            deleteButton = new Button();
            updateButton = new Button();
            createButton = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)proceduresDataGridView).BeginInit();
            SuspendLayout();
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // proceduresDataGridView1
            // 
            proceduresDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            proceduresDataGridView.Location = new Point(12, 12);
            proceduresDataGridView.Name = "proceduresDataGridView1";
            proceduresDataGridView.Size = new Size(776, 402);
            proceduresDataGridView.TabIndex = 0;
            proceduresDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(551, 420);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 1;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            deleteButton.Click += deleteButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(632, 420);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 2;
            updateButton.Text = "Update";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            updateButton.Click += updateButton_Click;
            // 
            // createButton
            // 
            createButton.Location = new Point(713, 420);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 3;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createButton.Click += createButton_Click;
            // 
            // ProcedureList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(createButton);
            Controls.Add(updateButton);
            Controls.Add(deleteButton);
            Controls.Add(proceduresDataGridView);
            Name = "ProcedureList";
            Text = "ProcedureList";
            Load += ProcedureList_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ((System.ComponentModel.ISupportInitialize)proceduresDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ErrorProvider errorProvider1;
        private DataGridView proceduresDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
    }
}