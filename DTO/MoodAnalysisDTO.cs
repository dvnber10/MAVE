namespace MAVE.DTO
{
    public class MoodTrendPointDTO
    {
        public string Date { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class MoodAnalysisDTO
    {
        public int FacesCount { get; set; }
        public double FacesAvg { get; set; }
        public List<int> Distribution { get; set; } = new List<int> { 0, 0, 0, 0, 0 };
        public double Volatility { get; set; }
        public int StreakDays { get; set; }
        public string Trend { get; set; } = "stable";
        public List<MoodTrendPointDTO> FacesTrend { get; set; } = new List<MoodTrendPointDTO>();
        public int? Index { get; set; }
        public string IndexBand { get; set; } = string.Empty;
        public int DaysActive30 { get; set; }
        public Phq4LastDTO? LastPhq4 { get; set; }
    }
}
