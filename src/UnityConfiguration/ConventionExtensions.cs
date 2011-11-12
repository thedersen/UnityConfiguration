namespace UnityConfiguration
{
    /// <summary>
    /// Extends the <see cref="IAssemblyScanner"/> with convenion methods for configuring the conventions.
    /// </summary>
    public static class ConventionExtensions
    {
        /// <summary>
        /// Adds a convention to the scanner that will look for and include all <see cref="UnityRegistry"/> derived classes it finds.
        /// </summary>
        public static ScanForRegistriesConvention ForRegistries(this IAssemblyScanner scanner)
        {
            return scanner.With<ScanForRegistriesConvention>();
        }

        /// <summary>
        /// Adds a convention to the scanner that registers all types by the first interface defined for the type.
        /// </summary>
        public static FirstInterfaceConvention WithFirstInterfaceConvention(this IAssemblyScanner scanner)
        {
            return scanner.With<FirstInterfaceConvention>();
        }

        /// <summary>
        /// Adds a convention to the scanner that registers all types by a naming convention.
        /// By default it tries to find an interface with the same name as the
        /// service, prefixed with an I, but this can be overridden.
        /// </summary>
        public static NamingConvention WithNamingConvention(this IAssemblyScanner scanner)
        {
            return scanner.With<NamingConvention>();
        }

        /// <summary>
        /// Adds a convention to the scanner that registers several types implementing the same interface.
        /// By default it is registered with the name of the type, but this can be overridden.
        /// </summary>
        public static AddAllConvention WithAddAllConvention(this IAssemblyScanner scanner)
        {
            return scanner.With<AddAllConvention>();
        }

        /// <summary>
        /// Adds a convention to the scanner that looks for a writable property of a specified type
        /// and configures injection for it. Mostly useful for optional dependencies like loggers etc.
        /// </summary>
        public static SetAllPropertiesConvention WithSetAllPropertiesConvention(this IAssemblyScanner scanner)
        {
            return scanner.With<SetAllPropertiesConvention>();
        }
    }
}