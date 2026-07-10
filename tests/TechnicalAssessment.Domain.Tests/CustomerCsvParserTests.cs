using System;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class CustomerCsvParserTests
{
    [Theory]
    [MemberData(nameof(CustomerTestData.ValidCases), MemberType = typeof(CustomerTestData))]
    public void Parse_ValidCsvLine_ReturnsCustomerWithMappedFields(
        string csv,
        int expectedId,
        string expectedFirst,
        string expectedLast,
        string? expectedPhone,
        string expectedEmail,
        string expectedStreet,
        string expectedCity,
        string expectedState,
        string expectedZip)
    {
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var customer = result.Value!;
        Assert.Equal(expectedId, customer.CustomerId);
        Assert.Equal(expectedFirst, customer.FirstName);
        Assert.Equal(expectedLast, customer.LastName);
        if (string.IsNullOrEmpty(expectedPhone))
            Assert.True(string.IsNullOrEmpty(customer.Phone));
        else
            Assert.Equal(expectedPhone, customer.Phone);
        Assert.Equal(expectedEmail, customer.Email);
        Assert.Equal(expectedStreet, customer.Street);
        Assert.Equal(expectedCity, customer.City);
        Assert.Equal(expectedState, customer.State);
        Assert.Equal(expectedZip, customer.ZipCode);
    }

    [Theory]
    [MemberData(nameof(CustomerTestData.NullCases), MemberType = typeof(CustomerTestData))]
    public void Parse_NullOrInvalid_ReturnsNull(string csv)
    {
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.False(result.IsSuccess);
    }
}
