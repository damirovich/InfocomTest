using AspNetCoreRateLimit;

namespace InfocomTest.Common.Extensions;

//Настройки ограничения частоты запросов (Rate Limiting).
public static class RateLimitExtensions
{
    public static void RateLimiting(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1m", //Период
                    Limit = 5  // 5 запросов в минуту с одного IP
                }
            };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); // Хранилище правил ограничения.
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>(); // Хранилище счетчиков запросов.
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>(); // Конфигурация ограничений.
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Стратегия обработки запросов.
    }
}