using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssessment.Domain;
using TechnicalAssessment.Infrastructure;

namespace TechnicalAssessment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public CustomersController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var customers = _dataStore.Report.Customers.Successes;
        var customerTotals = _dataStore.Report.GetCustomerTotals();

        // Count orders per customer
        var orderCounts = _dataStore.Report.Orders.Successes
            .GroupBy(o => o.CustomerId)
            .ToDictionary(g => g.Key, g => g.Count());

        var customersWithStats = customers.Select(c => new
        {
            c.CustomerId,
            c.FirstName,
            c.LastName,
            c.State,
            c.Email,
            OrderCount = orderCounts.TryGetValue(c.CustomerId, out var count) ? count : 0,
            Total = customerTotals.TryGetValue(c.CustomerId, out var total) ? total : 0m
        }).ToList();

        return Ok(customersWithStats);
    }
}
