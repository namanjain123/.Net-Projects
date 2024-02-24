namespace Services.IService
{
    public interface IRedisPublishService
    {
        Task<string> PublishMessageAsync(object message);
    }

}
