namespace InfocomTest.Data.EntityModel;

public class PaymentEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Amount { get; set; }
}
