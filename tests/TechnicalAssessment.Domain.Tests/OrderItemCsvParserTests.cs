using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class OrderItemCsvParserTests
{
    [Theory]
    [MemberData(nameof(OrderItemTestData.ValidCases), MemberType = typeof(OrderItemTestData))]
    public void Parse_ValidCsvLine_ReturnsOrderItem(string csv, int expectedOrderId, int expectedItemId, decimal expectedPrice, decimal expectedDiscount)
    {
        var parser = OrderItemTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var item = result.Value!;
        Assert.Equal(expectedOrderId, item.OrderId);
        Assert.Equal(expectedItemId, item.ItemId);
        Assert.Equal(expectedPrice, item.ListPrice);
        Assert.Equal(expectedDiscount, item.Discount);
    }

    [Theory]
    [MemberData(nameof(OrderItemTestData.NullCases), MemberType = typeof(OrderItemTestData))]
    public void Parse_InvalidOrHeader_ReturnsFailure(string csv)
    {
        var parser = OrderItemTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.False(result.IsSuccess);
    }
}
