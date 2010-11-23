namespace UnityConfiguration.Services
{
    public class StartableService2 : IStartable
    {
        #region IStartable Members

        public void Start()
        {
            StartWasCalled = true;
        }

        public bool StartWasCalled { get; set; }

        #endregion
    }
}