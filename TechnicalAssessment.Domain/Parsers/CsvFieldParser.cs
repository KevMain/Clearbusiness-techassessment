using System.Globalization;

namespace TechnicalAssessment.Domain;

public static class CsvFieldParser
{
    public static string? GetField(string[] parts, int index)
    {
        if (index < 0 || index >= parts.Length)
            return null;
        var v = parts[index]?.Trim();
        return string.IsNullOrEmpty(v) ? null : v;
    }

    public static bool TryParseIntField(string[] parts, int index, out int value)
    {
        value = default;
        var s = GetField(parts, index);
        if (s == null)
            return false;
        return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    public static bool TryParseDecimalField(string[] parts, int index, out decimal value)
    {
        value = default;
        var s = GetField(parts, index);
        if (s == null)
            return false;
        return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
    }

    public static bool TryParseDateTimeField(string[] parts, int index, out DateTime value)
    {
        value = default;
        var s = GetField(parts, index);
        if (s == null)
            return false;

        var formats = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "yyyy-M-d" }; // Check for common date formats
        if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            return true;

        return DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

    public static string? ParseStringField(string[] parts, int index)
    {
        return GetField(parts, index);
    }
}
