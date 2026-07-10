using System;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class CsvFieldParserTests
{
    [Fact]
    public void GetField_ReturnsTrimmedValue_WhenFieldHasWhitespace()
    {
        var parts = "  abc  ,def".Split(',');

        var v = CsvFieldParser.GetField(parts, 0);

        Assert.Equal("abc", v);
    }

    [Fact]
    public void GetField_ReturnsNull_WhenIndexOutOfRange()
    {
        var parts = "a,b".Split(',');

        var v = CsvFieldParser.GetField(parts, 5);

        Assert.Null(v);
    }

    [Fact]
    public void GetField_ReturnsNull_WhenFieldEmptyAfterTrim()
    {
        var parts = " ,b".Split(',');

        var v = CsvFieldParser.GetField(parts, 0);

        Assert.Null(v);
    }

    [Fact]
    public void TryParseIntField_ReturnsTrueAndValue_OnValidInteger()
    {
        var parts = "42,foo".Split(',');

        var ok = CsvFieldParser.TryParseIntField(parts, 0, out var val);

        Assert.True(ok);
        Assert.Equal(42, val);
    }

    [Fact]
    public void TryParseIntField_ReturnsFalse_OnNonIntegerOrMissing()
    {
        var parts = "abc,foo".Split(',');

        var ok = CsvFieldParser.TryParseIntField(parts, 0, out var val);

        Assert.False(ok);

        var parts2 = "".Split(',');
        var ok2 = CsvFieldParser.TryParseIntField(parts2, 0, out _);
        Assert.False(ok2);
    }

    [Fact]
    public void TryParseDateTimeField_ReturnsTrue_OnIsoDate()
    {
        var parts = "2023-01-15,other".Split(',');

        var ok = CsvFieldParser.TryParseDateTimeField(parts, 0, out var dt);

        Assert.True(ok);
        Assert.Equal(new DateTime(2023, 1, 15), dt.Date);
    }

    [Fact]
    public void TryParseDateTimeField_ReturnsTrue_OnDdMmYyyy()
    {
        var parts = "16/02/2016,other".Split(',');

        var ok = CsvFieldParser.TryParseDateTimeField(parts, 0, out var dt);

        Assert.True(ok);
        Assert.Equal(new DateTime(2016, 2, 16), dt.Date);
    }

    [Fact]
    public void TryParseDateTimeField_ReturnsFalse_OnInvalidDate()
    {
        var parts = "notadate,other".Split(',');

        var ok = CsvFieldParser.TryParseDateTimeField(parts, 0, out _);

        Assert.False(ok);
    }

    [Fact]
    public void TryParseDecimalField_ReturnsTrueAndValue_OnValidDecimal()
    {
        var parts = "549.99,other".Split(',');

        var ok = CsvFieldParser.TryParseDecimalField(parts, 0, out var val);

        Assert.True(ok);
        Assert.Equal(549.99m, val);
    }

    [Fact]
    public void TryParseDecimalField_ReturnsFalse_OnInvalidDecimal()
    {
        var parts = "notanumber,other".Split(',');

        var ok = CsvFieldParser.TryParseDecimalField(parts, 0, out _);

        Assert.False(ok);
    }

    [Fact]
    public void ParseStringField_BehavesLikeGetField()
    {
        var parts = "  hello  ,x".Split(',');

        var s1 = CsvFieldParser.ParseStringField(parts, 0);
        var s2 = CsvFieldParser.GetField(parts, 0);

        Assert.Equal(s2, s1);
    }
}
