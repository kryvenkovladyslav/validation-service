﻿using AzureBlobStorage.Options;
using BusinessLayer.Configuration.Options;
using Microsoft.Extensions.Options;
using System;

namespace BusinessLayer.Configuration.OptionsSetup
{
    /// <summary>
    /// The class for setting up options for external Azure Authenticator
    /// </summary>
    public sealed class AzureIdentityOptionsSetup : IConfigureOptions<AzureIdentityOptions>
    {
        /// <summary>
        /// The options for configuring Azure credentials
        /// </summary>
        private readonly AzureIdentityConfigurationOptions configuration;

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="configuration">The options for configuring Azure Access Key</param>
        public AzureIdentityOptionsSetup(IOptionsMonitor<AzureIdentityConfigurationOptions> configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            this.configuration = configuration.CurrentValue ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configures external Azure Access Key options
        /// </summary>
        public void Configure(AzureIdentityOptions options)
        {
            options.FullyQualifiedNamespace = this.configuration.FullyQualifiedNamespace;
        }
    }
}