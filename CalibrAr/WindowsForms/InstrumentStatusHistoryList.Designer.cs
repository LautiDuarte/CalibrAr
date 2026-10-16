namespace WindowsForms
{
    partial class InstrumentStatusHistoryList
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
            statusHistoriesDataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)statusHistoriesDataGridView).BeginInit();
            SuspendLayout();
            // statusHistoriesDataGridView
            statusHistoriesDataGridView.AllowUserToAddRows = false;
            statusHistoriesDataGridView.AllowUserToDeleteRows = false;
            statusHistoriesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusHistoriesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            statusHistoriesDataGridView.Location = new Point(12, 12);
            statusHistoriesDataGridView.MultiSelect = false;
            statusHistoriesDataGridView.Name = "statusHistoriesDataGridView";
            statusHistoriesDataGridView.ReadOnly = true;
            statusHistoriesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            statusHistoriesDataGridView.Size = new Size(836, 446);
            statusHistoriesDataGridView.TabIndex = 0;
            // InstrumentStatusHistoryList
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 470);
            Controls.Add(statusHistoriesDataGridView);
            Name = "InstrumentStatusHistoryList";
            Text = "Instrument status history";
            Load += InstrumentStatusHistoryList_Load;
            ((System.ComponentModel.ISupportInitialize)statusHistoriesDataGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private DataGridView statusHistoriesDataGridView;
    }
}