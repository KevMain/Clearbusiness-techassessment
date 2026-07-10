using System;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class OrderCsvParserTests
{
    [Theory]
    [MemberData(nameof(OrderTestData.ValidCases), MemberType = typeof(OrderTestData))]
    public void Parse_ValidCsvLine_ReturnsOrderWithMappedFields(string csv, int expectedOrderId, int expectedCustomerId, int expectedStatus, DateTime expectedOrderDate, DateTime? expectedRequiredDate, DateTime? expectedShipped)
    {
        var parser = OrderTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var order = result.Value!;
        Assert.Equal(expectedOrderId, order.OrderId);
        Assert.Equal(expectedCustomerId, order.CustomerId);
        Assert.Equal(expectedStatus, order.OrderStatus);
        Assert.Equal(expectedOrderDate.Date, order.OrderDate.Date);
        if (expectedRequiredDate.HasValue)
            Assert.Equal(expectedRequiredDate.Value.Date, order.RequiredDate?.Date);
        else
            Assert.Null(order.RequiredDate);

        if (expectedShipped.HasValue)
            Assert.Equal(expectedShipped.Value.Date, order.ShippedDate?.Date);
        else
            Assert.Null(order.ShippedDate);
    }

    [Theory]
    [MemberData(nameof(OrderTestData.NullCases), MemberType = typeof(OrderTestData))]
    public void Parse_InvalidOrHeader_ReturnsNull(string csv)
    {
        var parser = OrderTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.False(result.IsSuccess);
    }
}
