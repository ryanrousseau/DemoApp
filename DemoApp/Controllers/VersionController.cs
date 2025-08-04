using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenFeature;

namespace DemoApp.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IFeatureClient _featureClient;
    private readonly ILogger<VersionController> _logger;

    public VersionController(IConfiguration configuration, ILogger<VersionController> logger)
    {
        _configuration = configuration;
        _featureClient = Api.Instance.GetClient();
        _logger = logger;
    }

    [HttpGet(Name = "GetVersion")]
    public async Task<VersionDetails> Get()
    {
        var crashApplication = await _featureClient.GetBooleanValueAsync("crash", false);

        if (crashApplication) {
            throw new Exception("Crashed!");
        }

        var details = new VersionDetails {
            Release = _configuration["RELEASE"],
            Version = _configuration["VERSION"]
        };

        Console.WriteLine(details.Version);

        return details;
    }
}
