namespace BusinessLayer.Configuration.Options
{
    /// <summary>
    /// The options for configuring Azure credentials authentication
    /// </summary>
    public sealed class AzureIdentityConfigurationOptions
    {
        /// <summary>
        /// A required name of options settings in appsettings
        /// </summary>
        public static string Position { get; } = nameof(AzureIdentityConfigurationOptions);

        /// <summary>
        /// A required key for accessing the values
        /// </summary>
        public string FullyQualifiedNamespace { get; init; }
    }
}