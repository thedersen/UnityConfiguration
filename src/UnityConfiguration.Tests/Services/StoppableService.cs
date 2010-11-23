namespace UnityConfiguration.Services
{
    public class StoppableService : IStoppable
    {
        #region IStoppable Members

        public void Stop()
        {
            StopWasCalled = true;
        }

        public bool StopWasCalled { get; set; }

        #endregion
    }
}