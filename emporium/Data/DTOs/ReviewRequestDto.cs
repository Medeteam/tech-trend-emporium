using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ReviewRequestDto
    {
        public string User {  get; set; }
        public string Comment { get; set; }
        public decimal Rate { get; set; }
    }
}
