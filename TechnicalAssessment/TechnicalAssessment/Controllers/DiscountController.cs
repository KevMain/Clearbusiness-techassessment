using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssessment.Infrastructure;

namespace TechnicalAssessment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DiscountController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public DiscountController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpPost("apply-ca")]
    public IActionResult ApplyCADiscount()
    {
        // Apply 5% discount to CA customers
        _dataStore.ApplyCADiscount(0.05m);

        return Ok(new { message = "CA discount applied successfully", discountPercent = 5 });
    }
}
