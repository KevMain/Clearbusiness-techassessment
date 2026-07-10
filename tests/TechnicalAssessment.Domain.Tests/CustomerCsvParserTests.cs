using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class CustomerCsvParserTests
{
    [Fact]
    public void Parse_ValidCsvLine_ReturnsCustomerWithMappedFields()
    {
        // Arrange
        var csv = "1,Debra,Burks,,debra.burks@yahoo.com,9273 Thorne Ave. ,Orchard Park,NY,14127";
        var customerCsvParser = new CustomerCsvParser();

        // Act
        var result = customerCsvParser.Parse(csv);

        // Assert - parser should map fields to the new Customer shape
        Assert.NotNull(result);
        Assert.Equal(1, result!.CustomerId);
        Assert.Equal("Debra", result.FirstName);
        Assert.Equal("Burks", result.LastName);
        Assert.True(string.IsNullOrEmpty(result.Phone));
        Assert.Equal("debra.burks@yahoo.com", result.Email);
        Assert.Equal("9273 Thorne Ave.", result.Street);
        Assert.Equal("Orchard Park", result.City);
        Assert.Equal("NY", result.State);
        Assert.Equal("14127", result.ZipCode);
    }
}
