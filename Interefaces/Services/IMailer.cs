namespace almondcove.Interefaces.Services
{
    public interface IMailer
    {
        bool SendEmailAsync(string to, string subject, string body);
    }
}
