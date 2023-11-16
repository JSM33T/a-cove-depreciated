using System.Net.Mail;
using System.Net;

namespace almondCove.Services
{
  public class Mailer : IMailer
  {
    private readonly IConfigManager _configManager;

    public Mailer(IConfigManager configurationService)
    {
		_configManager = configurationService;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
      using var client = new SmtpClient(_configManager.GetSmtpServer(), int.Parse(_configManager.GetSmtpPort()));
      client.UseDefaultCredentials = false;
      client.Credentials = new NetworkCredential(_configManager.GetSmtpUsername(), _configManager.GetSmtpPassword());
      client.EnableSsl = true;

      var message = new MailMessage(_configManager.GetSmtpUsername(), to)
      {
        Subject = subject,
        Body = body,
        IsBodyHtml = true
      };

      await client.SendMailAsync(message);
    }
  }
}
