namespace InfocomTest.Common.Extensions;
public static class ApiVersionExtensions
{
    // Метод-расширение для добавления настраиваемого версионирования API в IServiceCollection
    /// <summary>
    /// Добавляет кастомное версионирование API в IServiceCollection
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // Версия API по умолчанию
            options.DefaultApiVersion = new ApiVersion(2, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true; // Включает заголовки с версиями API
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(), // Версия через URL (например, /api/v2/)
                new HeaderApiVersionReader("x-api-version"), // Версия через заголовок
                new QueryStringApiVersionReader("api-version") // Версия через параметр запроса
            );
        })
        .AddApiExplorer(options =>
        {
            options.SubstituteApiVersionInUrl = true;
            options.GroupNameFormat = "'v'VVV"; // Формат групп (например, v1, v2)
            options.AssumeDefaultVersionWhenUnspecified = true;
        });
    }
}
