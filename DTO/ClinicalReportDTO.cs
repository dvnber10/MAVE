namespace MAVE.DTO
{
    public class Phq4LastDTO
    {
        public string Date { get; set; } = string.Empty;
        public int D { get; set; }
        public int A { get; set; }
        public int Total { get; set; }
        public string Band { get; set; } = string.Empty;
        public bool NeedsFollowUp { get; set; }
    }

    public class ClinicalReportDTO
    {
        public string PatientName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ActivityTimeDTO Activity { get; set; } = new ActivityTimeDTO();
        public Phq4LastDTO? LastPhq4 { get; set; }
        public List<Phq4PointDTO> Phq4History { get; set; } = new List<Phq4PointDTO>();
        public int DaysMood30 { get; set; }
        public int DaysHabits30 { get; set; }
        public List<HabitAnswerDTO> RecentHabits { get; set; } = new List<HabitAnswerDTO>();
    }
}
