using InfocomTest.Data.EntityModel;
using InfocomTest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InfocomTest.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<UserEntity> _userManager;

    public UserRepository(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    // Получает данные пользователя по имени.
    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }
    // Проверяет, существует ли пользователь с указанным именем.
    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == username);
    }

    // Создает нового пользователя в системе.
    public async Task<IdentityResult> CreateUserAsync(UserEntity user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    // Получает текущего аутентифицированного пользователя.
    public async Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await _userManager.GetUserAsync(user);
    }

    // Проверяет, совпадает ли введенный пароль с паролем пользователя.
    public async Task<bool> CheckPasswordAsync(UserEntity user, string password) 
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

}