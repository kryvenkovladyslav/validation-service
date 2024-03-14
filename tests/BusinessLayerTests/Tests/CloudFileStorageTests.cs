using Moq;
using Xunit;
using System;
using System.IO;
using Domain.Abstract;
using System.Threading.Tasks;
using AzureBlobStorage.Abstract;
using AzureBlobStorage.Exceptions;
using BusinessLayer.Implementation;
using Microsoft.Extensions.Options;
using BusinessLayer.Configuration.Options;

namespace BusinessLayerTests.Tests
{
    public sealed class CloudFileStorageTests
    {
        private readonly Mock<IAzureBlobService> blobService;
        private readonly IOptionsMonitor<FileStoreConfigurationOptions> optionsMonitor;

        public CloudFileStorageTests()
        {
            this.blobService = new Mock<IAzureBlobService>();
            this.optionsMonitor = this.CreateFileStoreConfigurationOptions("ectd-123456");
        }

        [Fact]
        public void FileStoreConfigurationOptions_HasActualPositionValue_ReturnsTrue()
        {
            Assert.True(!string.IsNullOrEmpty(FileStoreConfigurationOptions.Position));
        }

        [Fact]
        public void CloudFileStorage_HasNullParameterInCtor_ThrowsArgumentNullException()
        {
            var creationWithoutFirstParameter = () => new CloudFileStorage(null, this.optionsMonitor);
            var creationWithoutSecondParameter = () => new CloudFileStorage(this.blobService.Object, null);

            Assert.Throws<ArgumentNullException>(creationWithoutFirstParameter);
            Assert.Throws<ArgumentNullException>(creationWithoutSecondParameter);
        }

        [Theory]
        [InlineData("valid0000/index.xml", false)]
        [InlineData("valid0000/0000/index.xml", true)]
        public async Task FindExistAsync_SearchesFileUsingProvidedFilePath_ReturnsAppropriateResult(string documentFilePath, bool expectedResult)
        {
            this.blobService.Setup(service => service.ExistsAsync(It.IsAny<IBlobRequestModel>(), default)).ReturnsAsync(expectedResult);
            var fileStorage = this.CreateCloudFileStorage();

            var result = await fileStorage.FindExistsAsync(documentFilePath);

            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("valid0000/0000/index.xml", "Any piece of text")]
        public async Task FindAsync_WithCorrectFilePathToExistingFile_ReturnNotEmptyStream(string documentFilePath, string value)
        {
            this.blobService.Setup(service => service.DownloadBlobAsync(It.IsAny<IBlobRequestModel>(), default)).ReturnsAsync(new BinaryData(value));
            var fileStorage = this.CreateCloudFileStorage();

            var result = await fileStorage.FindByFullPathAsync(documentFilePath);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Stream>(result);
        }

        [Theory]
        [InlineData("valid0000/index.xml")]
        public async Task FindAsync_WithIncorrectFilePathToExistingFile_ThrowsException(string documentFilePath)
        {
            this.blobService.Setup(service => service.DownloadBlobAsync(It.IsAny<IBlobRequestModel>(), default)).ThrowsAsync(new AzureBlobNotExistException());
            var fileStorage = this.CreateCloudFileStorage();

            var result = () => fileStorage.FindByFullPathAsync(documentFilePath);

            await Assert.ThrowsAsync<AzureBlobNotExistException>(result);
        }

        private IOptionsMonitor<FileStoreConfigurationOptions> CreateFileStoreConfigurationOptions(string containerName)
        {
            var optionsMonitor = new Mock<IOptionsMonitor<FileStoreConfigurationOptions>>();
            optionsMonitor.Setup(monitor => monitor.CurrentValue).Returns(new FileStoreConfigurationOptions { ContainerName = containerName });
            return optionsMonitor.Object;
        }

        private IFileStorage CreateCloudFileStorage()
        {
            return new CloudFileStorage(this.blobService.Object, this.optionsMonitor);
        }
    }
}