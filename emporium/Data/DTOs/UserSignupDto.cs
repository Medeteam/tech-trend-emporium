﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class UserSignupDto
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
