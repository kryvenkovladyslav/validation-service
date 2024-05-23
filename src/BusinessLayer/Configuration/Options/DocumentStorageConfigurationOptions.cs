namespace BusinessLayer.Configuration.Options
{
    /// <summary>
    /// The options for configuring Cloud File Storage
    /// </summary>
    public sealed class DocumentStorageConfigurationOptions
    {
        /// <summary>
        /// A required name of options settings in appsettings
        /// </summary>
        public static string Position { get; } = nameof(DocumentStorageConfigurationOptions);

        /// <summary>
        /// A required key for accessing the values
        /// </summary>
        public string ContainerName { get; init; }
    }
}