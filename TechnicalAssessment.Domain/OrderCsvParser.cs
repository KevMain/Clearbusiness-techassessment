namespace TechnicalAssessment.Domain;

public class OrderCsvParser : AbstractCsvParser<Order>
{
    public override ParseResult<Order> Parse(string csvLine)
    {
        if (!TryPrepare(csvLine, minColumns: 6, headerToken: "order_id", out var parts, out var failure))
            return failure;

        if (!parts.TryGetInt(0, out var orderId))
            return ParseResult<Order>.Failure("Invalid order_id", csvLine);

        if (!parts.TryGetInt(1, out var customerId))
            return ParseResult<Order>.Failure("Invalid customer_id", csvLine);

        if (!parts.TryGetInt(2, out var status))
            return ParseResult<Order>.Failure("Invalid order_status", csvLine);

        if (!parts.TryGetDate(3, out var orderDate))
            return ParseResult<Order>.Failure("Invalid order_date", csvLine);

        DateTime? requiredDate = null;
        if (parts.TryGetDate(4, out var rd))
            requiredDate = rd;

        DateTime? shippedDate = null;
        if (parts.TryGetDate(5, out var sd))
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
