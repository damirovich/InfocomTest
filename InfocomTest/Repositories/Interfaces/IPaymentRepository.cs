using InfocomTest.Data.EntityModel;

namespace InfocomTest.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<decimal> GetUserBalanceAsync(string userId);
    Task<bool> DeductBalanceAsync(string userId, decimal amount);
    Task AddPaymentAsync(PaymentEntity payment);
}
