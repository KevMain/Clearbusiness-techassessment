using TechnicalAssessment.Domain;

namespace TechnicalAssessment.Application;

public class HelloAppService : IHelloAppService
{
    private readonly IGreeter _greeter;

    public HelloAppService(IGreeter greeter)
    {
        _greeter = greeter;
    }

    public string GetHello()
    {
        return _greeter.GetGreeting();
    }
}
