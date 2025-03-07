using InfocomTest.Data.EntityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InfocomTest.Repositories.Interfaces;

public interface IUserRepository 
{
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<bool> UserExistsAsync(string username);
    Task<IdentityResult> CreateUserAsync(UserEntity user, string password);
    Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal user);
    Task<bool> CheckPasswordAsync(UserEntity user, string password);
}