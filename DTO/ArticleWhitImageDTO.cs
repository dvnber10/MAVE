using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.DTO
{
    public class ArticleWhitImageDTO
    {
        public string? ArticleName { get; set; }
        public string? Resume { get; set;}
        public string? Link { get; set; }
        public short Type { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public IFormFile? Image { get; set; }
    }
}