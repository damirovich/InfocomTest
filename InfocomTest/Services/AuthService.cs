using InfocomTest.Data.EntityModel;
using InfocomTest.Models.DTOs;
using InfocomTest.Repositories.Interfaces;

namespace InfocomTest.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthService(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    /// <summary>
    /// Регистрирует нового пользователя.
    /// </summary>
    public async Task<BaseResponse<string>> RegisterAsync(RegisterDto dto)
    {
        // Проверяем, существует ли уже пользователь с таким именем
        if (await _userRepository.UserExistsAsync(dto.UserName))
            return new BaseResponse<string>(false, "Пользователь с таким именем уже существует.");

        // Создаем нового пользователя с начальным балансом
        var user = new UserEntity { UserName = dto.UserName, Balance = 8.00m };
        var result = await _userRepository.CreateUserAsync(user, dto.Password);

        // Проверяем, успешно ли создан пользователь
        if (!result.Succeeded)
            return new BaseResponse<string>(false, "Не удалось зарегистрировать пользователя.");

        return new BaseResponse<string>(true, "Регистрация прошла успешно.");
    }

    /// <summary>
    /// Аутентифицирует пользователя и выдает JWT-токен.
    /// </summary>
    public async Task<BaseResponse<string>> LoginAsync(LoginDto model)
    {
        // Проверяем, существует ли пользователь и правильный ли пароль
        var user = await _userRepository.GetByUsernameAsync(model.UserName);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, model.Password))
            return new BaseResponse<string>(false, "Неверное имя пользователя или пароль.");

        // Генерируем JWT-токен
        var token = _tokenService.GenerateJwtToken(user);
        return new BaseResponse<string>(true, "Вход выполнен успешно.", token);
    }

}
