using InfocomTest.Data.EntityModel;
using InfocomTest.Models.Settings;
using InfocomTest.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfocomTest.Services;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ITokenRepository _tokenRepository;

    public TokenService(IOptions<JwtSettings> jwtSettings, ITokenRepository tokenRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenRepository = tokenRepository;
    }

    /// <summary>
    /// Генерирует JWT-токен для пользователя.
    /// </summary>
    public string GenerateJwtToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var jti = Guid.NewGuid().ToString(); // Уникальный идентификатор токена

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id), 
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpireHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Аннулирует токен, добавляя его в список отозванных токенов.
    /// </summary>
    /// <param name="jti">Уникальный идентификатор токена (JTI).</param>
    /// <param name="expiration">Дата и время истечения токена.</param>
    public async Task InvalidateTokenAsync(string jti, DateTime expiration)
    {
        await _tokenRepository.InvalidateTokenAsync(jti, expiration);
    }

    /// <summary>
    /// Проверяет, был ли токен аннулирован.
    /// </summary>
    /// <param name="jti">Уникальный идентификатор токена (JTI).</param>
    /// <returns>True, если токен отозван, иначе false.</returns>
    public async Task<bool> IsTokenInvalidAsync(string jti)
    {
        return await _tokenRepository.IsTokenRevokedAsync(jti);
    }
}
