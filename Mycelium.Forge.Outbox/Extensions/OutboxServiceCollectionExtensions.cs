// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxServiceCollectionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Orm.Outbox;

    /// <summary>
    /// Provides extension methods for registering transactional outbox services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class OutboxServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="OutboxOptions" /> with startup validation, <see cref="IOutboxDao" />, and the
        /// <see cref="OutboxDispatcherHostedService" /> background worker.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        /// <param name="configuration">The application configuration providing outbox settings.</param>
        /// <returns>The configured <see cref="IServiceCollection" /> instance.</returns>
        public static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddOptions<OutboxOptions>().Bind(configuration.GetSection(OutboxOptions.SectionName)).Validate(IsValidOptions, "Outbox options must define positive intervals, batch size, max attempts, lease duration, and MaxRetryDelaySeconds >= BaseRetryDelaySeconds.").ValidateOnStart();
            services.AddScoped<IOutboxDao, OutboxDao>();
            services.AddHostedService<OutboxDispatcherHostedService>();

            return services;
        }

        /// <summary>
        /// Validates that the configured <see cref="OutboxOptions" /> values are within safe operational bounds.
        /// </summary>
        /// <param name="options">The <see cref="OutboxOptions" /> instance to validate.</param>
        /// <returns><see langword="true" /> if all option values are valid; otherwise <see langword="false" />.</returns>
        private static bool IsValidOptions(OutboxOptions options)
        {
            return options.PollingIntervalSeconds > 0
                   && options.BatchSize > 0
                   && options.MaxAttempts > 0
                   && options.LeaseDurationSeconds > 0
                   && options.BaseRetryDelaySeconds > 0
                   && options.MaxRetryDelaySeconds >= options.BaseRetryDelaySeconds;
        }
    }
}
