namespace UnityConfiguration.Services
{
    public class FooDecorator : IFooDecorator, IFooService
    {
        public IFooService InnerService { get; set; }

        public FooDecorator(IFooService fooService)
        {
            InnerService = fooService;
        }
    }
}