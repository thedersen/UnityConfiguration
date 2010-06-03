namespace UnityConfiguration.Services
{
    public class BarRegistry : UnityRegistry
    {
        public BarRegistry()
        {
            Register<IBarService, BarService>();
        }
    }
}