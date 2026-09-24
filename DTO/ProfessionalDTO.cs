namespace MAVE.DTO
{
    /// <summary>Directorio público de profesionales (sin datos sensibles).</summary>
    public class ProfessionalDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Verified { get; set; }
    }

    public class PsychologistProfileDTO
    {
        public string Description { get; set; } = string.Empty;
        public string CredentialUrl { get; set; } = string.Empty;
        public bool Verified { get; set; }
    }
}
