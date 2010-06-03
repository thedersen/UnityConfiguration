namespace UnityConfiguration.Services
{
    public interface IStoppable
    {
        void Stop();
        bool StopWasCalled { get; set; }
    }
}