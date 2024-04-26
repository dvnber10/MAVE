using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatStatus
{
    public short StatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
