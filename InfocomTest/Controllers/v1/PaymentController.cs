using InfocomTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfocomTest.Controllers.v1;


/// <summary>
/// Контроллер для управления платежами пользователей.
/// </summary>
[Route("api/v{version:apiVersion}/payment")]
[ApiController]
[Authorize]
[ApiVersion("1.0")]
public class PaymentController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PaymentService _paymentService;

    public PaymentController(UserService userService, PaymentService paymentService)
    {
        _userService = userService;
        _paymentService = paymentService;
    }

    /// <summary>
    /// Совершает платеж, списывая 1.1 USD с баланса пользователя.
    /// </summary>
    /// <returns>Обновленный баланс пользователя.</returns>
    /// <response code="200">Платеж успешно выполнен.</response>
    /// <response code="400">Недостаточно средств на балансе.</response>
    /// <response code="401">Пользователь не найден или не авторизован.</response>
    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment()
    {
        var user = await _userService.GetCurrentUserAsync(User);
        if (user == null)
            return Unauthorized(new BaseResponse<string>(false, "User not found"));

        var response = await _paymentService.MakePaymentAsync(user.Id, 1.1m);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Тестовый эндпоинт для проверки работы API.
    /// </summary>
    /// <returns>Сообщение о работоспособности API.</returns>
    /// <response code="200">API работает корректно.</response>
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new BaseResponse<string>(true, "API is working correctly"));
    }
}
