using System;
using System.IO;
using System.Xml;
using Domain.Abstract;
using System.Xml.Schema;
using System.Threading.Tasks;
using BusinessLayer.Defaults;
using System.Collections.Generic;
using BusinessLayer.Implementation.Validators;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// The provider for creating validation settings for validation XML document against XML Schema
    /// </summary>
    public class SchemaValidationSettingsProvider : ValidationSettingsProvider, IValidationXmlSettingProvider<SchemaDocumentValidationStrategy>
    {
        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="fileStorage">The required file storage implementation</param>
        public SchemaValidationSettingsProvider(IFileStorage fileStorage) : base(fileStorage) { }

        /// <summary>
        /// Asynchronously creates settings for XML validation against XML Schema
        /// </summary>
        /// <param name="documentFullPath">A full path to a specified XML document</param>
        /// <param name="eventHandler">A handler for handling any kind of validation errors</param>
        /// <returns>Validation settings for validation XML document against XML Schema</returns>
        public virtual async Task<XmlReaderSettings> CreateXmlReaderSettingsAsync(string documentFullPath, Action<object, ValidationEventArgs> eventHandler)
        {
            var handler = new ValidationEventHandler(eventHandler);
            var schemas = await this.RestoreDependenciesAsync(documentFullPath);

            var settings = new XmlReaderSettings();
            settings.Async = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += handler;

            foreach (var current in schemas)
            {
                settings.Schemas.Add(XmlSchema.Read(current, handler));
            }

            return settings;
        }

        /// <summary>
        /// Asynchronously restores all required dependencies for validation XML document against XML Schema
        /// </summary>
        /// <param name="documentFullPath">A full path to XML document</param>
        /// <returns>Restored XML schemas for the required XML document</returns>
        protected virtual async Task<IEnumerable<Stream>> RestoreDependenciesAsync(string documentFullPath)
        {
            var dependencies = new Dictionary<string, Stream>();

            using var document = await this.FileStorage.FindByFullPathAsync(documentFullPath);
            await this.RestoreXmlSchemasAsync(documentFullPath, document, dependencies);
            return dependencies.Values;
        }

        /// <summary>
        /// Asynchronously restores all schemas for the required XML document
        /// </summary>
        /// <param name="documentFullPath">A current XML document</param>
        /// <param name="documentStream">A current document stream</param>
        /// <param name="dependencies">A collection with all dependencies</param>
        protected virtual async Task RestoreXmlSchemasAsync(string documentFullPath, Stream documentStream, Dictionary<string, Stream> dependencies)
        {
            var document = this.CreateDocument(documentStream);

            using var schemasLocation = this.GetDocumentSchemas(document);

            foreach (XmlNode location in schemasLocation)
            {
                var newPath = Path.Combine(this.GetEctdRelativeWorkingDirectory(documentFullPath), this.GetShemaRelativeWorkingDirectory(location.Value));
                
                if(await this.FileStorage.FindExistsAsync(newPath))
                {
                    var schemaStream = await this.FileStorage.FindByFullPathAsync(newPath);
                    dependencies.TryAdd(newPath, schemaStream);
                    await this.RestoreXmlSchemasAsync(newPath, schemaStream, dependencies);
                }
            }
        }

        /// <summary>
        /// Creates a XML document from a specified stream
        /// </summary>
        /// <param name="stream">A required document stream</param>
        /// <returns>Created XML document</returns>
        private XmlDocument CreateDocument(Stream stream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            stream.Position = 0;

            return xmlDocument;
        }

        /// <summary>
        /// Gets relative working directory on Azure Blob for validation schema
        /// </summary>
        private string GetShemaRelativeWorkingDirectory(string input)
        {
            var lastIndex = input.LastIndexOf(DefaultsEctd.Slash);
            return lastIndex != -1 ? input.Substring(lastIndex).Replace(DefaultsEctd.Slash, string.Empty) : input;
        }

        /// <summary>
        /// Gets relative root working directory on Azure Blob for a container
        /// </summary>
        private string GetEctdRelativeWorkingDirectory(string input)
        {
            int indexOfSlash = input.IndexOf(DefaultsEctd.Slash);
            string result = input.Substring(0, indexOfSlash + 6);
            return string.Concat(result, DefaultsEctd.UtilDtdPath);
        }

        /// <summary>
        /// Gets all schemas for a current document
        /// </summary>
        /// <param name="document">A required document</param>
        /// <returns>A list of XML nodes</returns>
        private XmlNodeList GetDocumentSchemas(XmlDocument document)
        {
            var root = document.DocumentElement;
            return root.SelectNodes(DefaultsEctd.SchemaXPath);
        }
    }
}