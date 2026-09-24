namespace MAVE.DTO
{
    public class ActivityLogDTO
    {
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int Seconds { get; set; }
    }
}
