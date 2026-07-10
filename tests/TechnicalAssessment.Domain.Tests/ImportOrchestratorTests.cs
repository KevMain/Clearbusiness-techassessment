using System.Collections.Generic;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests;

public class ImportOrchestratorTests
{
    [Fact]
    public void Import_ReportsMissingCustomerAndOrderErrors()
    {
        var customerLines = new List<string>
        {
            CustomerTestData.Header,
            CustomerTestData.Debra
        };

        var orderLines = new List<string>
        {
            OrderTestData.Header,
            OrderTestData.OrderWithMissingCustomer // references customer 999
        };

        var itemLines = new List<string>
        {
            OrderItemTestData.Header,
            OrderItemTestData.ItemWithMissingOrder // references order 888
        };

        var report = ImportOrchestrator.Import(
            customerLines,
            orderLines,
            itemLines,
            CustomerTestData.CreateParser(),
            OrderTestData.CreateParser(),
            OrderItemTestData.CreateParser());

        // Customer import should have 1 success
        Assert.Single(report.Customers.Successes);

        // Orders: order referenced missing customer -> should be recorded as failure
        Assert.Empty(report.Orders.Successes);
        Assert.Contains(report.Orders.Failures, f => f.Code == "MissingCustomer");

        // Items: item references missing order -> failure
        Assert.Empty(report.Items.Successes);
        Assert.Contains(report.Items.Failures, f => f.Code == "MissingOrder");
    }
}
