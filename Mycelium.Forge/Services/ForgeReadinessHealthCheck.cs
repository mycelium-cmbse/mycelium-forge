// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeReadinessHealthCheck.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// Health check implementation that verifies whether the application Forge context has been initialized.
    /// </summary>
    public class ForgeReadinessHealthCheck : IHealthCheck
    {
        /// <summary>
        /// The Forge application context.
        /// </summary>
        private readonly IForgeContext forgeContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgeReadinessHealthCheck" /> class.
        /// </summary>
        /// <param name="forgeContext">The Forge application context.</param>
        public ForgeReadinessHealthCheck(IForgeContext forgeContext)
        {
            ArgumentNullException.ThrowIfNull(forgeContext);

            this.forgeContext = forgeContext;
        }

        /// <summary>
        /// Runs the health check, returning Healthy if the Forge context has completed initialization, or Unhealthy otherwise.
        /// </summary>
        /// <param name="context">A <see cref="HealthCheckContext" /> representing the current health check context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}" /> representing the asynchronous health check result.</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (this.forgeContext.IsInitialized)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Forge context is initialized."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Forge context is not yet initialized."));
        }
    }
}
