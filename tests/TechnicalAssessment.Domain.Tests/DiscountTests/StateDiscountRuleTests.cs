using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests.DiscountTests;

public class StateDiscountRuleTests
{
    [Fact]
    public void StateDiscountApplies_WhenCustomerInState()
    {
        var rule = new StateDiscountRule("CA", 0.05m);

        var customer = new Customer { CustomerId = 1, State = "CA" };
        var order = new Order { OrderId = 10, CustomerId = 1 };
        var item = new OrderItem { OrderId = 10, ItemId = 1, ListPrice = 100m, Discount = 0m };

        var add = rule.GetAdditionalDiscount(item, order, customer);

        Assert.Equal(0.05m, add);
    }

    [Fact]
    public void StateDiscountDoesNotApply_WhenCustomerDifferentState()
    {
        var rule = new StateDiscountRule("CA", 0.05m);

        var customer = new Customer { CustomerId = 1, State = "NY" };
        var order = new Order { OrderId = 10, CustomerId = 1 };
        var item = new OrderItem { OrderId = 10, ItemId = 1, ListPrice = 100m, Discount = 0m };

        var add = rule.GetAdditionalDiscount(item, order, customer);

        Assert.Equal(0m, add);
    }
}
