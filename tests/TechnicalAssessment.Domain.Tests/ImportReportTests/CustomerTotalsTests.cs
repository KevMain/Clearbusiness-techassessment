using System.Collections.Generic;
using TechnicalAssessment.Domain;
using Xunit;

namespace TechnicalAssessment.Domain.Tests.ImportReportTests;

public class CustomerTotalsTests
{
    [Fact]
    public void GetCustomerTotals_AggregatesOrderTotalsPerCustomer()
    {
        // Arrange: simple in-memory CSV lines
        var customers = new List<string>
        {
            CustomerTestData.Header,
            "1,One,Customer,,one@example.com,Addr,City,ST,00001",
            "2,Two,Customer,,two@example.com,Addr,City,ST,00002"
        };

        var orders = new List<string>
        {
            OrderTestData.Header,
            // orderId,customerId,...
            "10,1,1,01/01/2020,01/01/2020,",
            "20,2,1,02/01/2020,02/01/2020,"
        };

        var items = new List<string>
        {
            OrderItemTestData.Header,
            // order 10 items for customer 1
            "10,1,100.00,0.10",
            "10,2,50.00,0.00",
            // order 20 items for customer 2
            "20,1,200.00,0.20"
        };

        // Act
        var report = ImportOrchestrator.Import(
            customers,
            orders,
            items,
            new CustomerCsvParser(),
            new OrderCsvParser(),
            new OrderItemCsvParser());

        var totals = report.GetCustomerTotals();

        // Assert
        Assert.Equal(2, totals.Count);

        // customer 1: (100 * 0.9) + (50 * 1.0) = 90 + 50 = 140
        Assert.True(totals.ContainsKey(1));
        Assert.Equal(140.00m, totals[1]);

        // customer 2: (200 * 0.8) = 160
        Assert.True(totals.ContainsKey(2));
        Assert.Equal(160.00m, totals[2]);
    }
}
