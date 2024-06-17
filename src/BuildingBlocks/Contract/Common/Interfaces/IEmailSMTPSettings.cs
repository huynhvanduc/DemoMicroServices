namespace Contract.Common.Interfaces;

public interface IEmailSMTPSetting
{
    string DisplayName { get; set; }
    string EnableVerification { get; set; }
    string From { get; set; }
    string SMTPServer { get; set; }
    bool UseSsl { get; set; }
    int Port { get; set; }
    string Username { get; set; }
    string Password { get; set; }
}
