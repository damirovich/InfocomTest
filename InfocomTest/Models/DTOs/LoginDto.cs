using System.Text.Json.Serialization;

namespace InfocomTest.Models.DTOs;

/// <summary>
/// DTO для аутентификации пользователя.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [JsonPropertyName("UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [JsonPropertyName("Password")]
    public string Password { get; set; }
}