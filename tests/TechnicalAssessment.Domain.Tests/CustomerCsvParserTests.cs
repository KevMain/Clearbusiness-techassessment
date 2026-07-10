using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class CustomerCsvParserTests
{
    [Fact]
    public void Parse_HeaderRow_IsIgnored()
    {
        var header = CustomerTestData.Header;
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(header);

        Assert.Null(result);
    }

    [Fact]
    public void Parse_ValidCsvLine_ReturnsCustomerWithMappedFields()
    {
        // Arrange
        var csv = CustomerTestData.Debra;
        var customerCsvParser = CustomerTestData.CreateParser();

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

    [Fact]
    public void Parse_InvalidCsvLine_ReturnsNull()
    {
        var csv = CustomerTestData.Invalid;
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.Null(result);
    }

    [Fact]
    public void Parse_CsvLine_TrimsWhitespace()
    {
        var csv = CustomerTestData.Kasha;
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.NotNull(result);
        Assert.Equal(2, result!.CustomerId);
        Assert.Equal("Kasha", result.FirstName);
        Assert.Equal("Todd", result.LastName);
        Assert.Equal("kasha.todd@yahoo.com", result.Email);
        Assert.Equal("910 Vine Street", result.Street);
        Assert.Equal("Campbell", result.City);
        Assert.Equal("CA", result.State);
        Assert.Equal("95008", result.ZipCode);
    }

    [Fact]
    public void Parse_CsvLine_WithPhone_ReturnsPhone()
    {
        var csv = CustomerTestData.Charolette;
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.NotNull(result);
        Assert.Equal(5, result!.CustomerId);
        Assert.Equal("(916) 381-6003", result.Phone);
    }

    [Fact]
    public void Parse_CsvLine_MissingColumns_ReturnsNull()
    {
        var csv = CustomerTestData.TooFew; // too few columns
        var parser = CustomerTestData.CreateParser();

        var result = parser.Parse(csv);

        Assert.Null(result);
    }
}
