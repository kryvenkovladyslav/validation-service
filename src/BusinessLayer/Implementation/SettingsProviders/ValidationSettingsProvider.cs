using System;
using Domain.Abstract;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// The abstract provider for creating validation settings for validation XML document
    /// </summary>
    public abstract class ValidationSettingsProvider
    {
        /// <summary>
        /// The required file storage implementation
        /// </summary>
        public virtual IFileStorage FileStorage { protected get; init; }

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="fileStorage">The required file storage implementation</param>
        public ValidationSettingsProvider(IFileStorage fileStorage)
        {
            this.FileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        }
    }
}