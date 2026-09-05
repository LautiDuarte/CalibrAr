namespace WindowsForms
{
    partial class InstrumentList
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
            instrumentsDataGridView = new DataGridView();
            createButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)instrumentsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // instrumentsDataGridView
            // 
            instrumentsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            instrumentsDataGridView.Location = new Point(12, 52);
            instrumentsDataGridView.Name = "instrumentsDataGridView";
            instrumentsDataGridView.Size = new Size(1442, 472);
            instrumentsDataGridView.TabIndex = 0;
            // 
            // createButton
            // 
            createButton.Location = new Point(1379, 530);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 1;
            createButton.Text = "Create";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(1298, 530);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 2;
            updateButton.Text = "Update";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(1217, 530);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 3;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // InstrumentList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1466, 558);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(instrumentsDataGridView);
            Name = "InstrumentList";
            Text = "InstrumentList";
            Load += InstrumentList_Load;
            ((System.ComponentModel.ISupportInitialize)instrumentsDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView instrumentsDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
    }
}