using InfocomTest.Data.EntityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InfocomTest.Repositories.Interfaces;

public interface IUserRepository 
{
    Task<UserEntity?> GetByUsernameAsync(string username); // Получает данные пользователя по имени.
    Task<bool> UserExistsAsync(string username); // Проверяет, существует ли пользователь с указанным именем.
    Task<IdentityResult> CreateUserAsync(UserEntity user, string password); // Создает нового пользователя в системе.
    Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal user);// Получает текущего аутентифицированного пользователя.
    Task<bool> CheckPasswordAsync(UserEntity user, string password); // Проверяет, совпадает ли введенный пароль с паролем пользователя.
}