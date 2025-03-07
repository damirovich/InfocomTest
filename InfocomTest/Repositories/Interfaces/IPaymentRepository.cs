using InfocomTest.Data.EntityModel;

namespace InfocomTest.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<decimal> GetUserBalanceAsync(string userId);  // Получает текущий баланс пользователя.
    Task<bool> DeductBalanceAsync(string userId, decimal amount); // Списывает указанную сумму с баланса пользователя.
    Task AddPaymentAsync(PaymentEntity payment); // Добавляет запись о платеже в базу данных.
}
