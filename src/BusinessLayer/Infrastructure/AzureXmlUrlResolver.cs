using System;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using BusinessLayer.Defaults;
using BusinessLayer.Implementation.ExternalResourceProviders;
using BusinessLayer.Infrastructure.Exceptions;

namespace BusinessLayer.Infrastructure
{
    /// <summary>
    /// Provider functionality to find unresolved DTD while validating XML Document
    /// </summary>
    public sealed class AzureXmlUrlResolver : XmlUrlResolver
    {
        /// <summary>
        /// The directory for storing XML Documents
        /// </summary>
        private readonly string rootWorkingPath;

        /// <summary>
        /// The provider for extracting external resources
        /// </summary>
        private readonly IExternalResourceProvider externalResourceProvider;

        /// <summary>
        /// Initializes the class using IExternalResourceProvider and current root working directory of the document
        /// </summary>
        /// <param name="externalResourceProvider">The service for extracting resources</param>
        /// <param name="rootWorkingPath">The directory for storing XML Documents</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if service on of the services is not provided</exception>
        public AzureXmlUrlResolver(IExternalResourceProvider externalResourceProvider, string rootWorkingPath)
        {
            ArgumentNullException.ThrowIfNull(rootWorkingPath, nameof(rootWorkingPath));

            this.externalResourceProvider = externalResourceProvider ?? throw new ArgumentNullException((nameof(externalResourceProvider)));
            this.rootWorkingPath = this.GetEctdWorkingDirectory(rootWorkingPath);
        }

        /// <summary>
        /// Asynchronously gets unresolved resource for XML validation against DTD
        /// </summary>
        /// <returns>Resolved resource for XML validation against DTD</returns>
        /// <exception cref="ResourceNotFoundException">ResourceNotFoundException is thrown if the resource was not found</exception>
        public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            var relativePath = this.GetDtdWorkingDirectory(absoluteUri.AbsolutePath);
            var fullPath = Path.Combine(this.rootWorkingPath, relativePath);

            var dtdExists = await this.externalResourceProvider.ResourceExistsAsync(fullPath);
            ResourceNotFoundException.ThrowIf(!dtdExists, fullPath);

            var resolvedDtd = await this.externalResourceProvider.ProvideResourceAsync(fullPath);
            return resolvedDtd;
        }

        private string GetEctdWorkingDirectory(string input)
        {
            int indexOfSlash = input.IndexOf(DefaultsEctd.Slash);
            return input.Substring(0, indexOfSlash + 6);
        }

        private string GetDtdWorkingDirectory(string input)
        {
            var lastIndex = input.LastIndexOf(DefaultsEctd.UtilSegment);
            return input.Substring(lastIndex);
        }
    }
}