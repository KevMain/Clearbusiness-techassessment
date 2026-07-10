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
    public ActionResult<IEnumerable<Order>> GetByCustomerId(int customerId)
    {
        var orders = _dataStore.Report.Orders.Successes
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return Ok(orders);
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
