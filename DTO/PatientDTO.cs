namespace MAVE.DTO
{
    public class PatientDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class SetPsychologistDTO
    {
        public int PsychologistId { get; set; }
    }

    public class HabitAnswerDTO
    {
        public string Date { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
