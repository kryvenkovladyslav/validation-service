using System;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml;

namespace Domain.Abstract
{
    /// <summary>
    /// The provider for creating validation settings 
    /// </summary>
    /// <typeparam name="TValidator">A type of validator that accepts a provided settings</typeparam>
    /// <typeparam name="TSetting">A type of validation settings to be provided</typeparam>
    /// <typeparam name="TEventArgs">A type for handling validation errors</typeparam>
    public interface IValidationSettingProvider<out TStrategy, TSettings, TEventArgs>
        where TSettings : class
        where TEventArgs : EventArgs
        where TStrategy : IDocumentValidationStrategy<TSettings>
    {
        /// <summary>
        /// Asynchronously creates validation settings
        /// </summary>
        /// <returns>Created validation settings for specified file</returns>
        public Task<TSettings> CreateSettingsAsync(string documentFullPath, Action<object, TEventArgs> eventHandler);
    }

    public interface IValidationXmlSettingProvider<out TStrategy> : IValidationSettingProvider<TStrategy, XmlReaderSettings, ValidationEventArgs>
        where TStrategy : IXmlDocumentValidationStrategy
    { }
}