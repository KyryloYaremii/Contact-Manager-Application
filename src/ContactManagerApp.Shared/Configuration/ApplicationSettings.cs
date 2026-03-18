namespace ContactManagerApp.Shared.Configuration;

public class ApplicationSettings
{
    public const string SectionName = "ApplicationSettings";
    public string ConnectionString { get; init; } = string.Empty;
}