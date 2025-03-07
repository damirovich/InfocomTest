using InfocomTest.Models.DTOs;
using InfocomTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Serilog;

namespace InfocomTest.Controllers.v1;

/// <summary>
/// Контроллер для управления аутентификацией пользователей.
/// </summary>
[Route("api/v{version:apiVersion}/auth")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public AuthController(AuthService authService, TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Регистрирует нового пользователя.
    /// </summary>
    /// <param name="dto">Данные для регистрации.</param>
    /// <returns>Ответ с результатом регистрации.</returns>
    /// <response code="200">Регистрация успешна.</response>
    /// <response code="400">Ошибка валидации или пользователь уже существует.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        Log.Information("Попытка регистрации пользователя: {UserName}", dto.UserName);

        var response = await _authService.RegisterAsync(dto);

        if (response.Success)
        {
            Log.Information("Пользователь {UserName} успешно зарегистрирован.", dto.UserName);
            return Ok(response);
        }

        Log.Warning("Ошибка регистрации пользователя {UserName}: {ErrorMessage}", dto.UserName, response.Message);
        return BadRequest(response);
    }

    /// <summary>
    /// Аутентифицирует пользователя и выдает JWT-токен.
    /// </summary>
    /// <param name="model">Данные для входа.</param>
    /// <returns>JWT-токен при успешной аутентификации.</returns>
    /// <response code="200">Вход успешен, возвращает токен.</response>
    /// <response code="401">Неверный логин или пароль.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        Log.Information("Попытка входа пользователя: {UserName}", model.UserName);

        var response = await _authService.LoginAsync(model);

        if (response.Success)
        {
            Log.Information("Пользователь {UserName} успешно вошел в систему.", model.UserName);
            return Ok(response);
        }

        Log.Warning("Ошибка входа для пользователя {UserName}: Неверные учетные данные.", model.UserName);
        return Unauthorized(response);
    }

    /// <summary>
    /// Выход из системы, аннулирует токен.
    /// </summary>
    /// <returns>Результат выхода.</returns>
    /// <response code="200">Выход успешен.</response>
    /// <response code="400">Некорректный токен.</response>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            Log.Warning("Попытка выхода с некорректным токеном.");
            return BadRequest(new BaseResponse<string>(false, "Invalid token"));
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var jti = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

        if (jti == null)
        {
            Log.Warning("Попытка выхода с некорректным JWT токеном.");
            return BadRequest(new BaseResponse<string>(false, "Invalid token"));
        }

        await _tokenService.InvalidateTokenAsync(jti, jwtToken.ValidTo);
        Log.Information("Пользователь успешно вышел из системы и токен аннулирован.");

        return Ok(new BaseResponse<string>(true, "Logged out successfully"));
    }
}
