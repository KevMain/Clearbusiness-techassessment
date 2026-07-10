namespace TechnicalAssessment.Domain;

public sealed class StateDiscountRule : IDiscountRule
{
    private readonly string _state;
    private readonly decimal _discountFraction;

    public StateDiscountRule(string state, decimal discountFraction)
    {
        _state = state;
        _discountFraction = discountFraction;
    }

    public decimal GetAdditionalDiscount(OrderItem item, Order order, Customer customer)
    {
        if (customer == null)
            return 0m;

        if (string.Equals(customer.State?.Trim(), _state, StringComparison.OrdinalIgnoreCase))
            return _discountFraction;

        return 0m;
    }
}
