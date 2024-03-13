using Azure.Storage.Blobs;

namespace AzureBlobStorage.Abstract
{
    /// <summary>
    /// The interface for authenticating the Azure Blob Client 
    /// </summary>
    public interface IAzureBlobAuthenticator
    {
        /// <summary>
        /// Authenticates the Azure Blob Client
        /// </summary>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        public BlobServiceClient AuthenticateClient();
    }
}