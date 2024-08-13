﻿using Contract.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using Shared.Services.Emails;
using System.Threading;

namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;
    private readonly SMTPEmailSetting _settings;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, SMTPEmailSetting settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _smtpClient = new SmtpClient();
    }

    private MimeMessage getMineMessage(MailRequest request)
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
            foreach (var toAddress in request.ToAddresses)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
        else
        {
            var toAddress = request.ToAddress;
            emailMessage.To.Add(MailboxAddress.Parse(toAddress));
        }

        return emailMessage;
    }

    public void SendEmail(MailRequest request)
    {
        var emailMessage = getMineMessage(request);

        try
        {
             _smtpClient.Connect(_settings.SMTPServer, _settings.Port,
                _settings.UseSsl);
             _smtpClient.Authenticate(_settings.Username, _settings.Password);
             _smtpClient.Send(emailMessage);
             _smtpClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
        finally
        {
             _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        var emailMessage = getMineMessage(request);

        try
        {
            await _smtpClient.ConnectAsync(_settings.SMTPServer, _settings.Port,
                _settings.UseSsl, cancellationToken);
            await _smtpClient.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await _smtpClient.SendAsync(emailMessage, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _smtpClient.Dispose();
        }
    }
}