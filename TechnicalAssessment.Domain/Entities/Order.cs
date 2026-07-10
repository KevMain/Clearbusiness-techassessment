namespace TechnicalAssessment.Domain;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
}
