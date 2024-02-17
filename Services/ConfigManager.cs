using almondcove.Interefaces.Services;

namespace almondcove.Services
{
	public class ConfigManager(IConfiguration configuration) : IConfigManager
	{
		private readonly IConfiguration _configuration = configuration;

        public string GetConnString() => _configuration["ConnectionStrings:almondCoveStr"]?.ToString();
		public string GetSmtpServer() => _configuration["SmtpSettings:Server"];
		public string GetSmtpPort() => _configuration["SmtpSettings:Port"];
		public string GetSmtpUsername() => _configuration["SmtpSettings:Username"];
		public string GetSmtpPassword() => _configuration["SmtpSettings:Password"];
		public string GetCryptKey() => _configuration["EncryptionKey"]?.ToString();
	}
}
