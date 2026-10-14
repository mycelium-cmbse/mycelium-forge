// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeInitializationHostedService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Hosted service that initializes the Forge context during application startup and retries periodically until successful.
    /// </summary>
    public class ForgeInitializationHostedService : BackgroundService
    {
        /// <summary>
        /// The configuration key for the retry interval in seconds.
        /// </summary>
        public const string RetryIntervalSecondsConfigurationKey = "ForgeInitialization:RetryIntervalSeconds";

        /// <summary>
        /// The default retry interval in seconds when not configured.
        /// </summary>
        public const int DefaultRetryIntervalSeconds = 5;

        /// <summary>
        /// The configuration settings provider.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The Forge application context.
        /// </summary>
        private readonly IForgeContext forgeContext;

        /// <summary>
        /// The logger for diagnostic messages.
        /// </summary>
        private readonly ILogger<ForgeInitializationHostedService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgeInitializationHostedService" /> class.
        /// </summary>
        /// <param name="forgeContext">The Forge application context.</param>
        /// <param name="configuration">The configuration settings provider.</param>
        /// <param name="logger">The logger for diagnostic messages.</param>
        public ForgeInitializationHostedService(IForgeContext forgeContext, IConfiguration configuration, ILogger<ForgeInitializationHostedService> logger)
        {
            ArgumentNullException.ThrowIfNull(forgeContext);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(logger);

            this.forgeContext = forgeContext;
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the background service task that periodically attempts to initialize the Forge context until successful.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="Task" /> representing the background execution.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var retryIntervalSeconds = this.configuration.GetValue(RetryIntervalSecondsConfigurationKey, DefaultRetryIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested && !this.forgeContext.IsInitialized)
            {
                try
                {
                    if (await this.forgeContext.TryInitializeAsync(stoppingToken))
                    {
                        break;
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    this.logger.LogWarning(exception, "Error occurred during Forge context initialization.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(retryIntervalSeconds), stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }
}
