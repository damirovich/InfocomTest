using InfocomTest.Data.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace InfocomTest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<RevokedTokenEntity> RevokedTokens { get; set; }
}
