using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace UnityConfiguration
{
    [TestFixture]
    public class UnityExtensionsConfigurationTests
    {
        [Test]
        public void Can_configure_unity_extensions()
        {
            const int number = 1;
            var container = new UnityContainer();

            container.Configure(x =>
                                    {
                                        x.AddExtension<MyExtension>();
                                        x.ConfigureExtension<MyExtension>(c => c.SetNumber(number));
                                    });

            Assert.That(MyExtension.Number, Is.EqualTo(number));
        }
    }

    public class MyExtension : UnityContainerExtension
    {
        public static int Number;

        protected override void Initialize()
        {
        }

        public void SetNumber(int number)
        {
            Number = number;
        }
    }
}