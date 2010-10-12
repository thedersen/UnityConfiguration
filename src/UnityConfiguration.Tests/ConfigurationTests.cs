using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;
using UnityConfiguration.Services.OtherNamespace;

namespace UnityConfiguration
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void Can_resolve_the_container_without_registering_it()
        {
            var container = new UnityContainer();

            container.Initialize(x => { });

            Assert.That(container.Resolve<IUnityContainer>(), Is.SameAs(container));
        }

        [Test]
        public void Can_initalize_container_with_one_registry()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.AddRegistry<FooRegistry>());

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_initalize_container_with_two_registries()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
                                     {
                                         x.AddRegistry<FooRegistry>();
                                         x.AddRegistry<BarRegistry>();
                                     });

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Can_register_transient_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Register<IBarService, BarService>());

            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.Resolve<IBarService>(), Is.Not.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_register_singletons()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Register<IBarService, BarService>().AsSingleton());

            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_register_named_instance()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Register<IBarService, BarService>().WithName("name"));

            Assert.That(container.Resolve<IBarService>("name"), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Can_register_named_singleton_instance()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Register<IBarService, BarService>().WithName("name").AsSingleton());

            Assert.That(container.Resolve<IBarService>("name"), Is.SameAs(container.Resolve<IBarService>("name")));
        }

        [Test]
        public void Can_register_using_factory_delegate()
        {
            var container = new UnityContainer();

            var myService = new BarService();
            container.Initialize(x => x.Register<IBarService>(c => myService));

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(myService));
        }

        [Test]
        public void Can_register_named_instance_using_factory_delegate()
        {
            var container = new UnityContainer();

            var myService = new BarService();
            container.Initialize(x => x.Register<IBarService>(c => myService).WithName("name"));

            Assert.That(container.Resolve<IBarService>("name"), Is.SameAs(myService));
        }

        [Test]
        public void Can_scan_using_the_first_interface_convention()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
                     {
                         scan.AssemblyContaining<FooRegistry>();
                         scan.With<FirstInterfaceConvention>();
                     }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Can_scan_using_the_add_all_convention()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
                     {
                         scan.AssemblyContaining<FooRegistry>();
                         scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
                     }));

            Assert.That(container.ResolveAll<IHaveManyImplementations>().Count(), Is.EqualTo(2));
            Assert.That(container.ResolveAll<IHaveManyImplementations>().First(), Is.Not.SameAs(container.ResolveAll<IHaveManyImplementations>().First()));
        }

        [Test]
        public void Can_scan_using_the_add_all_convention_and_override_the_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
                     {
                         scan.AssemblyContaining<FooRegistry>();
                         scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().WithName(t => "test");
                     }));

            Assert.That(container.Resolve<IHaveManyImplementations>("test"), Is.Not.Null);
        }

        [Test]
        public void Can_scan_using_the_add_all_convention_and_add_all_as_singletons()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().AsSingleton();
            }));

            Assert.That(container.ResolveAll<IHaveManyImplementations>().First(), Is.SameAs(container.ResolveAll<IHaveManyImplementations>().First()));
        }

        [Test]
        public void Can_scan_using_the_add_all_convention_add_all_as_singletons_and_override_the_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().WithName(t => "test").AsSingleton();
            }));

            Assert.That(container.Resolve<IHaveManyImplementations>("test"), Is.SameAs(container.Resolve<IHaveManyImplementations>("test")));
        }

        [Test]
        public void Can_scan_using_several_conventions()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
                     {
                         scan.AssemblyContaining<FooRegistry>();
                         scan.With<FirstInterfaceConvention>();
                         scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
                     }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.ResolveAll<IHaveManyImplementations>().Count(), Is.EqualTo(2));
        }

        [Test]
        public void Can_configure_concrete_types_as_singletons()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
                {
                    x.Register<IBarService, BarService>();
                    x.MakeSingleton<BarService>();
                });

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_configure_interfaces_as_singletons()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
                {
                    x.Register<IBarService, BarService>();
                    x.MakeSingleton<IBarService>();
                });

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }


        [Test]
        public void Can_configure_interfaces_as_singletons_2()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssemblyContaining<FooRegistry>();
                    scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
                });
                x.MakeSingleton<IHaveManyImplementations>();
            });

            Assert.That(container.Resolve<IHaveManyImplementations>("Implementation1"), Is.SameAs(container.Resolve<IHaveManyImplementations>("Implementation1")));
            Assert.That(container.Resolve<IHaveManyImplementations>("Implementation2"), Is.SameAs(container.Resolve<IHaveManyImplementations>("Implementation2")));
        }

        [Test]
        public void Can_connect_implementations_to_open_generic_types()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
            }));

            Assert.That(container.Resolve<IHandler<Message>>(), Is.InstanceOf<MessageHandler>());
            Assert.That(container.Resolve<IHandler<AnotherMessage>>(), Is.InstanceOf<AnotherMessageHandler>());
        }

        [Test]
        public void Can_connect_implementations_to_open_generic_types_2()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
            }));

            Assert.That(container.Resolve<IMapper<Message, AnotherMessage>>(), Is.InstanceOf<MessageToAnotherMessageMapper>());
        }

        [Test]
        public void Can_exclude_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeType<BarService>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IBarService>());
        }

        [Test]
        public void Can_exclude_type_using_delegate()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.Exclude(t => t == typeof(BarService));
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IBarService>());
        }

        [Test]
        public void Can_exclude_namespace_containing_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeNamespaceContaining<ServiceInOtherNamespace>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_namespace_containing_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.IncludeNamespaceContaining<ServiceInOtherNamespace>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_namespace()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.IncludeNamespace("UnityConfiguration.Services.OtherNamespace");
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_using_delegate()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.Include(t => t == typeof(ServiceInOtherNamespace));
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }

        [Test]
        public void Can_configure_ctor_arguments_for_type()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
            {
                x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                x.Register<IFooService, FooService>();
                x.ConfigureCtorArgsFor<ServiceWithCtorArgs>("some string", typeof(IFooService));
            });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.EqualTo("some string"));
            Assert.That(serviceWithCtorArgs.FooService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_select_constructor_to_use()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
            {
                x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                x.SelectConstructor<ServiceWithCtorArgs>();
            });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.Null);
        }

        [Test]
        public void Can_select_constructor_to_use_2()
        {
            var container = new UnityContainer();

            container.Initialize(x =>
            {
                x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                x.Register<IFooService, FooService>();
                x.SelectConstructor<ServiceWithCtorArgs>(typeof(IFooService));
            });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_make_registered_transient_sevices_a_singleton_in_child_container()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Register<IFooService, FooService>());

            IUnityContainer childContainer = container.CreateChildContainer();
            childContainer.Initialize(x => x.MakeSingleton<FooService>());

            Assert.That(container.Resolve<IFooService>(), Is.Not.SameAs(container.Resolve<IFooService>()));
            Assert.That(container.Resolve<IFooService>(), Is.Not.SameAs(childContainer.Resolve<IFooService>()));
            Assert.That(childContainer.Resolve<IFooService>(), Is.SameAs(childContainer.Resolve<IFooService>()));
        }

        [Test]
        public void Can_configure_to_call_method_on_concrete_after_build_up()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.AfterBuildingUp<StartableService1>().Call((c, s) => s.Start()));

            Assert.That(container.Resolve<StartableService1>().StartWasCalled);
        }

        [Test]
        public void Can_configure_to_call_method_on_interface_after_build_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<IStartable, StartableService1>();
                                         x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
                                     });

            Assert.That(container.Resolve<IStartable>().StartWasCalled);
        }

        [Test]
        public void Can_configure_to_call_method_on_interface_after_build_up_2()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<IStartable, StartableService1>().WithName("1");
                                         x.Register<IStartable, StartableService2>().WithName("2");
                                         x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
                                     });

            Assert.That(container.Resolve<IStartable>("1").StartWasCalled);
            Assert.That(container.Resolve<IStartable>("2").StartWasCalled);
        }

        [Test]
        public void Can_configure_to_call_method_on_several_interfaces_after_build_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<IStartable, StartableService1>();
                                         x.Register<IStoppable, StoppableService>();
                                         x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
                                         x.AfterBuildingUp<IStoppable>().Call((c, s) => s.Stop());
                                     });

            Assert.That(container.Resolve<IStartable>().StartWasCalled);
            Assert.That(container.Resolve<IStoppable>().StopWasCalled);
        }

        [Test]
        public void Can_decorate_concrete_service_after_build_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<IFooService, FooService>();
                                         x.AfterBuildingUp<FooService>().DecorateWith((c, t) => new FooDecorator(t));
                                     });

            var fooService = container.Resolve<IFooService>();
            Assert.That(fooService, Is.InstanceOf<FooDecorator>());
            Assert.That(fooService.As<FooDecorator>().InnerService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_decorate_interface_after_build_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<IFooService, FooService>();
                                         x.AfterBuildingUp<IFooService>().DecorateWith((c, t) => new FooDecorator(t));
                                     });

            var fooService = container.Resolve<IFooService>();
            Assert.That(fooService, Is.InstanceOf<FooDecorator>());
            Assert.That(fooService.As<FooDecorator>().InnerService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_configure_property_injection_using_convention()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Scan(scan =>
                    {
                        scan.AssemblyContaining<FooRegistry>();
                        scan.With<FirstInterfaceConvention>();
                        scan.With<SetAllPropertiesConvention>().OfType<ILogger>();
                    }));

            Assert.That(container.Resolve<FooService>().Logger, Is.Not.Null);
        }

        [Test]
        public void Can_set_property_after_building_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<ILogger, NullLogger>();
                                         x.AfterBuildingUp<FooService>().Call((c, s) => s.Logger = c.Resolve<ILogger>());
                                     });

            Assert.That(container.Resolve<FooService>().Logger, Is.Not.Null);
        }
    }
}
