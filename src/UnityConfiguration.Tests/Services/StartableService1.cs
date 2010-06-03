namespace UnityConfiguration.Services
{
    public class StartableService1 : IStartable
    {
        public void Start()
        {
            StartWasCalled = true;
        }

        public bool StartWasCalled { get; set; }
    }
}