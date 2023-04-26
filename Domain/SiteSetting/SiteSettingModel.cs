namespace Domain.SiteSetting
{
    public class SiteSettingModel
    {
        public ConnectionStringModel ConnectionStrings { get; set; } = null!;
        public LoggingModel Logging { get; set; } = null!;
        public string AllowedHosts { get; set; } = null!;
    }
}
