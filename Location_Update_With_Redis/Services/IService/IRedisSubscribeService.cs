namespace Services.IService
{
    public interface IRedisSubscribeService
    {
        void StartListening(Action<object> messageReceivedCallback);
    }
}
