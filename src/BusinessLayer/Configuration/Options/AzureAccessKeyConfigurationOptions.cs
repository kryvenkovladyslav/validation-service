namespace BusinessLayer.Configuration.Options
{
    /// <summary>
    /// The options for configuring Azure Access Key authentication
    /// </summary>
    public sealed class AzureAccessKeyConfigurationOptions
    {
        /// <summary>
        /// A required name of options settings in appsettings
        /// </summary>
        public static string Position { get; } = nameof(AzureAccessKeyConfigurationOptions);

        /// <summary>
        /// A required key for accessing the values
        /// </summary>
        public string AccessKey { get; init; }
    }
}