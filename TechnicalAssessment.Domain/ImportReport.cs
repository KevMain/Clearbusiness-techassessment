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
    public Dictionary<int, decimal> GetOrderTotals()
    {
        return GetOrderTotals(null);
    }

    public Dictionary<int, decimal> GetOrderTotals(DiscountEngine? discountEngine)
    {
        var ordersById = Orders.Successes.ToDictionary(o => o.OrderId);
        var customersById = Customers.Successes.ToDictionary(c => c.CustomerId);

        return Items.Successes
            .GroupBy(i => i.OrderId)
            .ToDictionary(g => g.Key, g => g.Sum(it =>
            {
                var baseDiscount = it.Discount;

                ordersById.TryGetValue(it.OrderId, out var order);
                Customer? customer = null;
                if (order != null)
                    customersById.TryGetValue(order.CustomerId, out customer);

                var additional = discountEngine?.GetAdditionalDiscount(it, order!, customer) ?? 0m;
                
                var effective = baseDiscount + additional;
                
                return it.ListPrice * (1 - effective);
            }));
    }

    public Dictionary<int, decimal> GetCustomerTotals()
    {
        var orderTotals = GetOrderTotals();
        
        var totals = new Dictionary<int, decimal>();

        foreach (var order in Orders.Successes)
        {
            var amount = orderTotals.TryGetValue(order.OrderId, out var v) ? v : 0m;

            if (totals.ContainsKey(order.CustomerId))
                totals[order.CustomerId] += amount;
            else
                totals[order.CustomerId] = amount;
        }

        foreach (var customer in Customers.Successes)
        {
            if (!totals.ContainsKey(customer.CustomerId))
                totals[customer.CustomerId] = 0m;
        }
        

        return totals;
    }
}
