namespace UnityConfiguration.Services
{
    public class ServiceWithCtorArgs : IServiceWithCtorArgs
    {
        public string SomeString { get; set; }
        public IFooService FooService { get; set; }

        public ServiceWithCtorArgs()
        {
        }

        public ServiceWithCtorArgs(IFooService fooService)
        {
            FooService = fooService;
        }

        public ServiceWithCtorArgs(string someString, IFooService fooService)
        {
            SomeString = someString;
            FooService = fooService;
        }
    }
}