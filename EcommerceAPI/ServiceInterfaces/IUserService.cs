using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API.ServiceInterfaces
{
    public interface IUserService : ICRUDService<IdentityUser>
    {
    }
}
