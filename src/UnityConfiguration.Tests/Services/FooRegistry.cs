namespace UnityConfiguration.Services
{
    public class FooRegistry : UnityRegistry
    {
        public FooRegistry()
        {
            Register<IFooService, FooService>();
        }
    }
}