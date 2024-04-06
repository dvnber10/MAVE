using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatRole
{
    public short RoleId { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
