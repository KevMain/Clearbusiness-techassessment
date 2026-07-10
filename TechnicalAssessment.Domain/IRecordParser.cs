namespace TechnicalAssessment.Domain;

public interface IRecordParser<T>
{
    /// <summary>
    /// Parse a single CSV line into a ParseResult. The result captures success/failure and any errors.
    /// </summary>
    ParseResult<T> Parse(string csvLine);
}
