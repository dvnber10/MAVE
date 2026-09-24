namespace MAVE.DTO
{
    public class MeditationSoundDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public string StreamUrl { get; set; } = string.Empty;
    }
}
