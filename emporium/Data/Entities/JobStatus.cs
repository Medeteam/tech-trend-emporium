using System.ComponentModel.DataAnnotations;
namespace Data.Entities
{
    public class JobStatus
    {

        [Key]
        public Guid Job_status_id { get; set; } = Guid.NewGuid();

        [MaxLength (255)]
        public required string Job_status { get; set; }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
