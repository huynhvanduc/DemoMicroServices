using Contract.Common.Interfaces;

namespace Infrastructure.Common;


public class EmailSMTPSetting : IEmailSMTPSetting
{
    public string DisplayName {get; set; }
    public string EnableVerification { get; set; }
    public string From { get; set; }
    public string SMTPServer { get; set; }
    public bool UseSsl { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
