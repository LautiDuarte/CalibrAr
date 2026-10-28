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
            filterTextLabel = new Label();
            searchTextBox = new TextBox();
            filterStatusLabel = new Label();
            statusFilterComboBox = new ComboBox();
            filterAreaLabel = new Label();
            areaFilterComboBox = new ComboBox();
            searchButton = new Button();
            clearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)instrumentsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // filterTextLabel
            // 
            filterTextLabel.AutoSize = true;
            filterTextLabel.Location = new Point(12, 19);
            filterTextLabel.Name = "filterTextLabel";
            filterTextLabel.Text = "Texto:";
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(60, 16);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(200, 23);
            searchTextBox.TabIndex = 0;
            // 
            // filterStatusLabel
            // 
            filterStatusLabel.AutoSize = true;
            filterStatusLabel.Location = new Point(275, 19);
            filterStatusLabel.Name = "filterStatusLabel";
            filterStatusLabel.Text = "Estado:";
            // 
            // statusFilterComboBox
            // 
            statusFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusFilterComboBox.FormattingEnabled = true;
            statusFilterComboBox.Location = new Point(328, 16);
            statusFilterComboBox.Name = "statusFilterComboBox";
            statusFilterComboBox.Size = new Size(140, 23);
            statusFilterComboBox.TabIndex = 1;
            // 
            // filterAreaLabel
            // 
            filterAreaLabel.AutoSize = true;
            filterAreaLabel.Location = new Point(483, 19);
            filterAreaLabel.Name = "filterAreaLabel";
            filterAreaLabel.Text = "Área:";
            // 
            // areaFilterComboBox
            // 
            areaFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            areaFilterComboBox.FormattingEnabled = true;
            areaFilterComboBox.Location = new Point(523, 16);
            areaFilterComboBox.Name = "areaFilterComboBox";
            areaFilterComboBox.Size = new Size(220, 23);
            areaFilterComboBox.TabIndex = 2;
            // 
            // searchButton
            // 
            searchButton.Location = new Point(758, 15);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(75, 25);
            searchButton.TabIndex = 3;
            searchButton.Text = "Buscar";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += SearchButton_Click;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(839, 15);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 25);
            clearButton.TabIndex = 4;
            clearButton.Text = "Limpiar";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += ClearButton_Click;
            // 
            // instrumentsDataGridView
            // 
            instrumentsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            instrumentsDataGridView.Location = new Point(12, 52);
            instrumentsDataGridView.Name = "instrumentsDataGridView";
            instrumentsDataGridView.Size = new Size(1442, 472);
            instrumentsDataGridView.TabIndex = 5;
            instrumentsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // createButton
            // 
            createButton.Location = new Point(1379, 530);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 6;
            createButton.Text = "Crear";
            createButton.UseVisualStyleBackColor = true;
            createButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createButton.Click += CreateButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(1298, 530);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(75, 23);
            updateButton.TabIndex = 7;
            updateButton.Text = "Modificar";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            updateButton.Click += UpdateButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(1217, 530);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(75, 23);
            deleteButton.TabIndex = 8;
            deleteButton.Text = "Eliminar";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            deleteButton.Click += DeleteButton_Click;
            // 
            // InstrumentList
            // 
            AcceptButton = searchButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1466, 558);
            Controls.Add(clearButton);
            Controls.Add(searchButton);
            Controls.Add(areaFilterComboBox);
            Controls.Add(filterAreaLabel);
            Controls.Add(statusFilterComboBox);
            Controls.Add(filterStatusLabel);
            Controls.Add(searchTextBox);
            Controls.Add(filterTextLabel);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(instrumentsDataGridView);
            Name = "InstrumentList";
            Text = "Instrumentos";
            Load += InstrumentList_Load;
            ((System.ComponentModel.ISupportInitialize)instrumentsDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView instrumentsDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
        private Label filterTextLabel;
        private TextBox searchTextBox;
        private Label filterStatusLabel;
        private ComboBox statusFilterComboBox;
        private Label filterAreaLabel;
        private ComboBox areaFilterComboBox;
        private Button searchButton;
        private Button clearButton;
    }
}