using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.ExternalResourceProviders
{
    /// <summary>
    /// Provides methods for extracting some external resources
    /// </summary>
    public interface IExternalResourceProvider
    {
        /// <summary>
        /// Asynchronously extracts required resource
        /// </summary>
        /// <param name="resourceFullPath">A path to a required resource</param>
        /// <returns>Created stream representing extracted resource</returns>
        public Task<Stream> ProvideResourceAsync(string resourceFullPath);

        /// <summary>
        /// Asynchronously checks if the specified resource exists
        /// </summary>
        /// <param name="resourceFullPath">A path to a required resource</param>
        /// <returns>True if the specified resource exists, otherwise False</returns>
        public Task<bool> ResourceExistsAsync(string resourceFullPath);
    }

    /// <summary>
    /// Provides methods for extracting some external resources from Azure
    /// </summary>
    public interface IAzureExternalResourceProvider : IExternalResourceProvider { }

    /// <summary>
    /// Provides methods for extracting external resources from Web Root Directory
    /// </summary>
    public interface IWebRootExternalResourceProvider : IExternalResourceProvider { }

    /// <summary>
    /// Provides methods for extracting XML external resources
    /// </summary>
    public interface IExternalXmlResourceProvider : IExternalResourceProvider { }

    /// <summary>
    /// Provides methods for extracting external DTD
    /// </summary>
    public interface IExternalDtdProvider : IExternalXmlResourceProvider { }

    /// <summary>
    /// Provides methods for extracting external XML Schema
    /// </summary>
    public interface IExternalXmlSchemaProvider : IExternalXmlResourceProvider { }

    /// <summary>
    /// Provides methods for extracting external DTD from Azure
    /// </summary>
    public interface IAzureExternalDtdProvider : IAzureExternalResourceProvider, IExternalDtdProvider { }

    /// <summary>
    /// Provides methods for extracting external XML Schema from Azure
    /// </summary>
    public interface IAzureExternalXmlSchemaProvider : IAzureExternalResourceProvider, IExternalXmlSchemaProvider { }

    /// <summary>
    /// Provides methods for extracting external XML Schema from Web Root Directory
    /// </summary>
    public interface IWebRootExternalXmlSchemaProvider : IWebRootExternalResourceProvider, IExternalXmlSchemaProvider { }
}