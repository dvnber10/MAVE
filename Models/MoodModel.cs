using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class MoodModel
{
    public int MoodId { get; set; }

    public int UserId { get; set; }

    public short MoodScore { get; set; }

    public DateTime Date { get; set; }

    public virtual UserModel User { get; set; } = null!;
}
