namespace UnityConfiguration.Services
{
    public interface IStartable
    {
        void Start();
        bool StartWasCalled { get; set; }
    }
}