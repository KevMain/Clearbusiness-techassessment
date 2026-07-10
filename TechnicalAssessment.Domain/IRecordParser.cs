namespace TechnicalAssessment.Domain;

public interface IRecordParser<T>
{
    /// <summary>
    /// Parse a single CSV line into the domain object. Return null if the line is invalid or should be ignored.
    /// </summary>
    T? Parse(string csvLine);
}
