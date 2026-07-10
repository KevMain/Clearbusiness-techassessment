using System;

namespace TechnicalAssessment.Domain;

public static class CsvPartsExtensions
{
    public static string? GetString(this string[] parts, int index) => CsvFieldParser.GetField(parts, index);

    public static bool TryGetInt(this string[] parts, int index, out int value) => CsvFieldParser.TryParseIntField(parts, index, out value);

    public static bool TryGetDecimal(this string[] parts, int index, out decimal value) => CsvFieldParser.TryParseDecimalField(parts, index, out value);

    public static bool TryGetDate(this string[] parts, int index, out DateTime value) => CsvFieldParser.TryParseDateTimeField(parts, index, out value);
}
