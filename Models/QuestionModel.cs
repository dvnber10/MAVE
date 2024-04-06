using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class QuestionModel
{
    public int? Score { get; set; }

    public int QuestionId { get; set; }

    public short CatQuestionId { get; set; }

    public string? EvaluationMax { get; set; }

    public string? EvaluationMin { get; set; }

    public virtual CatQuestion CatQuestion { get; set; } = null!;

    public virtual ICollection<QuestionUserModel> QuestionUsers { get; set; } = new List<QuestionUserModel>();
}
