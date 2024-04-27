using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVE.DTO
{
    public class ArticleDTO
    {
        public string? ArticleName { get; set; }
        public string? HealthProf { get; set;}
        public string? Resume { get; set;}
        public string? Picture { get; set; }
        public string? Link { get; set; }
        public string? Type { get; set; }
        public DateOnly Date { get; set; }
    }
}