using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ProductToCategory
    {
        [Key]
        public Guid Product_category_id {  get; set; } = Guid.NewGuid();

        [ForeignKey("Product")]
        public Guid Product_id { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Category")]
        public Guid Category_id { get; set; }
        public Category Category { get; set; }
    }
}
