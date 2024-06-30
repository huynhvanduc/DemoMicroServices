namespace Contract.Messages;

public interface IMessageProducer
{
    void SendMessage<T>(T Message);
}
