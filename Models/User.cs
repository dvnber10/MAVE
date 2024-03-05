using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAVE.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? NameU { get; set; }

    public string Email { get; set; } = null!;

    public int? Phone { get; set; }

    public string Pass { get; set; } = null!;
}
