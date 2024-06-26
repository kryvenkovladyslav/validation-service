﻿using AzureBlobStorage.Options;
using BusinessLayer.Configuration.Options;
using Microsoft.Extensions.Options;
using System;

namespace BusinessLayer.Configuration.OptionsSetup
{
    /// <summary>
    /// The class for setting up options for external Azure Authenticator
    /// </summary>
    public sealed class AzureAccessKeyOptionsSetup : IConfigureOptions<AzureAccessKeyOptions>
    {
        /// <summary>
        /// The options for configuring access key
        /// </summary>
        private readonly AzureAccessKeyConfigurationOptions configuration;

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="configuration">The options for configuring Azure Access Key</param>
        public AzureAccessKeyOptionsSetup(IOptionsMonitor<AzureAccessKeyConfigurationOptions> configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            this.configuration = configuration.CurrentValue ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configures external Azure Access Key options
        /// </summary>
        public void Configure(AzureAccessKeyOptions options)
        {
            options.AccessKey = this.configuration.AccessKey;
        }
    }
}