using Contract.ScheduleJobs;

namespace Hangfire.API.Services.Interfaces;

public interface IBackgroundJobServices
{
    IScheduleJobService BackgroundJobService { get; }
    string SendEmailContent(string email, string subject, string emailCotent, DateTimeOffset enqueueAt);
}
