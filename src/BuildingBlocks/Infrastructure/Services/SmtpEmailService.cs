using Contract.Services;
using Shared.Services.Emails;
using Serilog;
using System.Net.Mail;
using MimeKit;
using Infrastructure.Common;

namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;
    private readonly EmailSMTPSetting _settings;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, EmailSMTPSetting settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _smtpClient = new SmtpClient();
    }


    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        var emailMessage = new MimeMessage
        {
            Sender = new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };

        if (request.ToAddresses.Any())
        {
            foreach(var toAddress in request.ToAddresses)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
    }
}
