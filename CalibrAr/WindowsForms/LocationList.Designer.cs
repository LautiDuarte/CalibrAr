namespace WindowsForms
{
    partial class LocationList
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

            locationsDataGridView = new DataGridView();
            deleteButton = new Button();
            updateButton = new Button();
            createButton = new Button();
            ((System.ComponentModel.ISupportInitialize)locationsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // locationsDataGridView
            // 
            locationsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            locationsDataGridView.Location = new Point(12, 12);
            locationsDataGridView.Name = "locationsDataGridView";
            locationsDataGridView.Size = new Size(776, 404);
            locationsDataGridView.TabIndex = 0;
            locationsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(551, 422);
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
            updateButton.Location = new Point(632, 422);
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
            createButton.Location = new Point(713, 422);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 3;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createButton.Click += createButton_Click;
            // 
            // LocationList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(createButton);
            Controls.Add(updateButton);
            Controls.Add(deleteButton);
            Controls.Add(locationsDataGridView);
            Name = "LocationList";
            Text = "LocationList";
            Load += LocationList_Load;
            ((System.ComponentModel.ISupportInitialize)locationsDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView locationsDataGridView;
        private Button deleteButton;
        private Button updateButton;
        private Button createButton;
    }
}