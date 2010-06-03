namespace UnityConfiguration.Services
{
    public class StoppableService : IStoppable
    {
        public void Stop()
        {
            StopWasCalled = true;
        }

        public bool StopWasCalled { get; set; }
    }
}