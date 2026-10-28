namespace WindowsForms
{
    partial class ReportsForm
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
            reportsTabControl = new TabControl();
            upcomingTabPage = new TabPage();
            daysLabel = new Label();
            daysComboBox = new ComboBox();
            viewUpcomingButton = new Button();
            downloadUpcomingButton = new Button();
            upcomingSummaryLabel = new Label();
            upcomingDataGridView = new DataGridView();
            resultsTabPage = new TabPage();
            fromLabel = new Label();
            fromDateTimePicker = new DateTimePicker();
            toLabel = new Label();
            toDateTimePicker = new DateTimePicker();
            viewResultsButton = new Button();
            downloadResultsButton = new Button();
            resultsSummaryLabel = new Label();
            chartPanel = new Panel();
            resultsDataGridView = new DataGridView();
            reportsTabControl.SuspendLayout();
            upcomingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)upcomingDataGridView).BeginInit();
            resultsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resultsDataGridView).BeginInit();
            SuspendLayout();
            // reportsTabControl
            reportsTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportsTabControl.Controls.Add(upcomingTabPage);
            reportsTabControl.Controls.Add(resultsTabPage);
            reportsTabControl.Location = new Point(12, 12);
            reportsTabControl.Name = "reportsTabControl";
            reportsTabControl.SelectedIndex = 0;
            reportsTabControl.Size = new Size(876, 536);
            reportsTabControl.TabIndex = 0;
            // upcomingTabPage
            upcomingTabPage.Controls.Add(daysLabel);
            upcomingTabPage.Controls.Add(daysComboBox);
            upcomingTabPage.Controls.Add(viewUpcomingButton);
            upcomingTabPage.Controls.Add(downloadUpcomingButton);
            upcomingTabPage.Controls.Add(upcomingSummaryLabel);
            upcomingTabPage.Controls.Add(upcomingDataGridView);
            upcomingTabPage.Location = new Point(4, 24);
            upcomingTabPage.Name = "upcomingTabPage";
            upcomingTabPage.Padding = new Padding(3);
            upcomingTabPage.Size = new Size(868, 508);
            upcomingTabPage.TabIndex = 0;
            upcomingTabPage.Text = "Calibraciones próximas y vencidas";
            upcomingTabPage.UseVisualStyleBackColor = true;
            // daysLabel
            daysLabel.AutoSize = true;
            daysLabel.Location = new Point(12, 18);
            daysLabel.Name = "daysLabel";
            daysLabel.Size = new Size(48, 15);
            daysLabel.TabIndex = 0;
            daysLabel.Text = "Período:";
            // daysComboBox
            daysComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            daysComboBox.FormattingEnabled = true;
            daysComboBox.Items.AddRange(new object[] { "30 días", "60 días", "90 días" });
            daysComboBox.Location = new Point(75, 14);
            daysComboBox.Name = "daysComboBox";
            daysComboBox.Size = new Size(120, 23);
            daysComboBox.TabIndex = 1;
            // viewUpcomingButton
            viewUpcomingButton.Location = new Point(210, 12);
            viewUpcomingButton.Name = "viewUpcomingButton";
            viewUpcomingButton.Size = new Size(80, 27);
            viewUpcomingButton.TabIndex = 2;
            viewUpcomingButton.Text = "Ver";
            viewUpcomingButton.UseVisualStyleBackColor = true;
            viewUpcomingButton.Click += viewUpcomingButton_Click;
            // downloadUpcomingButton
            downloadUpcomingButton.Enabled = false;
            downloadUpcomingButton.Location = new Point(300, 12);
            downloadUpcomingButton.Name = "downloadUpcomingButton";
            downloadUpcomingButton.Size = new Size(120, 27);
            downloadUpcomingButton.TabIndex = 3;
            downloadUpcomingButton.Text = "Descargar PDF";
            downloadUpcomingButton.UseVisualStyleBackColor = true;
            downloadUpcomingButton.Click += downloadUpcomingButton_Click;
            // upcomingSummaryLabel
            upcomingSummaryLabel.AutoSize = true;
            upcomingSummaryLabel.Location = new Point(12, 50);
            upcomingSummaryLabel.Name = "upcomingSummaryLabel";
            upcomingSummaryLabel.Size = new Size(0, 15);
            upcomingSummaryLabel.TabIndex = 4;
            // upcomingDataGridView
            upcomingDataGridView.AllowUserToAddRows = false;
            upcomingDataGridView.AllowUserToDeleteRows = false;
            upcomingDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            upcomingDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            upcomingDataGridView.Location = new Point(12, 75);
            upcomingDataGridView.MultiSelect = false;
            upcomingDataGridView.Name = "upcomingDataGridView";
            upcomingDataGridView.ReadOnly = true;
            upcomingDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            upcomingDataGridView.Size = new Size(844, 420);
            upcomingDataGridView.TabIndex = 5;
            // resultsTabPage
            resultsTabPage.Controls.Add(fromLabel);
            resultsTabPage.Controls.Add(fromDateTimePicker);
            resultsTabPage.Controls.Add(toLabel);
            resultsTabPage.Controls.Add(toDateTimePicker);
            resultsTabPage.Controls.Add(viewResultsButton);
            resultsTabPage.Controls.Add(downloadResultsButton);
            resultsTabPage.Controls.Add(resultsSummaryLabel);
            resultsTabPage.Controls.Add(chartPanel);
            resultsTabPage.Controls.Add(resultsDataGridView);
            resultsTabPage.Location = new Point(4, 24);
            resultsTabPage.Name = "resultsTabPage";
            resultsTabPage.Padding = new Padding(3);
            resultsTabPage.Size = new Size(868, 508);
            resultsTabPage.TabIndex = 1;
            resultsTabPage.Text = "Resultados de calibraciones";
            resultsTabPage.UseVisualStyleBackColor = true;
            // fromLabel
            fromLabel.AutoSize = true;
            fromLabel.Location = new Point(12, 18);
            fromLabel.Name = "fromLabel";
            fromLabel.Size = new Size(42, 15);
            fromLabel.TabIndex = 0;
            fromLabel.Text = "Desde:";
            // fromDateTimePicker
            fromDateTimePicker.Format = DateTimePickerFormat.Short;
            fromDateTimePicker.Location = new Point(60, 14);
            fromDateTimePicker.Name = "fromDateTimePicker";
            fromDateTimePicker.Size = new Size(110, 23);
            fromDateTimePicker.TabIndex = 1;
            // toLabel
            toLabel.AutoSize = true;
            toLabel.Location = new Point(185, 18);
            toLabel.Name = "toLabel";
            toLabel.Size = new Size(41, 15);
            toLabel.TabIndex = 2;
            toLabel.Text = "Hasta:";
            // toDateTimePicker
            toDateTimePicker.Format = DateTimePickerFormat.Short;
            toDateTimePicker.Location = new Point(232, 14);
            toDateTimePicker.Name = "toDateTimePicker";
            toDateTimePicker.Size = new Size(110, 23);
            toDateTimePicker.TabIndex = 3;
            // viewResultsButton
            viewResultsButton.Location = new Point(357, 12);
            viewResultsButton.Name = "viewResultsButton";
            viewResultsButton.Size = new Size(80, 27);
            viewResultsButton.TabIndex = 4;
            viewResultsButton.Text = "Ver";
            viewResultsButton.UseVisualStyleBackColor = true;
            viewResultsButton.Click += viewResultsButton_Click;
            // downloadResultsButton
            downloadResultsButton.Enabled = false;
            downloadResultsButton.Location = new Point(447, 12);
            downloadResultsButton.Name = "downloadResultsButton";
            downloadResultsButton.Size = new Size(120, 27);
            downloadResultsButton.TabIndex = 5;
            downloadResultsButton.Text = "Descargar PDF";
            downloadResultsButton.UseVisualStyleBackColor = true;
            downloadResultsButton.Click += downloadResultsButton_Click;
            // resultsSummaryLabel
            resultsSummaryLabel.AutoSize = true;
            resultsSummaryLabel.Location = new Point(12, 50);
            resultsSummaryLabel.Name = "resultsSummaryLabel";
            resultsSummaryLabel.Size = new Size(0, 15);
            resultsSummaryLabel.TabIndex = 6;
            // chartPanel
            chartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            chartPanel.Location = new Point(12, 75);
            chartPanel.Name = "chartPanel";
            chartPanel.Size = new Size(844, 100);
            chartPanel.TabIndex = 7;
            chartPanel.Paint += chartPanel_Paint;
            // resultsDataGridView
            resultsDataGridView.AllowUserToAddRows = false;
            resultsDataGridView.AllowUserToDeleteRows = false;
            resultsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resultsDataGridView.Location = new Point(12, 185);
            resultsDataGridView.MultiSelect = false;
            resultsDataGridView.Name = "resultsDataGridView";
            resultsDataGridView.ReadOnly = true;
            resultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            resultsDataGridView.Size = new Size(844, 310);
            resultsDataGridView.TabIndex = 8;
            // ReportsForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 560);
            Controls.Add(reportsTabControl);
            Name = "ReportsForm";
            Text = "Reportes";
            Load += ReportsForm_Load;
            reportsTabControl.ResumeLayout(false);
            upcomingTabPage.ResumeLayout(false);
            upcomingTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)upcomingDataGridView).EndInit();
            resultsTabPage.ResumeLayout(false);
            resultsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)resultsDataGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private TabControl reportsTabControl;
        private TabPage upcomingTabPage;
        private Label daysLabel;
        private ComboBox daysComboBox;
        private Button viewUpcomingButton;
        private Button downloadUpcomingButton;
        private Label upcomingSummaryLabel;
        private DataGridView upcomingDataGridView;
        private TabPage resultsTabPage;
        private Label fromLabel;
        private DateTimePicker fromDateTimePicker;
        private Label toLabel;
        private DateTimePicker toDateTimePicker;
        private Button viewResultsButton;
        private Button downloadResultsButton;
        private Label resultsSummaryLabel;
        private Panel chartPanel;
        private DataGridView resultsDataGridView;
    }
}
