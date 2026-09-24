namespace MAVE.DTO
{
    public class KpiDTO
    {
        public int TotalUsers { get; set; }
        public int SuperAdmins { get; set; }
        public int Admins { get; set; }
        public int Psychologists { get; set; }
        public int Patients { get; set; }
        public int ActiveUsers7d { get; set; }
        public int ActivityMinutes { get; set; }
        public int ChatMessages { get; set; }
        public int Phq4Count30d { get; set; }
        public double Phq4Avg30d { get; set; }
    }

    public class AdminActivityLogDTO
    {
        public DateTime Date { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }

    public class ProfessionalSignupDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class PendingPsyDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CredentialUrl { get; set; } = string.Empty;
        public bool Verified { get; set; }
    }
}
