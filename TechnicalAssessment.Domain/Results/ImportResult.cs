using System.Collections.Generic;

namespace TechnicalAssessment.Domain;

public sealed class ImportResult<T>
{
    public ImportResult()
    {
        Successes = new List<T>();
        Failures = new List<ParseError>();
    }

    public List<T> Successes { get; }
    public List<ParseError> Failures { get; }

    public int TotalLines => Successes.Count + Failures.Count;
}
