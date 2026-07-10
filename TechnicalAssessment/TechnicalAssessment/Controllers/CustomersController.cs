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
    public ActionResult<IEnumerable<Customer>> Get()
    {
        var customers = _dataStore.Report.Customers.Successes;
        return Ok(customers);
    }
}
