namespace InfocomTest.Data.EntityModel;

public class RevokedTokenEntity
{
    public int Id { get; set; }
    public string Jti { get; set; }
    public DateTime Expiration { get; set; }
}
