using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatArticleTypeModel
{
    public short TypeId { get; set; }

    public string ArticleType { get; set; } = null!;

    public virtual ICollection<ArticleModel> Articles { get; set; } = new List<ArticleModel>();
}
