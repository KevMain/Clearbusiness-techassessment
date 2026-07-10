namespace TechnicalAssessment.Domain;

public sealed class CustomerDiscountApplied
{
    public CustomerDiscountApplied(OrderItem item, Order order, Customer? customer, decimal additional)
    {
        Item = item;
        Order = order;
        Customer = customer;
        Additional = additional;
    }

    public OrderItem Item { get; }
    public Order Order { get; }
    public Customer? Customer { get; }
    public decimal Additional { get; }
}
