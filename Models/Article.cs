using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public int UserId { get; set; }

    public string ArticleName { get; set; } = null!;

    public string Resume { get; set; } = null!;

    public string Picture { get; set; } = null!;

    public string Link { get; set; } = null!;

    public short TypeId { get; set; }

    public virtual CatArticleType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
