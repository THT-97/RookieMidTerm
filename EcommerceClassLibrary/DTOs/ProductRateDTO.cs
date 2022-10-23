namespace Ecommerce.DTO.DTOs
{
    public class ProductRateDTO
    {
        public int ProductId { get; set; }
        public string UserEmail { get; set; }
        public string? Comment { get; set; }
        public byte Rate { get; set; }
    }
}
