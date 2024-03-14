using System;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    /// <summary>
    /// The provider for creating validation settings 
    /// </summary>
    /// <typeparam name="TValidator">A type of validator that accepts a provided settings</typeparam>
    /// <typeparam name="TSetting">A type of validation settings to be provided</typeparam>
    /// <typeparam name="TEventArgs">A type for handling validation errors</typeparam>
    public interface IValidationSettingProvider<out TValidator, TSetting, TEventArgs>
        where TSetting : class
        where TEventArgs : EventArgs
        where TValidator : IDocumentValidator
    {
        /// <summary>
        /// Asynchronously creates validation settings
        /// </summary>
        /// <returns>Created validation settings for specified file</returns>
        public Task<TSetting> CreateXmlReaderSettingsAsync(string documentFullPath, Action<object, TEventArgs> eventHandler);
    }
}