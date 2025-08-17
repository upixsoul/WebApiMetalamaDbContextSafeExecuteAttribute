using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Core.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}
