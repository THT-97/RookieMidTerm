using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Data.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Brand name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public byte Status { get; set; }

        public List<Product> Products { get; set; }
    }
}
