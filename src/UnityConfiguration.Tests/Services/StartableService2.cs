namespace UnityConfiguration.Services
{
    public class StartableService2 : IStartable
    {
        public void Start()
        {
            StartWasCalled = true;
        }

        public bool StartWasCalled { get; set; }
    }
}