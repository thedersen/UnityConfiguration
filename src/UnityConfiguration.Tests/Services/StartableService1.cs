namespace UnityConfiguration.Services
{
    public class StartableService1 : IStartable
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