namespace almondcove.Interefaces.Services
{
    public interface IConfigManager
    {
        public string GetConnString();
        public string GetSmtpServer();
        public string GetSmtpPort();
        public string GetSmtpUsername();
        public string GetSmtpPassword();
        public string GetCryptKey();

    }
}
