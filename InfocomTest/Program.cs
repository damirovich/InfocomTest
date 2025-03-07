using InfocomTest.Middleware;
using Serilog;
using AspNetCoreRateLimit;
using InfocomTest.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования через Serilog.
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Подключение сервисов и конфигурации.
builder.Services.AddCustomApiVersioning();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.RateLimiting();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAppServices();
builder.Services.AddControllers();

var app = builder.Build();

// Настройка среды и middleware.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Payment API v1"));
}

app.UseMiddleware<RevokedTokenMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();