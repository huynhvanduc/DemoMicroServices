namespace EvenBus.Messages;

public record IntegrationBaseEvent()  : IIntergrationEvent
{
    public DateTime CreationDate { get; } = DateTime.Now;
    public Guid Id { get; set; }
}
