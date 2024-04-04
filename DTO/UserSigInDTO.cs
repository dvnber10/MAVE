using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.DTO
{
    public class UserSigInDTO
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int Phone { get; set; }

        public string Password { get; set; } = null!;
    }
}