namespace WindowsForms
{
    partial class ReferenceStandardList
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
            referenceStandardsDataGridView = new DataGridView();
            createButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)referenceStandardsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // referenceStandardsDataGridView
            // 
            referenceStandardsDataGridView.AllowUserToAddRows = false;
            referenceStandardsDataGridView.AllowUserToDeleteRows = false;
            referenceStandardsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            referenceStandardsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            referenceStandardsDataGridView.Location = new Point(12, 12);
            referenceStandardsDataGridView.MultiSelect = false;
            referenceStandardsDataGridView.Name = "referenceStandardsDataGridView";
            referenceStandardsDataGridView.ReadOnly = true;
            referenceStandardsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            referenceStandardsDataGridView.Size = new Size(916, 410);
            referenceStandardsDataGridView.TabIndex = 0;
            // 
            // createButton
            // 
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createButton.Location = new Point(853, 435);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 1;
            createButton.Text = "Crear";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += createButton_Click;
            // 
            // updateButton
            // 
            updateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            updateButton.Location = new Point(772, 435);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 2;
            updateButton.Text = "Modificar";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            deleteButton.Location = new Point(691, 435);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 3;
            deleteButton.Text = "Eliminar";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // ReferenceStandardList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(940, 470);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(referenceStandardsDataGridView);
            Name = "ReferenceStandardList";
            Text = "Patrones de referencia";
            Load += ReferenceStandardList_Load;
            ((System.ComponentModel.ISupportInitialize)referenceStandardsDataGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private DataGridView referenceStandardsDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
    }
}