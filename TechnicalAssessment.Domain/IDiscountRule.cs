namespace TechnicalAssessment.Domain;

public interface IDiscountRule
{
    decimal GetAdditionalDiscount(OrderItem item, Order order, Customer customer);
}
