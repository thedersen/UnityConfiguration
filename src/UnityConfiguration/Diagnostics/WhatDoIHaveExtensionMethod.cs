using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace UnityConfiguration.Diagnostics
{
    public static class WhatDoIHaveExtensionMethod
    {
        public static string WhatDoIHave(this IUnityContainer container)
        {
            
            
            var stringBuilder = new StringBuilder();
            container.Configure<IWhatDoIHave>().Print(stringBuilder);

            return stringBuilder.ToString();
        }
    }

    public interface IWhatDoIHave : IUnityContainerExtensionConfigurator
    {
        void Print(StringBuilder stringBuilder);
    }

    public class DiagnosticsExtension : UnityContainerExtension, IWhatDoIHave
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        protected override void Initialize()
        {
            Context.Registering += Context_Registering;
        }

        void Context_Registering(object sender, RegisterEventArgs e)
        {
            stringBuilder.AppendFormat("{0} - {1} - {2}", e.TypeFrom, e.TypeTo, e.Name);
        }

        public void Print(StringBuilder sb)
        {
            sb.Append(stringBuilder.ToString());
        }
    }
}