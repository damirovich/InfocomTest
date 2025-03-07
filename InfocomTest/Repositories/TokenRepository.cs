using InfocomTest.Data.EntityModel;
using InfocomTest.Data;
using InfocomTest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfocomTest.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;

    public TokenRepository(AppDbContext context)
    {
        _context = context;
    }

    // Проверяет, был ли токен аннулирован.
    public async Task<bool> IsTokenRevokedAsync(string jti)
    {
        return await _context.RevokedTokens.AnyAsync(t => t.Jti == jti);
    }

    // Аннулирует токен, добавляя его в список отозванных.
    public async Task InvalidateTokenAsync(string jti, DateTime expiration)
    {
        _context.RevokedTokens.Add(new RevokedTokenEntity { Jti = jti, Expiration = expiration });
        await _context.SaveChangesAsync();
    }

}