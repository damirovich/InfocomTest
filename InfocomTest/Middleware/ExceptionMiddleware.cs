using InfocomTest.Common;
using System.Net;

namespace InfocomTest.Middleware;

/// <summary>
/// Глобальный middleware для обработки исключений в приложении.
/// Перехватывает все необработанные ошибки и возвращает корректный JSON-ответ.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Метод для обработки входящих HTTP-запросов и перехвата ошибок.
    /// </summary>
    /// <param name="context">Контекст текущего HTTP-запроса.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<string>(false, "Внутренняя ошибка сервера", ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
