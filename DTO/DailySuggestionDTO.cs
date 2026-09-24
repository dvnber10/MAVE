namespace MAVE.DTO
{
    public class DailySuggestionDTO
    {
        public string Suggestion { get; set; } = string.Empty;
        public bool PendingMood { get; set; }
        public bool PendingHabits { get; set; }
    }
}
