namespace Infrastructure.Application;

public class BaseApplicationConfig
{
    [EnvironmentVariable("APPLICATION_ENVIRONMENT")]
    public string? ApplicationEnvironment { get; set; }

    [EnvironmentVariable("SERVICE_NAME")] 
    public string ServiceName { get; set; } = "DONT_KNOWN_APPLICATION";
    
    [EnvironmentVariable("IS_PRIVATE_HOSTED")]
    public bool? IsPrivateHosted { get; set; }
}