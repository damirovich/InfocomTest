using InfocomTest.Data.EntityModel;
using InfocomTest.Repositories.Interfaces;
using System.Security.Claims;

namespace InfocomTest.Services;

public class UserService 

{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
         _userRepository = userRepository;
    }

    /// <summary>
    /// Получает текущего аутентифицированного пользователя.
    /// </summary>
    /// <param name="user">Объект <see cref="ClaimsPrincipal"/>, представляющий текущего пользователя.</param>
    public async Task<UserEntity?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await _userRepository.GetCurrentUserAsync(user);
    }
}
