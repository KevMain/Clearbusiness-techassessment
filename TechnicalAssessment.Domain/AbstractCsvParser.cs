using System;

namespace TechnicalAssessment.Domain;

public abstract class AbstractCsvParser<T> : IRecordParser<T>
{
    public abstract ParseResult<T> Parse(string csvLine);

    protected bool TryPrepare(string csvLine, int minColumns, string headerToken, out string[] parts, out ParseResult<T> failure)
    {
        parts = Array.Empty<string>();
        failure = default!;

        if (string.IsNullOrWhiteSpace(csvLine))
        {
            failure = ParseResult<T>.Failure("Empty or whitespace line", csvLine);
            return false;
        }

        parts = csvLine.Split(',');
        if (parts.Length < minColumns)
        {
            failure = ParseResult<T>.Failure("Too few columns", csvLine);
            return false;
        }

        var first = CsvFieldParser.GetField(parts, 0);
        if (string.Equals(first, headerToken, StringComparison.OrdinalIgnoreCase))
        {
            var pe = new ParseError("header", "Header row") { Raw = csvLine };
            failure = ParseResult<T>.Failure(pe, csvLine);
            return false;
        }

        failure = default!;
        return true;
    }
}
