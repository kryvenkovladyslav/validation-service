using System;
using System.Xml;
using Domain.Abstract;
using System.Xml.Schema;
using System.Threading.Tasks;
using BusinessLayer.Infrastructure;
using BusinessLayer.Implementation.Validators;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// The provider for creating validation settings for validation XML document against DTD
    /// </summary>
    public class DtdValidationSettingsProvider : ValidationSettingsProvider, IValidationXmlSettingProvider<DtdDocumentValidationStrategy>
    {
        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="fileStorage">The required file storage implementation</param>
        public DtdValidationSettingsProvider(IFileStorage fileStorage) : base(fileStorage) { }

        /// <summary>
        /// Asynchronously creates settings for XML validation against DTD
        /// </summary>
        /// <param name="documentFullPath">A full path to a specified XML document</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Validation settings for validation XML document against DTD</returns>
        public virtual Task<XmlReaderSettings> CreateXmlReaderSettingsAsync(string documentFullPath, Action<object, ValidationEventArgs> eventHandler)
        {
            var settings = new XmlReaderSettings
            {
                Async = true,
                XmlResolver = new CloudXmlUrlResolver(this.FileStorage, documentFullPath),
                ValidationType = ValidationType.DTD,
                DtdProcessing = DtdProcessing.Parse,
            };
            settings.ValidationEventHandler += new ValidationEventHandler(eventHandler);

            return Task.FromResult(settings);
        }
    }
}