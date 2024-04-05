using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.DTO
{
    public class EmailDTO
    {
        public string Addressee { get; set; } = String.Empty;
        public string Affair { get; set; } = String.Empty;
        public string Contain { get; set; } = String.Empty;
    }
}