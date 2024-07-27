using Hangfire.API.Services.Interfaces;
using Infrastructure.Scheduled.Jobs;
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Emails;
using System.ComponentModel.DataAnnotations;

namespace Hangfire.API.Extensions;

[ApiController]
[Route("api/scheduled-jobs")]
public class ScheduledJobsController : ControllerBase
{
    private readonly IBackgroundJobServices _jobServices;

    public ScheduledJobsController(IBackgroundJobServices backgroundJobServices)
    {
        _jobServices = backgroundJobServices;
    }

    [HttpPost]
    [Route("send-email-reminder-checkout-order")]
    public IActionResult SendReminderCheckoutOrderEmail([FromBody] ReminderCheckoutOrderDto model)
    {
        var jobId = _jobServices.SendEmailContent(model.email, model.subject, model.emailContent, model.enqueueAt);
        return Ok(jobId);
    }

    [HttpDelete]
    [Route("delete/jobId/{id}")]
    public  IActionResult DeleteJobId([Required] string id) 
    {
        var result =  _jobServices.BackgroundJobService.Delete(id);
        return Ok(result);
    }
}
