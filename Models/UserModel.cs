using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class UserModel
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public short RoleId { get; set; }

    public string? Name { get; set; }

    public short EvaluationId { get; set; }

    public virtual ICollection<ArticleModel> Articles { get; set; } = new List<ArticleModel>();

    public virtual CatEvaluationModel Evaluation { get; set; } = null!;

    public virtual ICollection<MoodModel> Moods { get; set; } = new List<MoodModel>();

    public virtual ICollection<QuestionUserModel> QuestionUsers { get; set; } = new List<QuestionUserModel>();

    public virtual CatRoleModel Role { get; set; } = null!;
}
