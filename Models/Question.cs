using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class Question
{
    public int? Score { get; set; }

    public int QuestionId { get; set; }

    public short CatQuestionId { get; set; }

    public string? EvaluationMax { get; set; }

    public string? EvaluationMin { get; set; }

    public virtual CatQuestion CatQuestion { get; set; } = null!;

    public virtual ICollection<QuestionUser> QuestionUsers { get; set; } = new List<QuestionUser>();
}
