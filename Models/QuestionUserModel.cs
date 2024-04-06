using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class QuestionUserModel
{
    public int UserId { get; set; }

    public int HabitId { get; set; }

    public DateOnly Date { get; set; }

    public int HabitUserId { get; set; }

    public virtual QuestionModel Habit { get; set; } = null!;

    public virtual UserModel User { get; set; } = null!;
}
