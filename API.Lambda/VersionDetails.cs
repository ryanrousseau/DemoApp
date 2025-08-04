namespace API.Lambda;

public class VersionDetails
{
    public string? ApplicationName { get; set; }
    public string? DeploymentLink { get; set; }
    public string? DeploymentTime { get; set; }
    public string? Environment { get; set; }
    public Dictionary<string, string>? Features { get; set; }
    public string? Image { get; set; }
    public string? Release { get; set; }
}
