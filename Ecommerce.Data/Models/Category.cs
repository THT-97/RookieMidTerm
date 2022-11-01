using Ecommerce.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Category name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public byte Status { get; set; }

        public List<Product> Products { get; set; }
    }
}
