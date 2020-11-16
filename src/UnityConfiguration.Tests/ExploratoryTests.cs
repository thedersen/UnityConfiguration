using NUnit.Framework;
using Unity;

namespace UnityConfiguration
{
    [TestFixture]
    public class ExploratoryTests
    {
        [Test]
        public void Can_resolve_the_container_without_registering_it()
        {
            var container = new UnityContainer();

            container.Configure(x => { });

            Assert.That(container.Resolve<IUnityContainer>(), Is.SameAs(container));
        }
    }
}