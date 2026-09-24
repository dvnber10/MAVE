namespace MAVE.DTO
{
    public class DayTimeDTO
    {
        public string Date { get; set; } = string.Empty;
        public int Seconds { get; set; }
    }

    public class TypeTimeDTO
    {
        public string Action { get; set; } = string.Empty;
        public int Seconds { get; set; }
        public int Sessions { get; set; }
    }

    public class ActivityTimeDTO
    {
        public int MeditationSeconds { get; set; }
        public int MeditationSessions { get; set; }
        public int MoodsCount { get; set; }
        public int HabitsCount { get; set; }
        public List<DayTimeDTO> Last7Days { get; set; } = new List<DayTimeDTO>();
        public List<TypeTimeDTO> ByType { get; set; } = new List<TypeTimeDTO>();
    }
}
