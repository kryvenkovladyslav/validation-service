using Xunit;
using AzureBlobStorage.Options;
using Microsoft.Extensions.Options;
using BusinessLayerTests.Factories;
using BusinessLayer.Configuration.Options;
using BusinessLayer.Configuration.OptionsSetup;

namespace BusinessLayerTests.OptionsTests
{
    public sealed class AzureIdentityOptionsTests
    {
        private readonly IOptionsMonitorFactory<AzureIdentityConfigurationOptions> optionsFactory;

        public AzureIdentityOptionsTests()
        {
            this.optionsFactory = new OptionsMonitorFactory<AzureIdentityConfigurationOptions>();
        }

        [Fact]
        public void AzureIdentityConfigurationOptions_HasActualPositionValue_ReturnsTrue()
        {
            Assert.True(!string.IsNullOrEmpty(AzureIdentityConfigurationOptions.Position));
        }

        [Theory]
        [InlineData("FullyQualifiedNamespace")]
        public void Configure_WithProvidedOptionsMonitor_ConfiguresRequiredObject(string fullyQualifiedNamespace)
        {
            var requiredOptions = new AzureIdentityOptions();
            var accessKeyOptionsSetup = new AzureIdentityOptionsSetup(this.CreateAzureIdentityOptionsMonitor(fullyQualifiedNamespace));

            accessKeyOptionsSetup.Configure(requiredOptions);

            Assert.NotNull(requiredOptions.FullyQualifiedNamespace);
            Assert.Equal(fullyQualifiedNamespace, requiredOptions.FullyQualifiedNamespace);
        }

        private IOptionsMonitor<AzureIdentityConfigurationOptions> CreateAzureIdentityOptionsMonitor(string fullyQualifiedNamespace)
        {
            var configurationOptions = new AzureIdentityConfigurationOptions { FullyQualifiedNamespace = fullyQualifiedNamespace };
            return this.optionsFactory.Create(configurationOptions);
        }
    }
}