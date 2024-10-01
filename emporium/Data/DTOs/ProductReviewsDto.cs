using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ProductReviewsDto
    {
        public Guid Product_id { get; set; }
        public List<ReviewResponseDto> Reviews { get; set; }
    }

    public class ReviewResponseDto
    {
        public string User { get; set; }
        public string Comment { get; set; }
    }

}
