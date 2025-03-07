using InfocomTest.Data.EntityModel;
using InfocomTest.Data;
using InfocomTest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfocomTest.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    // Получает текущий баланс пользователя.
    public async Task<decimal> GetUserBalanceAsync(string userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Balance)
            .FirstOrDefaultAsync();
    }

    // Списывает указанную сумму с баланса пользователя.
    public async Task<bool> DeductBalanceAsync(string userId, decimal amount)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || user.Balance < amount)
            return false;

        user.Balance -= amount;
        await _context.SaveChangesAsync();
        return true;
    }

    // Добавляет запись о платеже в базу данных.
    public async Task AddPaymentAsync(PaymentEntity payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }
}