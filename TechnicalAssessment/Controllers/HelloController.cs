using Microsoft.AspNetCore.Mvc;
using TechnicalAssessment.Application;

namespace TechnicalAssessment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    private readonly IHelloAppService _service;

    public HelloController(IHelloAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = _service.GetHello() });
    }
}
