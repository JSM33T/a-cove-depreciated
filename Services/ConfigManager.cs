using almondCove.Interefaces.Services;

namespace almondCove.Services
{
    public class ConfigManager : IConfigManager
  {
    private readonly IConfiguration _configuration;

    public ConfigManager(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GetConnString()
    {
      return _configuration["ConnectionStrings:almondCoveStr"]?.ToString();
    }

    public string GetSmtpServer()
    {
      return _configuration["SmtpSettings:Server"];
    }

    public string GetSmtpPort()
    {
      return _configuration["SmtpSettings:Port"];
    }

    public string GetSmtpUsername()
    {
      return _configuration["SmtpSettings:Username"];
    }

    public string GetSmtpPassword()
    {
      return _configuration["SmtpSettings:Password"];
    }

    public string GetCryptKey()
    {
      return _configuration["EncryptionKey"]?.ToString();
    }
  }
}
