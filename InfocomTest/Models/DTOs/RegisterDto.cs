using System.Text.Json.Serialization;

namespace InfocomTest.Models.DTOs;

/// <summary>
/// DTO для регистрации нового пользователя.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Имя пользователя для регистрации.
    /// </summary>
    [JsonPropertyName("UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// Пароль пользователя для регистрации.
    /// </summary>
    [JsonPropertyName("Password")]
    public string Password { get; set; }
}