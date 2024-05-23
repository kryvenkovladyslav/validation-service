using BusinessLayer.Defaults;
using BusinessLayer.Implementation.ExternalResourceProviders;
using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// Provides settings for validating XML Document against XML Schema resource
    /// </summary>
    /// <typeparam name="TStrategy">The strategy for indicating if XML Document can be processed against XML Schema by the validator</typeparam>
    public abstract class XmlSchemaValidationSettingsProvider<TStrategy> : XmlValidationSettingsProvider<TStrategy>
        where TStrategy : IXmlDocumentValidationStrategy
    {
        /// <summary>
        /// Creates setting for validation XML document against XML Schema
        /// </summary>
        private readonly IXmlSchemaValidationSettingsCreator settingsCreator;

        /// <summary>
        /// The service for extracting XML document from a storage
        /// </summary>
        public IDocumentStorage DocumentStorage { protected get; init; }

        /// <summary>
        /// Initializes the class using IExternalXmlSchemaProvider and IDocumentStorage
        /// </summary>
        /// <param name="externalXmlSchemaProvider">The provider for providing external XML Schema resource</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if on of the arguments is not provided</exception>
        public XmlSchemaValidationSettingsProvider(IExternalXmlSchemaProvider externalXmlSchemaProvider,
            IXmlSchemaValidationSettingsCreator settingsCreator, IDocumentStorage documentStorage) : base(externalXmlSchemaProvider)
        {
            this.settingsCreator = settingsCreator ?? throw new ArgumentNullException(nameof(settingsCreator));
            this.DocumentStorage = documentStorage ?? throw new ArgumentNullException(nameof(documentStorage));
        }

        /// <summary>
        /// Asynchronously creates XML settings for XML Reader for XML Schema validation purposes 
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting XML Schema resource</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Created setting for XML Reader</returns>
        public override async Task<XmlReaderSettings> CreateSettingsAsync(string documentFullPath, Action<object, ValidationEventArgs> eventHandler)
        {
            var handler = new ValidationEventHandler(eventHandler);
            var schemas = await this.RestoreDependenciesAsync(documentFullPath);
            var schemaSet = this.GetXmlSchemaSet(schemas, handler);

            var settings = this.settingsCreator.CreateValidationSettings(schemaSet, eventHandler);
            return settings;
        }

        /// <summary>
        /// Asynchronously restores all dependencies mentioned in a document
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting XML Schema resources</param>
        /// <returns>A collection of restored external resources for a document</returns>
        protected virtual async Task<IEnumerable<Stream>> RestoreDependenciesAsync(string documentFullPath)
        {
            var dependencies = new Dictionary<string, Stream>();

            using var documentStream = await this.DocumentStorage.FindByFullPathAsync(documentFullPath);
            await this.RestoreXmlSchemasAsync(documentFullPath, documentStream, dependencies);
            return dependencies.Values;
        }

        /// <summary>
        /// Creates XmlDocument for preparation stage 
        /// </summary>
        /// <param name="stream">The stream represents the XML document for validation process</param>
        /// <returns>Created XML Document using the stream</returns>
        protected virtual XmlDocument CreateDocument(Stream stream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return xmlDocument;
        }

        /// <summary>
        /// Gets all XML Schemas mentioned in provided XML Document
        /// </summary>
        /// <param name="document">The document represents the XML document for validation process</param>
        /// <returns>Created a list of XML Schemas</returns>
        protected virtual XmlNodeList GetDocumentSchemas(XmlDocument document)
        {
            var root = document.DocumentElement;
            return root.SelectNodes(DefaultsEctd.SchemaXPath);
        }

        /// <summary>
        /// Provides created XML Schema Set for setting up the required schemas for XML Reader
        /// </summary>
        /// <param name="streams">A collection of XML Schemas</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Created Set with all required XML Schemas</returns>
        protected virtual XmlSchemaSet GetXmlSchemaSet(IEnumerable<Stream> streams, ValidationEventHandler eventHandler)
        {
            var schemaSet = new XmlSchemaSet();
            string baseNamespace = string.Empty;

            foreach (var stream in streams)
            {
                var xmlSchema = XmlSchema.Read(stream, eventHandler);
                stream.Seek(0, SeekOrigin.Begin);

                baseNamespace = xmlSchema.TargetNamespace ?? baseNamespace;
                schemaSet.Add(baseNamespace, XmlReader.Create(stream));
            }

            return schemaSet;
        }

        /// <summary>
        /// Asynchronously restores all XML Schemas for a specified document
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting XML Schema resource</param>
        /// <param name="documentStream">A stream representing a current document</param>
        /// <param name="dependencies">A collection for storing restored dependencies</param>
        protected abstract Task RestoreXmlSchemasAsync(string documentFullPath, Stream documentStream, Dictionary<string, Stream> dependencies);
    }
}