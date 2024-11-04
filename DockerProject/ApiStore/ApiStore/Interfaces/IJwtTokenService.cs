using ApiStore.Data.Entities.Identity;

namespace ApiStore.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}
