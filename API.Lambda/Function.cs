using Amazon.Lambda.Core;
using Octopus.OpenFeature.Provider;
using OpenFeature;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace API.Lambda;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<VersionDetails> FunctionHandler(ILambdaContext context)
    {
        var clientIdentifier = Environment.GetEnvironmentVariable("OPEN_FEATURE_CLIENT_ID") ?? string.Empty;
        var featureProvider = new OctopusFeatureProvider(new OctopusFeatureConfiguration(clientIdentifier));
        var feature1Enabled = false;
        var feature2Enabled = false;
        
        await Api.Instance.SetProviderAsync(featureProvider);
        var featureClient = Api.Instance.GetClient();

        if (featureClient != null)
        {
            var crashApplication = await featureClient.GetBooleanValueAsync("crash", false);

            if (crashApplication)
            {
                throw new Exception("Crashed!");
            }

            feature1Enabled = await featureClient.GetBooleanValueAsync("feature-one", false);

            feature2Enabled = await featureClient.GetBooleanValueAsync("feature-two", false);
        }

        var details = new VersionDetails
        {
            ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME"),
            DeploymentLink = Environment.GetEnvironmentVariable("DEPLOYMENT_LINK"),
            DeploymentTime = Environment.GetEnvironmentVariable("DEPLOYMENT_TIME"),
            Environment = Environment.GetEnvironmentVariable("ENVIRONMENT"),
            Features = new Dictionary<string, string>
            {
                { "feature-one", feature1Enabled.ToString() },
                { "feature-two", feature2Enabled.ToString() }
            },
            Image = Environment.GetEnvironmentVariable("IMAGE"),
            Release = Environment.GetEnvironmentVariable("RELEASE")
        };

        return details;
    }
}
