namespace UnityConfiguration.Services
{
    public class FooDecorator : IFooDecorator, IFooService
    {
        public FooDecorator(IFooService fooService)
        {
            InnerService = fooService;
        }

        public IFooService InnerService { get; set; }
    }
}