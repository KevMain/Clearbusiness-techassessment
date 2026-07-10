using System;
using System.IO;
using System.Linq;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Integration.Tests;

public class ImportIntegrationTests
{
    [Fact]
    public void Import_WithSampleCsvFiles_ParsesAndValidates()
    {
        var baseDir = AppContext.BaseDirectory;
        var dataDir = Path.Combine(baseDir, "TestData");

        var customersFile = Path.Combine(dataDir, "Customers.csv");
        var ordersFile = Path.Combine(dataDir, "Orders.csv");
        var itemsFile = Path.Combine(dataDir, "OrderItems.csv");

        Assert.True(File.Exists(customersFile), $"Missing file: {customersFile}");
        Assert.True(File.Exists(ordersFile), $"Missing file: {ordersFile}");
        Assert.True(File.Exists(itemsFile), $"Missing file: {itemsFile}");

        var customers = File.ReadAllLines(customersFile);
        var orders = File.ReadAllLines(ordersFile);
        var items = File.ReadAllLines(itemsFile);

        var report = ImportOrchestrator.Import(
            customers,
            orders,
            items,
            new CustomerCsvParser(),
            new OrderCsvParser(),
            new OrderItemCsvParser());

        // Basic sanity checks: at least one customer and no parser crashes
        Assert.True(report.Customers.Successes.Count >= 1, "Expected at least one parsed customer");

        Assert.NotNull(report);
    }
}
