namespace TechnicalAssessment.Domain;

public sealed class ParseError
{
    public ParseError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; init; }
    public string Message { get; init; }
    public string? Field { get; set; }
    public string? Raw { get; set; }
    public int? LineNumber { get; set; }

    public override string ToString() => $"{Code}: {Message}";
}
