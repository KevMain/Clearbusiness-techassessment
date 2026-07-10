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

        Assert.NotNull(result);
        Assert.Equal(expectedOrderId, result!.OrderId);
        Assert.Equal(expectedCustomerId, result.CustomerId);
        Assert.Equal(expectedStatus, result.OrderStatus);
        Assert.Equal(expectedOrderDate.Date, result.OrderDate.Date);
        if (expectedRequiredDate.HasValue)
            Assert.Equal(expectedRequiredDate.Value.Date, result.RequiredDate?.Date);
        else
            Assert.Null(result.RequiredDate);

        if (expectedShipped.HasValue)
            Assert.Equal(expectedShipped.Value.Date, result.ShippedDate?.Date);
        else
            Assert.Null(result.ShippedDate);
    }

    [Theory]
    [MemberData(nameof(OrderTestData.NullCases), MemberType = typeof(OrderTestData))]
    public void Parse_InvalidOrHeader_ReturnsNull(string csv)
    {
        var parser = OrderTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.Null(result);
    }
}
