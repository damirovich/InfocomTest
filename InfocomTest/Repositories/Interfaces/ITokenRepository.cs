using InfocomTest.Data.EntityModel;

namespace InfocomTest.Repositories.Interfaces;

public interface ITokenRepository 
{
    Task<bool> IsTokenRevokedAsync(string jti);
    Task InvalidateTokenAsync(string jti, DateTime expiration);
}