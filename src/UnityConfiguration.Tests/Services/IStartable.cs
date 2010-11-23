namespace UnityConfiguration.Services
{
    public interface IStartable
    {
        bool StartWasCalled { get; set; }
        void Start();
    }
}