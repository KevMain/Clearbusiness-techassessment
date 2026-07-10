using System.Collections.Generic;

namespace TechnicalAssessment.Domain;

public sealed class ImportReport
{
    public ImportReport()
    {
        Customers = new ImportResult<Customer>();
        Orders = new ImportResult<Order>();
        Items = new ImportResult<OrderItem>();
    }

    public ImportResult<Customer> Customers { get; }
    public ImportResult<Order> Orders { get; }
    public ImportResult<OrderItem> Items { get; }

    public List<ParseError> AllFailures
    {
        get
        {
            var list = new List<ParseError>();
            list.AddRange(Customers.Failures);
            list.AddRange(Orders.Failures);
            list.AddRange(Items.Failures);
            return list;
        }
    }

    // Compute order totals from parsed order items.
    // Total = Sum(list_price * (1 - discount)) per order_id
    public System.Collections.Generic.Dictionary<int, decimal> GetOrderTotals()
    {
        return Items.Successes
            .GroupBy(i => i.OrderId)
            .ToDictionary(g => g.Key, g => g.Sum(it => it.ListPrice * (1 - it.Discount)));
    }
}
