using Microsoft.AspNetCore.Identity;

namespace InfocomTest.Data.EntityModel;

public class UserEntity : IdentityUser
{
    public decimal Balance { get; set; } = 8.00m;
}
