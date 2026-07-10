namespace TechnicalAssessment.Domain;

public class OrderCsvParser : IRecordParser<Order>
{
    public Order? Parse(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
            return null;

        var parts = csvLine.Split(',');
        if (parts.Length < 6)
            return null;

        if (!CsvFieldParser.TryParseIntField(parts, 0, out var orderId))
            return null;

        if (!CsvFieldParser.TryParseIntField(parts, 1, out var customerId))
            return null;

        if (!CsvFieldParser.TryParseIntField(parts, 2, out var status))
            return null;

        if (!CsvFieldParser.TryParseDateTimeField(parts, 3, out var orderDate))
            return null;

        DateTime? requiredDate = null;
        if (CsvFieldParser.TryParseDateTimeField(parts, 4, out var rd))
            requiredDate = rd;

        DateTime? shippedDate = null;
        if (CsvFieldParser.TryParseDateTimeField(parts, 5, out var sd))
            shippedDate = sd;

        return new Order
        {
            OrderId = orderId,
            CustomerId = customerId,
            OrderStatus = status,
            OrderDate = orderDate,
            RequiredDate = requiredDate,
            ShippedDate = shippedDate
        };
    }
}
