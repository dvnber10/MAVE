using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.DTO
{
    public class UpdateUserDTO
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public short IdRole { get; set; } 
    }
}