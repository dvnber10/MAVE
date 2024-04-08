using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public short RoleId { get; set; }

    public string? Name { get; set; }

    public short EvaluationId { get; set; }

    public short StatusId { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual CatEvaluation Evaluation { get; set; } = null!;

    public virtual ICollection<Mood> Moods { get; set; } = new List<Mood>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual CatRole Role { get; set; } = null!;

    public virtual CatStatus Status { get; set; } = null!;
}
