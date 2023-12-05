using System.Net.Mail;
using System.Net;
using almondCove.Interefaces.Services;

namespace almondCove.Services
{
	public class Mailer : IMailer
	{
		private readonly IConfigManager _configManager;
		private readonly ILogger<Mailer> _logger;

		public Mailer(IConfigManager configurationService, ILogger<Mailer> logger)
		{
			_configManager = configurationService;
			_logger = logger;
		}

		public bool SendEmailAsync(string to, string subject, string body)
		{
			try
			{
				MailMessage message = new()
				{
					From = new MailAddress("mail@almondCove.in", "AlmondCove"),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};
				message.To.Add(to);
				SmtpClient smtpClient = new()
				{
					Host = "almondcove.in",
					Port = int.Parse(_configManager.GetSmtpPort()),
					EnableSsl = false,
					Credentials = new NetworkCredential(_configManager.GetSmtpUsername(), _configManager.GetSmtpPassword()),
					Timeout = 10000
				};
				smtpClient.Send(message);
				return true;
			}
			catch(Exception ex) {
				_logger.LogError(ex.Message);
				return false;
			}
		}
  }
}
