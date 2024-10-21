using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class UserRecoverDto
    {
        public string email { get; set; }
        public string qAnswer { get; set; }
        public string newPassword { get; set; }
    }
}
