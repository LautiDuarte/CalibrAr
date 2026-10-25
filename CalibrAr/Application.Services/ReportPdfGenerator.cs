using DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services
{
    public class ReportPdfGenerator
    {
        static ReportPdfGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateUpcomingCalibrations(UpcomingCalibrationsReportDTO report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Calibraciones próximas y vencidas").FontSize(16).Bold();
                        col.Item().Text($"Próximos {report.Days} días · Generado el {report.GeneratedAt:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"Vencidas: {report.OverdueCount} · Próximas: {report.UpcomingCount}");
                    });

                    page.Content().PaddingVertical(10).Element(c =>
                    {
                        if (report.Rows.Count == 0)
                        {
                            c.Text("No hay calibraciones en este período.");
                            return;
                        }

                        c.Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(1.2f);
                                cols.RelativeColumn(2.5f);
                                cols.RelativeColumn(1.5f);
                                cols.RelativeColumn(1.3f);
                                cols.RelativeColumn(1.3f);
                                cols.RelativeColumn(1.2f);
                            });

                            table.Header(header =>
                            {
                                foreach (var title in new[] { "Código", "Instrumento", "Área", "Última calibración", "Próxima calibración", "Días" })
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(title).Bold();
                            });

                            foreach (var row in report.Rows)
                            {
                                table.Cell().Padding(4).Text(row.InstrumentCode);
                                table.Cell().Padding(4).Text(row.InstrumentName);
                                table.Cell().Padding(4).Text(row.AreaName);
                                table.Cell().Padding(4).Text(row.LastCalibrationDate?.ToString("dd/MM/yyyy") ?? "-");
                                table.Cell().Padding(4).Text(row.NextCalibrationDate.ToString("dd/MM/yyyy"));
                                table.Cell().Padding(4).Text(row.DaysRemaining < 0
                                    ? $"Vencida ({-row.DaysRemaining})"
                                    : row.DaysRemaining.ToString());
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        public byte[] GenerateCalibrationResults(CalibrationResultsReportDTO report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Resultados de calibraciones").FontSize(16).Bold();
                        col.Item().Text($"Del {report.From:dd/MM/yyyy} al {report.To:dd/MM/yyyy} · Generado el {report.GeneratedAt:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"Total de calibraciones: {report.Total}");
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        if (report.Total == 0)
                        {
                            col.Item().Text("No hay calibraciones en este período.");
                            return;
                        }

                        col.Item().PaddingBottom(15).Element(c => DrawBarChart(c, report.Totals));

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(1.3f);
                                cols.RelativeColumn(1.5f);
                                cols.RelativeColumn(2.2f);
                                cols.RelativeColumn(1.2f);
                                cols.RelativeColumn(1.8f);
                            });

                            table.Header(header =>
                            {
                                foreach (var title in new[] { "Fecha", "Instrumento", "Resultado", "Tipo", "Certificado" })
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(title).Bold();
                            });

                            foreach (var row in report.Rows)
                            {
                                table.Cell().Padding(4).Text(row.CalibrationDate.ToString("dd/MM/yyyy"));
                                table.Cell().Padding(4).Text(row.InstrumentCode);
                                table.Cell().Padding(4).Text(row.Result);
                                table.Cell().Padding(4).Text(row.IsExternal ? "Externa" : "Interna");
                                table.Cell().Padding(4).Text(row.CertificateNumber ?? "-");
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        // Barras horizontales: el ancho de cada una es proporcional a su cantidad
        private static void DrawBarChart(IContainer container, List<ResultTotalDTO> totals)
        {
            int max = totals.Max(t => t.Count);
            string[] colors = [Colors.Green.Medium, Colors.Orange.Medium, Colors.Red.Medium];

            container.Column(col =>
            {
                for (int i = 0; i < totals.Count; i++)
                {
                    var item = totals[i];
                    var color = colors[i % colors.Length];

                    col.Item().PaddingVertical(3).Row(row =>
                    {
                        row.ConstantItem(150).AlignMiddle().Text(item.Label);

                        row.RelativeItem().Row(bar =>
                        {
                            if (item.Count > 0)
                                bar.RelativeItem(item.Count).Height(16).Background(color);
                            if (max - item.Count > 0)
                                bar.RelativeItem(max - item.Count);
                        });

                        row.ConstantItem(30).AlignMiddle().AlignRight().Text(item.Count.ToString());
                    });
                }
            });
        }
    }
}
