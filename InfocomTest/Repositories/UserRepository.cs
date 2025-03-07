using InfocomTest.Data.EntityModel;
using InfocomTest.Data;
using InfocomTest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InfocomTest.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;


    public UserRepository(AppDbContext context, UserManager<UserEntity> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<IdentityResult> CreateUserAsync(UserEntity user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
    public async Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await _userManager.GetUserAsync(user);
    }
    public async Task<bool> CheckPasswordAsync(UserEntity user, string password) 
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

}