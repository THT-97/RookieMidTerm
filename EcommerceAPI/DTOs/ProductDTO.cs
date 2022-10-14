namespace EcommerceAPI.DTOs
{
    //DTO class for Product
    public class ProductDTO
    {
        public string Name { get; set; }
        public string? Colors { get; set; }
        public string? Sizes { get; set; }
        public string? Description { get; set; }
        public float ListPrice { get; set; }
        public float? SalePrice { get; set; }
        public string? Images { get; set; }
        public int Quantity { get; set; }
        public float? Rating { get; set; }
        public int? RatingCount { get; set; }
    }
}
