using InfocomTest.Services;
using System.IdentityModel.Tokens.Jwt;

namespace InfocomTest.Middleware;

/// <summary>
/// Middleware для проверки аннулированных (отозванных) токенов.
/// Если токен найден в списке аннулированных, возвращается 401 Unauthorized.
/// </summary>
public class RevokedTokenMiddleware
{
    private readonly RequestDelegate _next;

    public RevokedTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Проверяет, был ли переданный JWT-токен аннулирован.
    /// </summary>
    /// <param name="context">Контекст текущего HTTP-запроса.</param>
    /// <param name="tokenService">Сервис для работы с токенами.</param>
    public async Task Invoke(HttpContext context, TokenService tokenService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var jti = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (jti != null && await tokenService.IsTokenInvalidAsync(jti))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Токен был аннулирован. Пожалуйста, выполните вход заново.");
                return;
            }
        }
        await _next(context);
    }
}
