namespace Infrastructure.Application;

public class EnvironmentVariableAttribute : Attribute
{
    public string Name { get; set; }

    public EnvironmentVariableAttribute(string name)
    {
        Name = name;
    }
}