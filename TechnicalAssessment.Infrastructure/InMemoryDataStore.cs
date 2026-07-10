using System.Collections.Generic;
using System.IO;

namespace TechnicalAssessment.Infrastructure;

using TechnicalAssessment.Domain;

public interface IDataStore
{
    ImportReport Report { get; }
    void LoadFromFolder(string folder);
}

public sealed class InMemoryDataStore : IDataStore
{
    public InMemoryDataStore()
    {
        Report = new ImportReport();
    }

    public ImportReport Report { get; }

    public void LoadFromFolder(string folder)
    {
        if (!Directory.Exists(folder))
            return;

        var customersPath = Path.Combine(folder, "Customers.csv");
        var ordersPath = Path.Combine(folder, "Orders.csv");
        var itemsPath = Path.Combine(folder, "OrderItems.csv");

        IEnumerable<string> customerLines = File.Exists(customersPath) ? File.ReadLines(customersPath) : new string[0];
        IEnumerable<string> orderLines = File.Exists(ordersPath) ? File.ReadLines(ordersPath) : new string[0];
        IEnumerable<string> itemLines = File.Exists(itemsPath) ? File.ReadLines(itemsPath) : new string[0];

        var report = ImportOrchestrator.Import(
            customerLines,
            orderLines,
            itemLines,
            new CustomerCsvParser(),
            new OrderCsvParser(),
            new OrderItemCsvParser());

        // Replace Report contents
        Report.Customers.Successes.Clear();
        Report.Customers.Successes.AddRange(report.Customers.Successes);
        Report.Customers.Failures.Clear();
        Report.Customers.Failures.AddRange(report.Customers.Failures);

        Report.Orders.Successes.Clear();
        Report.Orders.Successes.AddRange(report.Orders.Successes);
        Report.Orders.Failures.Clear();
        Report.Orders.Failures.AddRange(report.Orders.Failures);

        Report.Items.Successes.Clear();
        Report.Items.Successes.AddRange(report.Items.Successes);
        Report.Items.Failures.Clear();
        Report.Items.Failures.AddRange(report.Items.Failures);
    }
}
