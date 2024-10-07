using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class WishListDto
    {
        public Guid userId { get; set; }
        public List<Guid> productList { get; set; }
    }
}
