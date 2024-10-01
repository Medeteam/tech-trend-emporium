using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class CategoryDto
    {
        public Guid id { get; set; }
        public required string name { get; set; }
        public string? description { get; set; }
    }
}
