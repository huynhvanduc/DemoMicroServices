using Basket.API.Services.Interfaces;
using Shared.Configurations;

namespace Basket.API.Services;

public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateServices
{
    public BasketEmailTemplateService(BackgroundJobSettings settings) : base(settings)
    {
    }

    public string GenerateReminderCheckoutOrderEmail(string username)
    {
        var _checkoutUrl = $"{_backgroundJobSettings.CheckoutUrl}/{_backgroundJobSettings.BasketUrl}/{username}";
        var emailText = ReadEmailTemplateContent("reminder-checkout-order");
        var emailReplaceText = emailText.Replace("[username]", username)
            .Replace("[checkoutUrl]", _checkoutUrl);

        return emailReplaceText;
    }
}
