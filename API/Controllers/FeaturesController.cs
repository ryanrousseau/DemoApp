using Microsoft.AspNetCore.Mvc;
using OpenFeature;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class FeaturesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IFeatureClient _featureClient;
    private readonly ILogger<VersionController> _logger;

    public FeaturesController(IConfiguration configuration, ILogger<VersionController> logger)
    {
        _configuration = configuration;
        _featureClient = Api.Instance.GetClient();
        _logger = logger;
    }

    [HttpGet(Name = "GetFeatures")]
    public async Task<FeatureDetails> Get()
    {
        if (_featureClient != null) {
            var crashApplication = await _featureClient.GetBooleanValueAsync("crash", false);

            if (crashApplication) {
                throw new Exception("Crashed!");
            }
        }

        var details = new FeatureDetails
        {
            FeatureOne = await _featureClient.GetBooleanValueAsync("feature-one", false),
            FeatureTwo = await _featureClient.GetBooleanValueAsync("feature-two", false)
        };

        return details;
    }
}
