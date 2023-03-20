namespace Domain.SiteSetting;

public class SiteSettingModel
{
    public ConnectionStringsModel ConnectionStrings { get; set; }
    public LoggingModel Logging { get; set; }
    public string AllowedHosts { get; set; }
}