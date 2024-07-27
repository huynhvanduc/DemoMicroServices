using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Contract.Common.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Scheduled.Jobs;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;
    private readonly BackgroundJobHttpService _backgroundJobHttpService;
    private readonly IEmailTemplateServices _emailTemplateServices;

    public BasketRepository(IDistributedCache redisCacheService, 
        ISerializeService serializeService, 
        ILogger logger, 
        BackgroundJobHttpService backgroundJobHttpService,
        IEmailTemplateServices emailTemplateService)
    {
        _redisCacheService = redisCacheService;
        _serializeService = serializeService;
        _logger = logger;
        _backgroundJobHttpService = backgroundJobHttpService;
        _emailTemplateServices = emailTemplateService;
    }

    public async Task<bool> DeleteBasketFromUserName(string userName)
    {
        await DeleteReminderCheckoutOrder(userName);
        try
        {
            await _redisCacheService.RemoveAsync(userName);
            return true;
        }
        catch (Exception e)
        {
            _logger.Error("DeleteBasketFromUserName" + e.Message);
            throw;
        }
    }

    public async Task<Cart> GetBasketByUserName(string userName)
    {
        var basket = await _redisCacheService.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null :
            _serializeService.Deserialize<Cart>(basket);
    }

    public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
    {
        await DeleteReminderCheckoutOrder(cart.UserName);

        if (options is not null)
        {
            await _redisCacheService.SetStringAsync(cart.UserName,
                _serializeService.Serialize(cart), options);
        }
        else
        {
            await _redisCacheService.SetStringAsync(cart.UserName,
                _serializeService.Serialize(cart));
        }

        try
        {
            await TriggerSendEmailReminderCheckout(cart);
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
        }

        return await GetBasketByUserName(cart.UserName);
    }

    private async Task TriggerSendEmailReminderCheckout(Cart cart)
    {
        var emailTemplate = _emailTemplateServices.GenerateReminderCheckoutOrderEmail(cart.UserName);

        var model = new ReminderCheckoutOrderDto("vanduc9x98@gmail.com", "Reminder checkout", emailTemplate, DateTimeOffset.Now.AddMinutes(10));

        var Uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/send-email-reminder-checkout-order";
        var response = await _backgroundJobHttpService.Client.PostAsJson(Uri, model);
        if (response.EnsureSuccessStatusCode().IsSuccessStatusCode) 
        {
            var jobId = await response.ReadContentAs<string>();

            if (!string.IsNullOrEmpty(jobId)) 
            {
                cart.JobId = jobId;
                await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
        }
    }

    private async Task DeleteReminderCheckoutOrder(string username)
    {
        var cart = await GetBasketByUserName(username);
        if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

        var jobId = cart.JobId;
        var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/delete/jobId/{jobId}";

        _backgroundJobHttpService.Client.DeleteAsync(uri);

        _logger.Information($"DeleteReminderCheckoutOrder:Deleted JobId: {jobId}");
    }
}
