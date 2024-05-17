using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class Question
{
    public short? ScoreId { get; set; }

    public int QuestionId { get; set; }

    public short CatQuestionId { get; set; }

    public int? OptionId { get; set; }

    public DateOnly Date { get; set; }

    public int UserId { get; set; }

    public virtual CatQuestion CatQuestion { get; set; } = null!;

    public virtual CatOption? Option { get; set; }

    public virtual CatScore? Score { get; set; }

    public virtual User User { get; set; } = null!;
}
