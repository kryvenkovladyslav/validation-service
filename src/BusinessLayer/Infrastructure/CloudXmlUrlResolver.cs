using System;
using System.IO;
using System.Xml;
using Domain.Abstract;
using System.Threading.Tasks;
using BusinessLayer.Defaults;

namespace BusinessLayer.Infrastructure
{
    /// <summary>
    /// The resolver that finds a specified DTD file in a XML file
    /// </summary>
    public sealed class CloudXmlUrlResolver : XmlUrlResolver
    {
        /// <summary>
        /// The current root directory on Azure Blob
        /// </summary>
        private readonly string rootWorkingPath;

        /// <summary>
        /// The implementation of file storage
        /// </summary>
        private readonly IFileStorage fileStorage;

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <exception cref="ArgumentNullException">The ArgumentNullException is thrown if storage reference in null or root working directory in null or empty</exception>
        public CloudXmlUrlResolver(IFileStorage fileStorage, string rootWorkingPath)
        {
            ArgumentNullException.ThrowIfNull(rootWorkingPath, nameof(rootWorkingPath));

            this.fileStorage = fileStorage ?? throw new ArgumentNullException((nameof(fileStorage)));
            this.rootWorkingPath = this.GetEctdWorkingDirectory(rootWorkingPath);
        }

        /// <summary>
        /// Asynchronously gets a required DTD object for current XML document
        /// </summary>
        /// <returns>Provided DTD document stream</returns>
        public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            var relativePath = this.GetDtdWorkingDirectory(absoluteUri.AbsolutePath);
            var fullPath = Path.Combine(this.rootWorkingPath, relativePath);
            
            var resolvedDtd = await this.fileStorage.FindByFullPathAsync(fullPath);
            return resolvedDtd;
        }

        private string GetEctdWorkingDirectory(string input)
        {
            int indexOfSlash = input.IndexOf(DefaultsEctd.Slash);
            string result = input.Substring(0, indexOfSlash + 6);
            return result;
        }

        private string GetDtdWorkingDirectory(string input)
        {
            var lastIndex = input.LastIndexOf(DefaultsEctd.UtilSegment);
            return lastIndex != -1 ? input.Substring(lastIndex) : input;
        }
    }
}