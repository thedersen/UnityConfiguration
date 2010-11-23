namespace UnityConfiguration.Services
{
    public interface IStoppable
    {
        bool StopWasCalled { get; set; }
        void Stop();
    }
}