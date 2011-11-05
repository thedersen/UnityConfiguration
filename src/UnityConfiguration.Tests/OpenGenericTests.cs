using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class OpenGenericTests
    {
        [Test]
        public void Can_connect_implementations_to_open_generic_types()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeType<MessageHandler2>();
            }));

            Assert.That(container.Resolve<IHandler<Message>>(), Is.InstanceOf<MessageHandler>());
            Assert.That(container.Resolve<IHandler<AnotherMessage>>(), Is.InstanceOf<AnotherMessageHandler>());
        }

        [Test]
        public void Can_connect_implementations_to_open_generic_types_2()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeType<MessageHandler2>();
            }));

            Assert.That(container.Resolve<IMapper<Message, AnotherMessage>>(), Is.InstanceOf<MessageToAnotherMessageMapper>());
        }
    }
}