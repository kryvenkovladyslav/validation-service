namespace AzureBlobStorage.Exceptions
{
    /// <summary>
    /// A static class for holding all exception messages
    /// </summary>
    public static class ExceptionMessages
    {
        /// <summary>
        /// The message for AzureBlobNotExistException
        /// </summary>
        public static string BlobDoesNotExistMessage { get; private set; } = "The file does not exist on Azure Storage or file path is incorrect";
    }
}