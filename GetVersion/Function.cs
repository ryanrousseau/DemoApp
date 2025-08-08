using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetVersion;

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
        var details = new VersionDetails
        {
            ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME"),
            DeploymentLink = Environment.GetEnvironmentVariable("DEPLOYMENT_LINK"),
            DeploymentTime = Environment.GetEnvironmentVariable("DEPLOYMENT_TIME"),
            Environment = Environment.GetEnvironmentVariable("ENVIRONMENT"),
            Image = Environment.GetEnvironmentVariable("IMAGE"),
            Release = Environment.GetEnvironmentVariable("RELEASE")
        };

        return details;
    }
}
