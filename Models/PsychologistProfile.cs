using System;

namespace MAVE.Models;

public partial class PsychologistProfile
{
    public int UserId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? CredentialUrl { get; set; }

    public bool Verified { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
