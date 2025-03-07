namespace InfocomTest.Repositories.Interfaces;

public interface ITokenRepository 
{
    Task<bool> IsTokenRevokedAsync(string jti); // Проверяет, был ли токен аннулирован.
    Task InvalidateTokenAsync(string jti, DateTime expiration); // Аннулирует токен, добавляя его в список отозванных.
}