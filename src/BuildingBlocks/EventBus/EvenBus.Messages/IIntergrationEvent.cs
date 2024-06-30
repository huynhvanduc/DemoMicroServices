namespace EvenBus.Messages;

public interface IIntergrationEvent
{
    public DateTime CreationDate { get; }
    public Guid Id { get; set; }
}
