namespace TechnicalAssessment.Domain;

public class CustomerCsvParser : IRecordParser<Customer>
{
    public ParseResult<Customer> Parse(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
            return ParseResult<Customer>.Failure("Empty or whitespace line", csvLine);

        var parts = csvLine.Split(',');
        if (parts.Length < 9)
            return ParseResult<Customer>.Failure("Too few columns", csvLine);

        var first = CsvFieldParser.GetField(parts, 0);
        if (string.Equals(first, "customer_id", StringComparison.OrdinalIgnoreCase))
            return ParseResult<Customer>.Failure("Header row", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 0, out var id))
            return ParseResult<Customer>.Failure("Invalid customer_id", csvLine);

        var firstName = CsvFieldParser.ParseStringField(parts, 1) ?? string.Empty;
        var lastName = CsvFieldParser.ParseStringField(parts, 2) ?? string.Empty;
        var phone = CsvFieldParser.ParseStringField(parts, 3);
        var email = CsvFieldParser.ParseStringField(parts, 4) ?? string.Empty;
        var street = CsvFieldParser.ParseStringField(parts, 5) ?? string.Empty;
        var city = CsvFieldParser.ParseStringField(parts, 6) ?? string.Empty;
        var state = CsvFieldParser.ParseStringField(parts, 7) ?? string.Empty;
        var zip = CsvFieldParser.ParseStringField(parts, 8) ?? string.Empty;

        var customer = new Customer
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

        return ParseResult<Customer>.Success(customer, csvLine);
    }
}
