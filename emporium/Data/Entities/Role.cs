using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Role
    {
        [Key]
        public Guid Role_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public required string RoleName { get; set; }

        public List<User>? Users { get; set; } 
    }
}
