using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        public string Name { get; set; }
        public string? Colors { get; set; }
        public string? Sizes { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter prouct price")]
        public float ListPrice { get; set; }

        public float? SalePrice { get; set; }
        public string? Images { get; set; }

        [Required(ErrorMessage = "Product's created date missing")]
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "Please enter product quantity")]
        public int Quantity { get; set; }
        public float? Rating { get; set; }
        public int? RatingCount { get; set; }

        [Required(ErrorMessage = "Please set product status")]
        public byte Status { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
