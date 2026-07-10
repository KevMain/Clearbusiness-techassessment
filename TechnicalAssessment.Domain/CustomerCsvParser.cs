namespace TechnicalAssessment.Domain;

public class CustomerCsvParser : AbstractCsvParser<Customer>
{
    public override ParseResult<Customer> Parse(string csvLine)
    {
        if (!TryPrepare(csvLine, minColumns: 9, headerToken: "customer_id", out var parts, out var failure))
            return failure;

        if (!parts.TryGetInt(0, out var id))
            return ParseResult<Customer>.Failure("Invalid customer_id", csvLine);

        var firstName = parts.GetString(1) ?? string.Empty;
        var lastName = parts.GetString(2) ?? string.Empty;
        var phone = parts.GetString(3);
        var email = parts.GetString(4) ?? string.Empty;
        var street = parts.GetString(5) ?? string.Empty;
        var city = parts.GetString(6) ?? string.Empty;
        var state = parts.GetString(7) ?? string.Empty;
        var zip = parts.GetString(8) ?? string.Empty;

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
