namespace BusinessLayer.Configuration.Options
{
    /// <summary>
    /// Provides a structure to read appsetings for configuring dependencies 
    /// </summary>
    public sealed class AzureExternalResourceProviderConfigurationOptions
    {
        /// <summary>
        /// Represents the name of the the object
        /// </summary>
        public static string Position { get; } = nameof(AzureExternalResourceProviderConfigurationOptions);

        /// <summary>
        /// Represents the key for getting the value
        /// </summary>
        public string ContainerName { get; init; }
    }
}