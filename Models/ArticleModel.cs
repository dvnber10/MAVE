using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class ArticleModel
{
    public int ArticleId { get; set; }

    public int UserId { get; set; }

    public string ArticleName { get; set; } = null!;

    public string Resume { get; set; } = null!;

    public string Picture { get; set; } = null!;

    public string Link { get; set; } = null!;

    public short TypeId { get; set; }

    public virtual CatArticleTypeModel Type { get; set; } = null!;

    public virtual UserModel User { get; set; } = null!;
}
