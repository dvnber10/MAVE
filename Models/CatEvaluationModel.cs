using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatEvaluationModel
{
    public short EvaluationId { get; set; }

    public string Result { get; set; } = null!;

    public virtual ICollection<UserModel> Users { get; set; } = new List<UserModel>();
}
