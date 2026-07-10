using System.Collections.Generic;
using System.Linq;

namespace TechnicalAssessment.Domain;

public static class ImportOrchestrator
{
    public static ImportReport Import(
        IEnumerable<string> customerLines,
        IEnumerable<string> orderLines,
        IEnumerable<string> itemLines,
        IRecordParser<Customer> customerParser,
        IRecordParser<Order> orderParser,
        IRecordParser<OrderItem> itemParser)
    {
        var report = new ImportReport();

        var customersResult = CsvImporter.ParseLines(customerLines, customerParser);
        report.Customers.Successes.AddRange(customersResult.Successes);
        report.Customers.Failures.AddRange(customersResult.Failures);

        var customersById = report.Customers.Successes.ToDictionary(c => c.CustomerId);

        var ordersResult = CsvImporter.ParseLines(orderLines, orderParser);
        
        // For each successful order, validate customer exists
        foreach (var order in ordersResult.Successes)
        {
            if (!customersById.ContainsKey(order.CustomerId))
            {
                var pe = new ParseError("MissingCustomer", $"Customer {order.CustomerId} not found") { Raw = null };
                report.Orders.Failures.Add(pe);
            }
            else
            {
                report.Orders.Successes.Add(order);
            }
        }
        report.Orders.Failures.AddRange(ordersResult.Failures);

        var ordersById = report.Orders.Successes.ToDictionary(o => o.OrderId);

        var itemsResult = CsvImporter.ParseLines(itemLines, itemParser);

        // For each successful item, validate order exists
        foreach (var orderItem in itemsResult.Successes)
        {
            if (!ordersById.ContainsKey(orderItem.OrderId))
            {
                var pe = new ParseError("MissingOrder", $"Order {orderItem.OrderId} not found") { Raw = null };
                report.Items.Failures.Add(pe);
            }
            else
            {
                report.Items.Successes.Add(orderItem);
            }
        }
        report.Items.Failures.AddRange(itemsResult.Failures);

        return report;
    }
}
