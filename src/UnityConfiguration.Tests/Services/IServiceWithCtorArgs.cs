namespace UnityConfiguration.Services
{
    public interface IServiceWithCtorArgs
    {
        string SomeString { get; set; }
        IFooService FooService { get; set; }
    }
}