namespace TechnicalAssessment.Domain;

public sealed class DiscountEngine
{
    private readonly IReadOnlyList<IDiscountRule> _rules;

    public DiscountEngine(IEnumerable<IDiscountRule>? rules)
    {
        _rules = rules?.ToList() ?? new List<IDiscountRule>();
    }

    public decimal GetAdditionalDiscount(OrderItem item, Order order, Customer customer)
    {
        return _rules.Sum(r => r.GetAdditionalDiscount(item, order, customer));
    }
}
