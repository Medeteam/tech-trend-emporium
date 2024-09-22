using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Category_id { get; set; } = Guid.NewGuid();


        [MaxLength(255)]
        public required string  Category_name { get; set; }

        [MaxLength(255)]
        public required string Category_description { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("User")]
        public Guid User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("JobStatus")]
        public Guid Job_status_id { get; set; }
        public JobStatus Job_status { get; set; }

        public ProductToCategory ProductToCategory { get; set; }

    }
}
