namespace almondCove.Services
{
	 public interface IMailer
	 {
		  Task SendEmailAsync(string to, string subject, string body);
	 }
}
