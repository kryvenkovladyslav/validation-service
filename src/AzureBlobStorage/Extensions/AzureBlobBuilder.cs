using System;
using AzureBlobStorage.Options;
using AzureBlobStorage.Abstract;
using AzureBlobStorage.Authenticators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AzureBlobStorage.Extensions
{
    /// <summary>
    /// The builder for configuring all necessary dependencies of Azure Blob Service 
    /// </summary>
    public sealed class AzureBlobBuilder
    {
        private readonly IServiceCollection services;

        /// <summary>
        /// The constructor for creating an instance
        /// </summary>
        /// <exception cref="ArgumentNullException">The ArgumentNullException is thrown if services is null</exception>
        public AzureBlobBuilder(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Adds a process of authentication via Azure access key
        /// </summary>
        public AzureBlobBuilder AddAzureAccessKeyAuthentication()
        {
            this.AddScopedService<IAzureBlobAuthenticator, AzureAccessKeyBlobAuthenticator>();
            return this;
        }

        /// <summary>
        /// Adds a process of authentication via Azure access key using access key options
        /// </summary>
        /// <param name="azureAccessKeyOptions">The options for configuring access key options</param>
        public AzureBlobBuilder AddAzureAccessKeyAuthentication(Action<AzureAccessKeyOptions> azureAccessKeyOptions)
        {
            this.services.Configure(azureAccessKeyOptions);
            this.AddAzureAccessKeyAuthentication();
            return this;
        }

        /// <summary>
        /// Adds a process of authentication via Azure credentials
        /// </summary>
        public AzureBlobBuilder AddAzureCredentialsBlobAuthentication()
        {
            this.AddScopedService<IAzureBlobAuthenticator, AzureCredentialsBlobAuthenticator>();
            return this;
        }

        /// <summary>
        /// Adds a process of authentication via Azure credentials
        /// </summary>
        /// <param name="azureIdentityOptions">The options for configuring credentials options</param>
        public AzureBlobBuilder AddAzureCredentialsBlobAuthentication(Action<AzureIdentityOptions> azureIdentityOptions)
        {
            this.services.Configure(azureIdentityOptions);
            this.AddAzureCredentialsBlobAuthentication();
            return this;
        }

        /// <summary>
        /// Adds a scoped service for the interface
        /// </summary>
        /// <typeparam name="TInterface">The required interfaces</typeparam>
        /// <typeparam name="TImplementation">The provided implementation</typeparam>
        private void AddScopedService<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.RemoveServices<TInterface>();
            this.services.TryAddScoped<TInterface, TImplementation>();
        }

        /// <summary>
        /// Removes all implementations for specified interface
        /// </summary>
        /// <typeparam name="TService">The required interface</typeparam>
        private void RemoveServices<TService>()
        {
            this.services.RemoveAll(typeof(TService));
        }
    }
}