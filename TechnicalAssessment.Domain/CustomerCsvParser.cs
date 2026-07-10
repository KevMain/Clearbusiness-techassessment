namespace TechnicalAssessment.Domain;

public class CustomerCsvParser : IRecordParser<Customer>
{
    public Customer? Parse(string csvLine)
    {
        // Basic validation to ensure the line is not null or empty
        if (string.IsNullOrWhiteSpace(csvLine))
            return null;

        // Split the CSV line into parts. For the sample files we assume no quoted commas.
        var parts = csvLine.Split(',');
        if (parts.Length < 9)
            return null;

        // Treat header rows as ignorable
        var first = CsvFieldParser.GetField(parts, 0);
        if (string.Equals(first, "customer_id", StringComparison.OrdinalIgnoreCase))
            return null;

        if (!CsvFieldParser.TryParseIntField(parts, 0, out var id))
            return null;

        var firstName = CsvFieldParser.ParseStringField(parts, 1) ?? string.Empty;
        var lastName = CsvFieldParser.ParseStringField(parts, 2) ?? string.Empty;
        var phone = CsvFieldParser.ParseStringField(parts, 3);
        var email = CsvFieldParser.ParseStringField(parts, 4) ?? string.Empty;
        var street = CsvFieldParser.ParseStringField(parts, 5) ?? string.Empty;
        var city = CsvFieldParser.ParseStringField(parts, 6) ?? string.Empty;
        var state = CsvFieldParser.ParseStringField(parts, 7) ?? string.Empty;
        var zip = CsvFieldParser.ParseStringField(parts, 8) ?? string.Empty;

        return new Customer
        {
            CustomerId = id,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Email = email,
            Street = street,
            City = city,
            State = state,
            ZipCode = zip
        };
    }
}
