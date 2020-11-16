using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;
using Unity.Registration;

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

        private static string ToRegistrationString(IContainerRegistration registration)
        {
            return $"{registration.RegisteredType.FullName} - {registration.MappedToType.FullName}{Named(registration)}{AsSingleton(registration)}";
        }

        private static string Named(IContainerRegistration registration)
        {
            return registration.Name != null ? $" named \"{registration.Name}\"" : null;
        }

        private static string AsSingleton(IContainerRegistration registration)
        {
            return registration.LifetimeManager != null ? $" with {registration.LifetimeManager.GetType().Name}" : null;
        }
    }
}