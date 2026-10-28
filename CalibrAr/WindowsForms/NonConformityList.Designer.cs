namespace WindowsForms
{
    partial class NonConformityList
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
            nonConformitiesDataGridView = new DataGridView();
            createButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)nonConformitiesDataGridView).BeginInit();
            SuspendLayout();
            // 
            // nonConformitiesDataGridView
            // 
            nonConformitiesDataGridView.AllowUserToAddRows = false;
            nonConformitiesDataGridView.AllowUserToDeleteRows = false;
            nonConformitiesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            nonConformitiesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            nonConformitiesDataGridView.Location = new Point(12, 12);
            nonConformitiesDataGridView.MultiSelect = false;
            nonConformitiesDataGridView.Name = "nonConformitiesDataGridView";
            nonConformitiesDataGridView.ReadOnly = true;
            nonConformitiesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            nonConformitiesDataGridView.Size = new Size(1076, 410);
            nonConformitiesDataGridView.TabIndex = 0;
            nonConformitiesDataGridView.CellContentClick += nonConformitiesDataGridView_CellContentClick;
            nonConformitiesDataGridView.CellPainting += nonConformitiesDataGridView_CellPainting;
            // 
            // createButton
            // 
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createButton.Location = new Point(1015, 435);
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
            updateButton.Location = new Point(934, 435);
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
            deleteButton.Location = new Point(853, 435);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 3;
            deleteButton.Text = "Eliminar";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // NonConformityList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 470);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(nonConformitiesDataGridView);
            Name = "NonConformityList";
            Text = "No conformidades";
            Load += NonConformityList_Load;
            ((System.ComponentModel.ISupportInitialize)nonConformitiesDataGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private DataGridView nonConformitiesDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
    }
}