using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public byte Points { get; set; }
        public string? Comment { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}
