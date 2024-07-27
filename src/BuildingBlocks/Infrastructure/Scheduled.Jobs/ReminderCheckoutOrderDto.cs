namespace Infrastructure.Scheduled.Jobs;

public record ReminderCheckoutOrderDto(string email, 
    string subject, string emailContent, DateTimeOffset enqueueAt);
