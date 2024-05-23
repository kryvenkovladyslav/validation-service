using System.Xml;
using BusinessLayer.Infrastructure;
using BusinessLayer.Implementation.ExternalResourceProviders;
using Domain.Abstract;
using BusinessLayer.Implementation.ValidationStrategies;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// Provides settings for validating XML Document against DTD resource from Azure Blob Storage
    /// </summary>
    public class AzureDtdValidationSettingsProvider : DtdValidationSettingsProvider<DtdXmlDocumentValidationStrategy>
    {
        /// <summary>
        /// Initializes the class using IAzureExternalDtdProvider
        /// </summary>
        /// <param name="azureExternalDtdProvider">The provider for providing external DTD resource from Azure Blob Storage</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if one of the arguments is not provided</exception>
        public AzureDtdValidationSettingsProvider(IAzureExternalDtdProvider azureExternalDtdProvider, IDtdValidationSettingsCreator settingsCreator) 
            : base(azureExternalDtdProvider, settingsCreator) { }

        protected override XmlUrlResolver GetXmlUrlResolverForCurrentDocumentResolver(string documentFullPath)
        {
            return new AzureXmlUrlResolver(this.ExternalXmlResourceProvider, documentFullPath);
        }
    }
}