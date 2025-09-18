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
    public VersionDetails FunctionHandler(ILambdaContext context)
    {
        var clientIdentifier = Environment.GetEnvironmentVariable("OPEN_FEATURE_CLIENT_ID") ?? string.Empty;
        context.Logger.LogLine("Client Identifier: " + clientIdentifier);
        IFeatureClient featureClient = new OctopusFeatureProvider(new OctopusFeatureConfiguration(clientIdentifier)) as IFeatureClient;
        var feature1Enabled = false;
        var feature2Enabled = false;

        if (featureClient != null)
        {
             context.Logger.LogLine("Feature Client configured");
            var result = featureClient.GetBooleanValueAsync("crash", false).Result;
            var crashApplication = result;

            if (crashApplication)
            {
                throw new Exception("Crashed!");
            }

            result = featureClient.GetBooleanValueAsync("feature-one", false).Result;
            feature1Enabled = result;

            result = featureClient.GetBooleanValueAsync("feature-two", false).Result;
            feature2Enabled = result;
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
