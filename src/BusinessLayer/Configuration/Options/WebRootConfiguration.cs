using System.Collections.Generic;

namespace BusinessLayer.Configuration.Options
{
    /// <summary>
    /// Provides a structure to read appsetings for configuring dependencies 
    /// </summary>
    public sealed class WebRootConfiguration
    {
        /// <summary>
        /// Represents the name of the the object
        /// </summary>
        public static string Position { get; } = nameof(WebRootConfiguration);

        /// <summary>
        /// Represents the key for getting the value
        /// </summary>
        public IEnumerable<string> Directories { get; init; }

        /// <summary>
        /// Represents the key for getting the value
        /// </summary>
        public string RootDirectory { get; init; }
    }
}