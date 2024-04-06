using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatEvaluation
{
    public short EvaluationId { get; set; }

    public string Result { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
