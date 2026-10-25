namespace DTOs.Reports
{
    public class CalibrationResultsReportDTO
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Total { get; set; }
        public List<ResultTotalDTO> Totals { get; set; } = new List<ResultTotalDTO>();
        public List<CalibrationResultRowDTO> Rows { get; set; } = new List<CalibrationResultRowDTO>();
    }

    public class ResultTotalDTO
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CalibrationResultRowDTO
    {
        public DateTime CalibrationDate { get; set; }
        public string InstrumentCode { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public bool IsExternal { get; set; }
        public string? CertificateNumber { get; set; }
    }
}
