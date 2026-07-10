namespace TechnicalAssessment.Domain;

public class OrderCsvParser : IRecordParser<Order>
{
    public ParseResult<Order> Parse(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
            return ParseResult<Order>.Failure("Empty or whitespace line", csvLine);

        var parts = csvLine.Split(',');
        if (parts.Length < 6)
            return ParseResult<Order>.Failure("Too few columns", csvLine);

        var first = CsvFieldParser.GetField(parts, 0);
        if (string.Equals(first, "order_id", StringComparison.OrdinalIgnoreCase))
            return ParseResult<Order>.Failure("Header row", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 0, out var orderId))
            return ParseResult<Order>.Failure("Invalid order_id", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 1, out var customerId))
            return ParseResult<Order>.Failure("Invalid customer_id", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 2, out var status))
            return ParseResult<Order>.Failure("Invalid order_status", csvLine);

        if (!CsvFieldParser.TryParseDateTimeField(parts, 3, out var orderDate))
            return ParseResult<Order>.Failure("Invalid order_date", csvLine);

        DateTime? requiredDate = null;
        if (CsvFieldParser.TryParseDateTimeField(parts, 4, out var rd))
            requiredDate = rd;

        DateTime? shippedDate = null;
        if (CsvFieldParser.TryParseDateTimeField(parts, 5, out var sd))
            shippedDate = sd;

        var order = new Order
        {
            OrderId = orderId,
            CustomerId = customerId,
            OrderStatus = status,
            OrderDate = orderDate,
            RequiredDate = requiredDate,
            ShippedDate = shippedDate
        };

        return ParseResult<Order>.Success(order, csvLine);
    }
}
