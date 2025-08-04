using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DemoApp.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<VersionController> _logger;

    public VersionController(IConfiguration configuration, ILogger<VersionController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet(Name = "GetVersion")]
    public VersionDetails Get()
    {
        var details = new VersionDetails {
            Version = _configuration["Version"]
        };

        Console.WriteLine(details.Version);

        return details;
    }
}
