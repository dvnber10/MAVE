using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatQuestion
{
    public short CatQuestionId { get; set; }

    public string Question { get; set; } = null!;

    public bool Initial { get; set; }

    public virtual ICollection<CatOption> CatOptions { get; set; } = new List<CatOption>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
