using Shared.Services.Emails;

namespace Contract.Services;

public interface ISmtpEmailService : IEmailService<MailRequest>
{
}
