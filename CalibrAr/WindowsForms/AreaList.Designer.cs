namespace WindowsForms
{
    partial class AreaList
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
            areasDataGridView = new DataGridView();
            deleteButton = new Button();
            updateButton = new Button();
            createButton = new Button();
            ((System.ComponentModel.ISupportInitialize)areasDataGridView).BeginInit();
            SuspendLayout();
            // 
            // areasDataGridView
            // 
            areasDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            areasDataGridView.Location = new Point(12, 12);
            areasDataGridView.Name = "areasDataGridView";
            areasDataGridView.Size = new Size(776, 406);
            areasDataGridView.TabIndex = 0;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(551, 424);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 1;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(632, 424);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 2;
            updateButton.Text = "Update";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // createButton
            // 
            createButton.Location = new Point(713, 424);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 3;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // AreaList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(createButton);
            Controls.Add(updateButton);
            Controls.Add(deleteButton);
            Controls.Add(areasDataGridView);
            Name = "AreaList";
            Text = "AreaList";
            Load += AreaList_Load;
            ((System.ComponentModel.ISupportInitialize)areasDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView areasDataGridView;
        private Button deleteButton;
        private Button updateButton;
        private Button createButton;
    }
}