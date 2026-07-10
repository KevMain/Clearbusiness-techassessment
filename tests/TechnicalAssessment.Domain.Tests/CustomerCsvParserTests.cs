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

        Assert.NotNull(result);
        Assert.Equal(expectedId, result!.CustomerId);
        Assert.Equal(expectedFirst, result.FirstName);
        Assert.Equal(expectedLast, result.LastName);
        if (string.IsNullOrEmpty(expectedPhone))
            Assert.True(string.IsNullOrEmpty(result.Phone));
        else
            Assert.Equal(expectedPhone, result.Phone);
        Assert.Equal(expectedEmail, result.Email);
        Assert.Equal(expectedStreet, result.Street);
        Assert.Equal(expectedCity, result.City);
        Assert.Equal(expectedState, result.State);
        Assert.Equal(expectedZip, result.ZipCode);
    }

    [Theory]
    [MemberData(nameof(CustomerTestData.NullCases), MemberType = typeof(CustomerTestData))]
    public void Parse_NullOrInvalid_ReturnsNull(string csv)
    {
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.Null(result);
    }
}
