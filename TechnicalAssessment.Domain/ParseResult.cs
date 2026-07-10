using System.Collections.Generic;

namespace TechnicalAssessment.Domain;

public sealed class ParseResult<T>
{
    private ParseResult(T? value, bool isSuccess, IEnumerable<ParseError>? errors, string? raw)
    {
        Value = value;
        IsSuccess = isSuccess;
        Errors = errors is null ? new List<ParseError>() : new List<ParseError>(errors);
        Raw = raw;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyList<ParseError> Errors { get; }
    public string? Raw { get; }

    public static ParseResult<T> Success(T value, string? raw = null) => new ParseResult<T>(value, true, null, raw);

    public static ParseResult<T> Failure(IEnumerable<ParseError> errors, string? raw = null) => new ParseResult<T>(default, false, errors, raw);

    public static ParseResult<T> Failure(ParseError error, string? raw = null) => new ParseResult<T>(default, false, new[] { error }, raw);

    public static ParseResult<T> Failure(string message, string? raw = null, string? code = null, string? field = null, int? lineNumber = null)
    {
        var err = new ParseError(code ?? "parse_error", message)
        {
            Field = field,
            Raw = raw,
            LineNumber = lineNumber
        };
        return Failure(err, raw);
    }
}
