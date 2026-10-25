namespace DTOs.Reports
{
    public class UpcomingCalibrationsReportDTO
    {
        public DateTime GeneratedAt { get; set; }
        public int Days { get; set; }
        public int OverdueCount { get; set; }
        public int UpcomingCount { get; set; }
        public List<UpcomingCalibrationRowDTO> Rows { get; set; } = new List<UpcomingCalibrationRowDTO>();
    }

    public class UpcomingCalibrationRowDTO
    {
        public string InstrumentCode { get; set; } = string.Empty;
        public string InstrumentName { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime NextCalibrationDate { get; set; }
        // Negativo = ya vencida
        public int DaysRemaining { get; set; }
    }
}
