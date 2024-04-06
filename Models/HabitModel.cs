using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class HabitModel
{
    public int Score { get; set; }

    public int HabitId { get; set; }

    public short QuestionId { get; set; }

    public virtual ICollection<HabitUserModel> HabitUsers { get; set; } = new List<HabitUserModel>();

    public virtual CatQuestion Question { get; set; } = null!;
}
