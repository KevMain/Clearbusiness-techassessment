namespace TechnicalAssessment.Domain;

public class OrderItemCsvParser : IRecordParser<OrderItem>
{
    public ParseResult<OrderItem> Parse(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
            return ParseResult<OrderItem>.Failure("Empty or whitespace line", csvLine);

        var parts = csvLine.Split(',');
        if (parts.Length < 4)
            return ParseResult<OrderItem>.Failure("Too few columns", csvLine);

        var first = CsvFieldParser.GetField(parts, 0);
        if (string.Equals(first, "order_id", StringComparison.OrdinalIgnoreCase))
            return ParseResult<OrderItem>.Failure("Header row", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 0, out var orderId))
            return ParseResult<OrderItem>.Failure("Invalid order_id", csvLine);

        if (!CsvFieldParser.TryParseIntField(parts, 1, out var itemId))
            return ParseResult<OrderItem>.Failure("Invalid item_id", csvLine);

        if (!CsvFieldParser.TryParseDecimalField(parts, 2, out var listPrice))
            return ParseResult<OrderItem>.Failure("Invalid list_price", csvLine);

        if (!CsvFieldParser.TryParseDecimalField(parts, 3, out var discount))
            return ParseResult<OrderItem>.Failure("Invalid discount", csvLine);

        var item = new OrderItem
        {
            OrderId = orderId,
            ItemId = itemId,
            ListPrice = listPrice,
            Discount = discount
        };

        return ParseResult<OrderItem>.Success(item, csvLine);
    }
}
