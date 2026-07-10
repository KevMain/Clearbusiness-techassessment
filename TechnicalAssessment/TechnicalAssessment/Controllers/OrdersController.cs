using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssessment.Domain;
using TechnicalAssessment.Infrastructure;

namespace TechnicalAssessment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public OrdersController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet("customer/{customerId}")]
    public IActionResult GetByCustomerId(int customerId)
    {
        var orders = _dataStore.Report.Orders.Successes
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        var orderTotals = _dataStore.Report.GetOrderTotals();
        var customerTotals = _dataStore.Report.GetCustomerTotals();

        var ordersWithTotals = orders.Select(o => new
        {
            o.OrderId,
            o.CustomerId,
            o.OrderStatus,
            o.OrderDate,
            o.RequiredDate,
            o.ShippedDate,
            OrderTotal = orderTotals.TryGetValue(o.OrderId, out var total) ? total : 0m
        }).ToList();

        return Ok(new
        {
            Orders = ordersWithTotals,
            CustomerTotal = customerTotals.TryGetValue(customerId, out var custTotal) ? custTotal : 0m
        });
    }

    [HttpGet("{orderId}/items")]
    public ActionResult<IEnumerable<OrderItem>> GetOrderItems(int orderId)
    {
        var orderItems = _dataStore.Report.Items.Successes
            .Where(item => item.OrderId == orderId)
            .ToList();

        return Ok(orderItems);
    }
}
