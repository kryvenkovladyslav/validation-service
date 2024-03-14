using AzureBlobStorage.Abstract;

namespace BusinessLayer.Models
{
    /// <summary>
    /// The model representing a request to Azure Blob
    /// </summary>
    public sealed class BlobRequestModel : IBlobRequestModel
    {
        /// <summary>
        /// The name of a container on Azure Blob
        /// </summary>
        public string ContainerName { get; init; }

        /// <summary>
        /// The full path to a file on Azure Blob without a container name
        /// </summary>
        public string RequiredFilePath { get; init; }
    }
}