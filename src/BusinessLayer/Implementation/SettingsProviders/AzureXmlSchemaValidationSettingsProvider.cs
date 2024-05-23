using System.IO;
using System.Xml;
using Domain.Abstract;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using BusinessLayer.Implementation.ExternalResourceProviders;
using BusinessLayer.Infrastructure.Exceptions;
using BusinessLayer.Defaults;
using BusinessLayer.Implementation.ValidationStrategies;

namespace BusinessLayer.Implementation.SettingsProviders
{
    /// <summary>
    /// Provides settings for validating XML Document against XML Schema resource from Azure Blob Storage
    /// </summary>
    public class AzureXmlSchemaValidationSettingsProvider : XmlSchemaValidationSettingsProvider<SchemaXmlDocumentValidationStrategy>
    {
        /// <summary>
        /// Initializes the class using IAzureExternalXmlSchemaProvider, IDocumentStorage and IAzureXmlSchemaPathResolver
        /// </summary>
        /// <param name="azureExternalXmlSchemaProvider">The provider for providing external XML Schema resource from Azure</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the one of the arguments is not provided</exception>
        public AzureXmlSchemaValidationSettingsProvider(IAzureExternalXmlSchemaProvider azureExternalXmlSchemaProvider,
            IXmlSchemaValidationSettingsCreator settingsCreator, IAzureDocumentStorage documentStorage) 
            : base(azureExternalXmlSchemaProvider, settingsCreator, documentStorage) { }

        /// <summary>
        /// Asynchronously restores all XML Schemas for a specified document from Azure
        /// </summary>
        /// <param name="documentFullPath">A path to a document requires extracting XML Schema resource</param>
        /// <param name="documentStream">A stream representing a current document</param>
        /// <param name="dependencies">A collection for storing restored dependencies</param>
        protected override async Task RestoreXmlSchemasAsync(string documentFullPath, Stream documentStream, Dictionary<string, Stream> dependencies)
        {
            var document = this.CreateDocument(documentStream);

            using var schemas = this.GetDocumentSchemas(document);

            foreach (XmlNode location in schemas)
            {
                var schemaPath = Path.Combine(this.GetEctdRelativeWorkingDirectory(documentFullPath), this.GetSchemaRelativeWorkingDirectory(location.Value));

                var schemaExists = await this.ExternalXmlResourceProvider.ResourceExistsAsync(schemaPath);

                if (schemaExists && !dependencies.ContainsKey(schemaPath))
                {
                    var schemaStream = await this.ExternalXmlResourceProvider.ProvideResourceAsync(schemaPath);
                    dependencies.TryAdd(schemaPath, schemaStream);
                    await this.RestoreXmlSchemasAsync(schemaPath, schemaStream, dependencies);
                }
            }
        }

        private string GetSchemaRelativeWorkingDirectory(string input)
        {
            var lastIndex = input.LastIndexOf(DefaultsEctd.Slash);
            return lastIndex != -1 ? input.Substring(lastIndex).Replace(DefaultsEctd.Slash, string.Empty) : input;
        }

        private string GetEctdRelativeWorkingDirectory(string input)
        {
            int indexOfSlash = input.IndexOf(DefaultsEctd.Slash);
            string result = input.Substring(0, indexOfSlash + 6);
            return string.Concat(result, DefaultsEctd.UtilDtdPath);
        }
    }
}