using Microsoft.AspNetCore.Mvc;
using OpenFeature;

namespace API.Controllers;

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
        var details = new VersionDetails {
            ApplicationName = _configuration["APPLICATION_NAME"],
            DeploymentLink = _configuration["DEPLOYMENT_LINK"],
            DeploymentTime = _configuration["DEPLOYMENT_TIME"],
            Environment = _configuration["ENVIRONMENT"],
            Image = _configuration["IMAGE"],
            Release = _configuration["RELEASE"]
        };

        return details;
    }
}
