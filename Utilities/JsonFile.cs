using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.Utilities
{
    public class JsonFile
    {
        public string Id { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int Status { get; set; }
    }
}