namespace WindowsForms
{
    partial class CalibrationList
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

            calibrationsDataGridView = new DataGridView();
            createButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            fromLabel = new Label();
            fromDateTimePicker = new DateTimePicker();
            toLabel = new Label();
            toDateTimePicker = new DateTimePicker();
            filterResultLabel = new Label();
            resultFilterComboBox = new ComboBox();
            filterInstrumentLabel = new Label();
            instrumentFilterComboBox = new ComboBox();
            searchButton = new Button();
            clearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)calibrationsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // fromLabel
            // 
            fromLabel.AutoSize = true;
            fromLabel.Location = new Point(12, 19);
            fromLabel.Name = "fromLabel";
            fromLabel.Text = "Desde:";
            // 
            // fromDateTimePicker
            // 
            fromDateTimePicker.Format = DateTimePickerFormat.Short;
            fromDateTimePicker.ShowCheckBox = true;
            fromDateTimePicker.Checked = false;
            fromDateTimePicker.Location = new Point(60, 16);
            fromDateTimePicker.Name = "fromDateTimePicker";
            fromDateTimePicker.Size = new Size(115, 23);
            fromDateTimePicker.TabIndex = 0;
            // 
            // toLabel
            // 
            toLabel.AutoSize = true;
            toLabel.Location = new Point(190, 19);
            toLabel.Name = "toLabel";
            toLabel.Text = "Hasta:";
            // 
            // toDateTimePicker
            // 
            toDateTimePicker.Format = DateTimePickerFormat.Short;
            toDateTimePicker.ShowCheckBox = true;
            toDateTimePicker.Checked = false;
            toDateTimePicker.Location = new Point(235, 16);
            toDateTimePicker.Name = "toDateTimePicker";
            toDateTimePicker.Size = new Size(115, 23);
            toDateTimePicker.TabIndex = 1;
            // 
            // filterResultLabel
            // 
            filterResultLabel.AutoSize = true;
            filterResultLabel.Location = new Point(365, 19);
            filterResultLabel.Name = "filterResultLabel";
            filterResultLabel.Text = "Resultado:";
            // 
            // resultFilterComboBox
            // 
            resultFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            resultFilterComboBox.FormattingEnabled = true;
            resultFilterComboBox.Location = new Point(432, 16);
            resultFilterComboBox.Name = "resultFilterComboBox";
            resultFilterComboBox.Size = new Size(150, 23);
            resultFilterComboBox.TabIndex = 2;
            // 
            // filterInstrumentLabel
            // 
            filterInstrumentLabel.AutoSize = true;
            filterInstrumentLabel.Location = new Point(597, 19);
            filterInstrumentLabel.Name = "filterInstrumentLabel";
            filterInstrumentLabel.Text = "Instrumento:";
            // 
            // instrumentFilterComboBox
            // 
            instrumentFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            instrumentFilterComboBox.FormattingEnabled = true;
            instrumentFilterComboBox.Location = new Point(678, 16);
            instrumentFilterComboBox.Name = "instrumentFilterComboBox";
            instrumentFilterComboBox.Size = new Size(180, 23);
            instrumentFilterComboBox.TabIndex = 3;
            // 
            // searchButton
            // 
            searchButton.Location = new Point(873, 15);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(75, 25);
            searchButton.TabIndex = 4;
            searchButton.Text = "Buscar";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += SearchButton_Click;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(954, 15);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 25);
            clearButton.TabIndex = 5;
            clearButton.Text = "Limpiar";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += ClearButton_Click;
            // 
            // calibrationsDataGridView
            // 
            calibrationsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            calibrationsDataGridView.Location = new Point(12, 52);
            calibrationsDataGridView.Name = "calibrationsDataGridView";
            calibrationsDataGridView.Size = new Size(1442, 472);
            calibrationsDataGridView.TabIndex = 6;
            calibrationsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            calibrationsDataGridView.CellPainting += CalibrationsDataGridView_CellPainting;
            calibrationsDataGridView.CellContentClick += CalibrationsDataGridView_CellContentClick;
            // 
            // createButton
            // 
            createButton.Location = new Point(1379, 530);
            createButton.Name = "createButton";
            createButton.Size = new Size(75, 23);
            createButton.TabIndex = 7;
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
            updateButton.TabIndex = 8;
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
            deleteButton.TabIndex = 9;
            deleteButton.Text = "Eliminar";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            deleteButton.Click += DeleteButton_Click;
            // 
            // CalibrationList
            // 
            AcceptButton = searchButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1466, 558);
            Controls.Add(clearButton);
            Controls.Add(searchButton);
            Controls.Add(instrumentFilterComboBox);
            Controls.Add(filterInstrumentLabel);
            Controls.Add(resultFilterComboBox);
            Controls.Add(filterResultLabel);
            Controls.Add(toDateTimePicker);
            Controls.Add(toLabel);
            Controls.Add(fromDateTimePicker);
            Controls.Add(fromLabel);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(createButton);
            Controls.Add(calibrationsDataGridView);
            Name = "CalibrationList";
            Text = "Calibraciones";
            Load += CalibrationList_Load;
            ((System.ComponentModel.ISupportInitialize)calibrationsDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView calibrationsDataGridView;
        private Button createButton;
        private Button updateButton;
        private Button deleteButton;
        private Label fromLabel;
        private DateTimePicker fromDateTimePicker;
        private Label toLabel;
        private DateTimePicker toDateTimePicker;
        private Label filterResultLabel;
        private ComboBox resultFilterComboBox;
        private Label filterInstrumentLabel;
        private ComboBox instrumentFilterComboBox;
        private Button searchButton;
        private Button clearButton;
    }
}