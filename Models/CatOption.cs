using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatOption
{
    public int OptionId { get; set; }

    public string EvaOption { get; set; } = null!;

    public string Value { get; set; } = null!;

    public short CatQuestionId { get; set; }

    public string Abcd { get; set; } = null!;

    public virtual CatQuestion CatQuestion { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
