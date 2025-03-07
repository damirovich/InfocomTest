using InfocomTest.Data.EntityModel;
using InfocomTest.Repositories.Interfaces;

namespace InfocomTest.Services;

public class PaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    /// <summary>
    /// Совершает платеж, списывая указанную сумму с баланса пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="amount">Сумма платежа.</param>
    /// <returns>Обновленный баланс пользователя.</returns>
    public async Task<BaseResponse<decimal>> MakePaymentAsync(string userId, decimal amount)
    {
        // Получаем текущий баланс пользователя
        var currentBalance = await _paymentRepository.GetUserBalanceAsync(userId);
        if (currentBalance < amount)
            return new BaseResponse<decimal>(false, "Недостаточно средств на балансе.");

        // Используем транзакцию для предотвращения частичного списания средств
        using (var transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled))
        {
            // Пытаемся списать деньги
            var success = await _paymentRepository.DeductBalanceAsync(userId, amount);
            if (!success)
                return new BaseResponse<decimal>(false, "Недостаточно средств на балансе.");

            // Добавляем запись о платеже
            await _paymentRepository.AddPaymentAsync(new PaymentEntity
            {
                UserId = userId,
                Amount = amount,
                Timestamp = DateTime.UtcNow
            });

            // Завершаем транзакцию
            transaction.Complete();
        }

        var newBalance = await _paymentRepository.GetUserBalanceAsync(userId);
        return new BaseResponse<decimal>(true, "Payment successful", newBalance);
    }
}
