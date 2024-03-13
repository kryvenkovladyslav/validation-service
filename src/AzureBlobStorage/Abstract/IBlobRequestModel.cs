namespace AzureBlobStorage.Abstract
{
    /// <summary>
    /// The interface representing a request to Azure Blob Storage
    /// </summary>
    public interface IBlobRequestModel
    {
        /// <summary>
        /// The name of a container on Azure Blob Storage
        /// </summary>
        public string ContainerName { get; init; }

        /// <summary>
        /// The full path to a required file on Azure Blob Storage
        /// </summary>
        public string RequiredFilePath { get; init; }
    }
}