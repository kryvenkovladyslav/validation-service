using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using BusinessLayer.Implementation.ExternalResourceProviders;
using Domain.Abstract;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// Provides settings for validating XML Document against XML external resources
    /// </summary>
    /// <typeparam name="TStrategy">The strategy for indicating if XML Document can be processed by the validator</typeparam>
    public abstract class XmlValidationSettingsProvider<TStrategy> : IValidationXmlSettingProvider<TStrategy> 
        where TStrategy : IXmlDocumentValidationStrategy
    {
        /// <summary>
        /// The provider for providing external XML resource
        /// </summary>
        public virtual IExternalXmlResourceProvider ExternalXmlResourceProvider { protected get; init; }

        /// <summary>
        /// Initializes the class using IExternalXmlResourceProvider
        /// </summary>
        /// <param name="externalXmlResourceProvider">The provider for providing external XML resource</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the externalXmlResourceProvider is not provided</exception>
        public XmlValidationSettingsProvider(IExternalXmlResourceProvider externalXmlResourceProvider)
        {
            this.ExternalXmlResourceProvider = externalXmlResourceProvider ?? throw new ArgumentNullException(nameof(externalXmlResourceProvider));
        }

        /// <summary>
        /// Asynchronously creates XML settings for XML Reader for validation purposes 
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting XML resources</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Created setting for XML Reader</returns>
        public abstract Task<XmlReaderSettings> CreateSettingsAsync(string documentFullPath, Action<object, ValidationEventArgs> eventHandler);
    }
}