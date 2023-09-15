namespace Infrastructure.Logging;

public class LoggerSettings
{
    public string AppName { get; set; } = "WebAPI";
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}
