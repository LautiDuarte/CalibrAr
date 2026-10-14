using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using API.Auth.WindowsForms;
using API.Clients;

namespace WindowsForms
{
    public partial class InstrumentStatusHistoryList : Form
    {
        private readonly InstrumentStatusHistoryApiClient instrumentStatusHistoryApiClient = new(AuthServiceProvider.Instance);

        public InstrumentStatusHistoryList()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private void AddColumn(string name, string header, int width, string? format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width };
            if (format != null) column.DefaultCellStyle.Format = format;
            statusHistoriesDataGridView.Columns.Add(column);
        }

        private void ConfigureColumns()
        {
            statusHistoriesDataGridView.AutoGenerateColumns = false;
            AddColumn("InstrumentCode", "Instrument", 120);
            AddColumn("PreviousStatus", "Previous status", 150);
            AddColumn("NewStatus", "New status", 150);
            AddColumn("ChangedAt", "Changed at", 150, "dd/MM/yyyy HH:mm:ss");
            AddColumn("ChangedByUserName", "Changed by", 200);
            // Los cambios automáticos del sistema no tienen usuario
            statusHistoriesDataGridView.Columns["ChangedByUserName"].DefaultCellStyle.NullValue = "System";
        }

        private async void InstrumentStatusHistoryList_Load(object sender, EventArgs e)
        {
            try
            {
                statusHistoriesDataGridView.DataSource = await instrumentStatusHistoryApiClient.GetAllAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading instrument status history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
