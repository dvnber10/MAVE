using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class AuditoryModel
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string OldValue { get; set; } = null!;

    public string NewValue { get; set; } = null!;

    public DateTime Date { get; set; }
}
