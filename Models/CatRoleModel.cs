using System;
using System.Collections.Generic;

namespace MAVE.Models;

public partial class CatRoleModel
{
    public short RoleId { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<UserModel> Users { get; set; } = new List<UserModel>();
}
