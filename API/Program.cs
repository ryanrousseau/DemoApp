using Octopus.OpenFeature.Provider;
using OpenFeature.Contrib.Providers.EnvVar;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "");

var clientIdentifier = Environment.GetEnvironmentVariable("OPEN_FEATURE_CLIENT_ID");

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("In Development mode");
    await OpenFeature.Api.Instance.SetProviderAsync(new EnvVarProvider("FeatureToggle_"));
}
else if (!string.IsNullOrEmpty(clientIdentifier))
{
    Console.WriteLine("Configuring Octopus Feature Provider");
    await OpenFeature.Api.Instance.SetProviderAsync(new OctopusFeatureProvider(new OctopusFeatureConfiguration(clientIdentifier)));
}

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.MapHealthChecks("/healthz");

app.Run();
