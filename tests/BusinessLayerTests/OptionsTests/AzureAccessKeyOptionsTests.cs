using Xunit;
using AzureBlobStorage.Options;
using Microsoft.Extensions.Options;
using BusinessLayerTests.Factories;
using BusinessLayer.Configuration.Options;
using BusinessLayer.Configuration.OptionsSetup;

namespace BusinessLayerTests.OptionsTests
{
    public sealed class AzureAccessKeyOptionsTests
    {
        private readonly IOptionsMonitorFactory<AzureAccessKeyConfigurationOptions> optionsFactory;

        public AzureAccessKeyOptionsTests()
        {
            this.optionsFactory = new OptionsMonitorFactory<AzureAccessKeyConfigurationOptions>();  
        }

        [Fact]
        public void AzureAccessKeyConfigurationOptions_HasActualPositionValue_ReturnsTrue()
        {
            Assert.True(!string.IsNullOrEmpty(AzureAccessKeyConfigurationOptions.Position));
        }

        [Theory]
        [InlineData("SuperPrivateAccessKeyExample")]
        public void Configure_WithProvidedOptionsMonitor_ConfiguresRequiredObject(string accessKey)
        {
            var requiredOptions = new AzureAccessKeyOptions();
            var accessKeyOptionsSetup = new AzureAccessKeyOptionsSetup(this.CreateAzureAccessKeyOptionsMonitor(accessKey));

            accessKeyOptionsSetup.Configure(requiredOptions);

            Assert.NotNull(requiredOptions.AccessKey);
            Assert.Equal(accessKey, requiredOptions.AccessKey);
        }

        private IOptionsMonitor<AzureAccessKeyConfigurationOptions> CreateAzureAccessKeyOptionsMonitor(string accessKey)
        {
            var configurationOptions = new AzureAccessKeyConfigurationOptions { AccessKey = accessKey };
            return this.optionsFactory.Create(configurationOptions);
        }
    }
}