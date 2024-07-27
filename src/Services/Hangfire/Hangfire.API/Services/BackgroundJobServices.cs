using Contract.ScheduleJobs;
using Contract.Services;
using Hangfire.API.Services.Interfaces;
using Shared.Services.Emails;
using ILogger = Serilog.ILogger;

namespace Hangfire.API.Services;

public class BackgroundJobServices : IBackgroundJobServices
{
    private readonly IScheduleJobService _jobService;
    private readonly ISmtpEmailService _emailService;
    private readonly ILogger _logger;

    public BackgroundJobServices(IScheduleJobService jobService, ISmtpEmailService emailService, ILogger logger)
    {
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger;
    }

    public IScheduleJobService BackgroundJobService => _jobService;

    public string? SendEmailContent(string email, string subject, string emailCotent, DateTimeOffset enqueueAt)
    {
        var emailRequest = new MailRequest
        {
            ToAddress = email,
            Body = emailCotent,
            Subject = subject,
        };

        try
        {
            var jobId = _jobService.Schedule(() => _emailService.SendEmail(emailRequest), enqueueAt);
            _logger.Information($"Sent email to {email} with subject: {subject} - job id: {jobId}");
            return jobId;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }

        return null;
    }
}
