namespace UnityConfiguration.Services
{
    public class ServiceWithCtorArgs : IServiceWithCtorArgs
    {
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

        #region IServiceWithCtorArgs Members

        public string SomeString { get; set; }
        public IFooService FooService { get; set; }

        #endregion
    }
}