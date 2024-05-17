using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatArticleType
{
    public short TypeId { get; set; }

    public string ArticleType { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
