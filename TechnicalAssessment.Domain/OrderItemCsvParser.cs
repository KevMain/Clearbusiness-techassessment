namespace TechnicalAssessment.Domain;

public class OrderItemCsvParser : AbstractCsvParser<OrderItem>
{
    // Expected header: order_id,item_id,list_price,discount
    public override ParseResult<OrderItem> Parse(string csvLine)
    {
        if (!TryPrepare(csvLine, minColumns: 4, headerToken: "order_id", out var parts, out var failure))
            return failure;

        if (!parts.TryGetInt(0, out var orderId))
            return ParseResult<OrderItem>.Failure("Invalid order_id", csvLine);

        if (!parts.TryGetInt(1, out var itemId))
            return ParseResult<OrderItem>.Failure("Invalid item_id", csvLine);

        if (!parts.TryGetDecimal(2, out var listPrice))
            return ParseResult<OrderItem>.Failure("Invalid list_price", csvLine);

        if (!parts.TryGetDecimal(3, out var discount))
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
