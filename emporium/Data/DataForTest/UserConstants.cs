using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataForTest
{
    public class UserConstants
    {
        public static List<User> Users = new List<User>()
        {
            new User() { Username = "jason_admin", Email = "jason.admin@email.com", Password = "MyPass_w0rd", Role = new Role() { RoleName = "Admin" } },
            new User() { Username = "elyse_shopper", Email = "elyse.shopper@email.com", Password = "MyPass_w0rd", Role = new Role() { RoleName = "User" } },
            new User() { Username = "pepe_employee", Email = "pepe.employee@email.com", Password = "MyPass_w0rd", Role = new Role() { RoleName = "Employee" } },
        };
    }
}
