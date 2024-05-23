using BusinessLayer.Implementation.ExternalResourceProviders;
using Domain.Abstract;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// Provides settings for validating XML Document against DTD resource
    /// </summary>
    /// <typeparam name="TStrategy">The strategy for indicating if XML Document can be processed against DTD by the validator</typeparam>
    public abstract class DtdValidationSettingsProvider<TStrategy> : XmlValidationSettingsProvider<TStrategy>
        where TStrategy : IXmlDocumentValidationStrategy
    {
        /// <summary>
        /// Creates setting for validation XML document against DTD
        /// </summary>
        private readonly IDtdValidationSettingsCreator settingsCreator;

        /// <summary>
        /// Initializes the class using IExternalDtdProvider
        /// </summary>
        /// <param name="externalDtdProvider">The provider for providing external DTD resource</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the on of the arguments is not provided</exception>
        public DtdValidationSettingsProvider(IExternalDtdProvider externalDtdProvider, IDtdValidationSettingsCreator settingsCreator) : base(externalDtdProvider) 
        {
            this.settingsCreator = settingsCreator ?? throw new ArgumentNullException(nameof(settingsCreator));
        }

        /// <summary>
        /// Asynchronously creates XML settings for XML Reader for DTD validation purposes 
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting DTD resource</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Created setting for XML Reader</returns>
        public override Task<XmlReaderSettings> CreateSettingsAsync(string documentFullPath, Action<object, ValidationEventArgs> eventHandler)
        {
            var xmlUrlResolver = this.GetXmlUrlResolverForCurrentDocumentResolver(documentFullPath);

            var settings = this.settingsCreator.CreateValidationSettings(xmlUrlResolver, eventHandler);
            return Task.FromResult(settings);
        }

        /// <summary>
        /// Provides XML URL Resolver for extracting DTD resource
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting DTD resource</param>
        /// <returns>Created XML URL Resolver</returns>
        protected abstract XmlUrlResolver GetXmlUrlResolverForCurrentDocumentResolver(string documentFullPath);
    }
}