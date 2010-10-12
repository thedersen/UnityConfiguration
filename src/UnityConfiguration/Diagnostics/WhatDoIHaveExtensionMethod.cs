using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace UnityConfiguration.Diagnostics
{
    public static class WhatDoIHaveExtensionMethod
    {
        public static string WhatDoIHave(this IUnityContainer container)
        {
            var stringBuilder = new StringBuilder();

            var registrations = container.Registrations.Select(ToRegistrationString).ToList();
            registrations.Sort();
            registrations.ForEach(s => stringBuilder.AppendLine(s));

            return stringBuilder.ToString();
        }

        private static string ToRegistrationString(ContainerRegistration registration)
        {
            return registration.RegisteredType.FullName + " - " + registration.MappedToType.FullName + Named(registration) + AsSingleton(registration);
        }

        private static string Named(ContainerRegistration registration)
        {
            return registration.Name != null ? string.Format(" named \"{0}\"", registration.Name) : null;
        }

        private static string AsSingleton(ContainerRegistration registration)
        {
            return registration.LifetimeManager != null ? " with " + registration.LifetimeManager.GetType().Name : null;
        }
    }
}