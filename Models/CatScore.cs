using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatScore
{
    public short ScoreId { get; set; }

    public string ScoreType { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
