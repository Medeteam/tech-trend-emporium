using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class CreateCouponDto
    {
        public string Coupon_name { get; set; }
        public int Discount { get; set; }
        public string Code { get; set; }
        public bool Coupon_status { get; set; }
    }
}
