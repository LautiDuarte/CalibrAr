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
using DTOs.Reports;

namespace WindowsForms
{
    public partial class ReportsForm : Form
    {
        private readonly ReportApiClient reportApiClient = new(AuthServiceProvider.Instance);

        private static readonly Color[] BarColors =
        {
            Color.FromArgb(76, 175, 80),
            Color.FromArgb(255, 152, 0),
            Color.FromArgb(244, 67, 54)
        };

        private UpcomingCalibrationsReportDTO? upcoming;
        private CalibrationResultsReportDTO? results;

        public ReportsForm()
        {
            InitializeComponent();
            ConfigureColumns();
        }

        private static void AddColumn(DataGridView grid, string name, string header, int width, string? format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width };
            if (format != null) column.DefaultCellStyle.Format = format;
            grid.Columns.Add(column);
        }

        private void ConfigureColumns()
        {
            upcomingDataGridView.AutoGenerateColumns = false;
            AddColumn(upcomingDataGridView, "InstrumentCode", "Código", 100);
            AddColumn(upcomingDataGridView, "InstrumentName", "Instrumento", 200);
            AddColumn(upcomingDataGridView, "AreaName", "Área", 140);
            AddColumn(upcomingDataGridView, "LastCalibrationDate", "Última calibración", 130, "dd/MM/yyyy");
            AddColumn(upcomingDataGridView, "NextCalibrationDate", "Próxima calibración", 130, "dd/MM/yyyy");
            AddColumn(upcomingDataGridView, "DaysRemaining", "Días", 100);
            upcomingDataGridView.CellFormatting += upcomingDataGridView_CellFormatting;

            resultsDataGridView.AutoGenerateColumns = false;
            AddColumn(resultsDataGridView, "CalibrationDate", "Fecha", 110, "dd/MM/yyyy");
            AddColumn(resultsDataGridView, "InstrumentCode", "Instrumento", 120);
            AddColumn(resultsDataGridView, "Result", "Resultado", 200);
            AddColumn(resultsDataGridView, "IsExternal", "Tipo", 80);
            AddColumn(resultsDataGridView, "CertificateNumber", "Certificado", 160);
            resultsDataGridView.CellFormatting += resultsDataGridView_CellFormatting;
        }

        private void resultsDataGridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || resultsDataGridView.Columns[e.ColumnIndex].Name != "IsExternal") return;

            if (e.Value is bool isExternal)
            {
                e.Value = isExternal ? "Externa" : "Interna";
                e.FormattingApplied = true;
            }
        }

        // Los días negativos son calibraciones ya vencidas
        private void upcomingDataGridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || upcomingDataGridView.Columns[e.ColumnIndex].Name != "DaysRemaining") return;

            if (e.Value is int days && days < 0)
            {
                e.Value = $"Vencida ({-days})";
                e.CellStyle!.ForeColor = Color.Firebrick;
                e.FormattingApplied = true;
            }
        }

        private async void ReportsForm_Load(object sender, EventArgs e)
        {
            daysComboBox.SelectedIndex = 0;
            fromDateTimePicker.Value = DateTime.Today.AddMonths(-12);
            toDateTimePicker.Value = DateTime.Today;

            await LoadUpcomingAsync();
            await LoadResultsAsync();
        }

        private async Task LoadUpcomingAsync()
        {
            try
            {
                upcoming = await reportApiClient.GetUpcomingCalibrationsAsync(SelectedDays());
                upcomingDataGridView.DataSource = upcoming.Rows;
                upcomingSummaryLabel.Text = upcoming.Rows.Count == 0
                    ? "No hay calibraciones en este período."
                    : $"Vencidas: {upcoming.OverdueCount} · Próximas: {upcoming.UpcomingCount}";
                downloadUpcomingButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadResultsAsync()
        {
            try
            {
                results = await reportApiClient.GetCalibrationResultsAsync(fromDateTimePicker.Value.Date, toDateTimePicker.Value.Date);
                resultsDataGridView.DataSource = results.Rows;
                resultsSummaryLabel.Text = results.Total == 0
                    ? "No hay calibraciones en este período."
                    : $"Total de calibraciones: {results.Total}";
                downloadResultsButton.Enabled = true;
                chartPanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int SelectedDays() => int.Parse(daysComboBox.Text.Split(' ')[0]);

        private async void viewUpcomingButton_Click(object sender, EventArgs e)
        {
            await LoadUpcomingAsync();
        }

        private async void viewResultsButton_Click(object sender, EventArgs e)
        {
            await LoadResultsAsync();
        }

        private async void downloadUpcomingButton_Click(object sender, EventArgs e)
        {
            await SavePdfAsync(() => reportApiClient.GetUpcomingCalibrationsPdfAsync(SelectedDays()), $"calibraciones-proximas-{DateTime.Now:yyyyMMdd}.pdf");
        }

        private async void downloadResultsButton_Click(object sender, EventArgs e)
        {
            await SavePdfAsync(() => reportApiClient.GetCalibrationResultsPdfAsync(fromDateTimePicker.Value.Date, toDateTimePicker.Value.Date), $"resultados-calibraciones-{DateTime.Now:yyyyMMdd}.pdf");
        }

        private async Task SavePdfAsync(Func<Task<byte[]>> getPdf, string suggestedName)
        {
            using var dialog = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf", FileName = suggestedName };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                byte[] pdf = await getPdf();
                await System.IO.File.WriteAllBytesAsync(dialog.FileName, pdf);
                MessageBox.Show("El PDF se guardó correctamente.", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Barras horizontales: el ancho de cada una es proporcional a su cantidad
        private void chartPanel_Paint(object sender, PaintEventArgs e)
        {
            if (results == null || results.Total == 0) return;

            int max = results.Totals.Max(t => t.Count);
            int labelWidth = 200;
            int countWidth = 40;
            int barAreaWidth = chartPanel.ClientSize.Width - labelWidth - countWidth;
            int rowHeight = 30;

            for (int i = 0; i < results.Totals.Count; i++)
            {
                var item = results.Totals[i];
                int y = 8 + i * rowHeight;
                int barWidth = max == 0 ? 0 : item.Count * barAreaWidth / max;

                e.Graphics.DrawString(item.Label, Font, Brushes.Black, 0, y + 3);
                using (var brush = new SolidBrush(BarColors[i % BarColors.Length]))
                {
                    e.Graphics.FillRectangle(brush, labelWidth, y, barWidth, 20);
                }
                e.Graphics.DrawString(item.Count.ToString(), Font, Brushes.Black, labelWidth + barAreaWidth + 8, y + 3);
            }
        }
    }
}
