using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class Mood
{
    public int MoodId { get; set; }

    public int UserId { get; set; }

    public short MoodScore { get; set; }

    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}
