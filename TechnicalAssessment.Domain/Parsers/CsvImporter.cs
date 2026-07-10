using System.Collections.Generic;

namespace TechnicalAssessment.Domain;

public static class CsvImporter
{
    public static ImportResult<T> ParseLines<T>(IEnumerable<string> lines, IRecordParser<T> parser)
    {
        var result = new ImportResult<T>();
        int lineNo = 0;
        foreach (var line in lines)
        {
            lineNo++;
            var parse = parser.Parse(line);
            if (parse.IsSuccess)
            {
                result.Successes.Add(parse.Value!);
            }
            else
            {
                if (parse.Errors != null && parse.Errors.Count > 0)
                {
                    foreach (var e in parse.Errors)
                    {
                        if (e.LineNumber == null)
                            e.LineNumber = lineNo;
                        if (string.IsNullOrEmpty(e.Raw))
                            e.Raw = line;
                        if (string.Equals(e.Code, "header", System.StringComparison.OrdinalIgnoreCase))
                            continue;
                        result.Failures.Add(e);
                    }
                }
                else
                {
                    var err = new ParseError("parse_error", "Unknown parse failure") { LineNumber = lineNo, Raw = line };
                    result.Failures.Add(err);
                }
            }
        }

        return result;
    }
}
