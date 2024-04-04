using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class HabitUser
{
    public int UserId { get; set; }

    public int HabitId { get; set; }

    public DateOnly Date { get; set; }

    public int HabitUserId { get; set; }

    public bool Initial { get; set; }

    public virtual Question Habit { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
