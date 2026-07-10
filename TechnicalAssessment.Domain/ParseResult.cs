using System.Collections.Generic;

namespace TechnicalAssessment.Domain;

public sealed class ParseResult<T>
{
    private ParseResult(T? value, bool isSuccess, IEnumerable<string>? errors, string? raw)
    {
        Value = value;
        IsSuccess = isSuccess;
        Errors = errors is null ? new List<string>() : new List<string>(errors);
        Raw = raw;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyList<string> Errors { get; }
    public string? Raw { get; }

    public static ParseResult<T> Success(T value, string? raw = null) => new ParseResult<T>(value, true, null, raw);

    public static ParseResult<T> Failure(IEnumerable<string> errors, string? raw = null) => new ParseResult<T>(default, false, errors, raw);

    public static ParseResult<T> Failure(string error, string? raw = null) => new ParseResult<T>(default, false, new[] { error }, raw);
}
