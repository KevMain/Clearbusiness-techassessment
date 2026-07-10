using System.Collections.Generic;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class CsvImporterTests
{
    [Fact]
    public void ParseLines_ParsesValidAndCollectsFailures()
    {
        var lines = new List<string>
        {
            CustomerTestData.Header,
            CustomerTestData.Debra,
            CustomerTestData.Invalid,
        };

        var parser = CustomerTestData.CreateParser();

        var result = CsvImporter.ParseLines(lines, parser);

        Assert.Equal(1, result.Successes.Count);
        Assert.Equal(1, result.Failures.Count);

        var success = result.Successes[0];
        Assert.Equal(1, success.CustomerId);

        var failure = result.Failures[0];
        Assert.Equal(3, failure.LineNumber);
        Assert.Equal(CustomerTestData.Invalid, failure.Raw);
    }
}
