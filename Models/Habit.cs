using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class Habit
{
    public int Score { get; set; }

    public int HabitId { get; set; }

    public short QuestionId { get; set; }

    public virtual ICollection<HabitUser> HabitUsers { get; set; } = new List<HabitUser>();

    public virtual CatQuestion Question { get; set; } = null!;
}
