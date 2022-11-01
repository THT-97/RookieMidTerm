namespace Ecommerce.DTO.DTOs
{
    public class ProductADTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Colors { get; set; }
        public string? Sizes { get; set; }
        public string? Description { get; set; }
        public float ListPrice { get; set; }
        public float? SalePrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Images { get; set; }
        public int Quantity { get; set; }
        public float? Rating { get; set; }
        public int? RatingCount { get; set; }
        public byte Status { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public ICollection<RatingDTO> Ratings { get; set; }
    }
}
