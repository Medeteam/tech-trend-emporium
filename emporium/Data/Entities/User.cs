﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class User
    {
        [Key]
        public Guid User_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public required string Username { get; set; }

        [MaxLength(255)]
        public required string Password { get; set; }

        [MaxLength(255)]
        public required string Email { get; set; }

        [MaxLength(120)]
        public required string SecurityQuestion { get; set; }

        [MaxLength(255)]
        public required string SecurityAnswer { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("Role")]
        public Guid Role_id { get; set; }
        public Role? Role { get; set; }
        public string? RoleName => Role?.RoleName;

        public WishList? WishList { get; set; }
        public Cart? Cart { get; set; }


    }
}
