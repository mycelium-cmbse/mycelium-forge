// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Represents configuration settings for the transactional outbox dispatcher and retry behavior,
    /// bound from the <see cref="SectionName" /> section of the application settings.
    /// </summary>
    public class OutboxOptions
    {
        /// <summary>
        /// The name of the configuration section holding the outbox settings.
        /// </summary>
        public const string SectionName = "Outbox";

        /// <summary>
        /// Gets or sets a value indicating whether the outbox dispatcher background service is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the interval in seconds between outbox polling cycles.
        /// </summary>
        public int PollingIntervalSeconds { get; set; } = 2;

        /// <summary>
        /// Gets or sets the maximum number of outbox messages claimed per polling batch.
        /// </summary>
        public int BatchSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the maximum number of durable delivery attempts before marking a message as failed.
        /// </summary>
        public int MaxAttempts { get; set; } = 10;

        /// <summary>
        /// Gets or sets the lease duration in seconds for claimed outbox messages before they become reclaimable.
        /// </summary>
        public int LeaseDurationSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the base delay in seconds for exponential backoff between durable retry attempts.
        /// </summary>
        public int BaseRetryDelaySeconds { get; set; } = 5;

        /// <summary>
        /// Gets or sets the maximum delay in seconds between durable retry attempts.
        /// </summary>
        public int MaxRetryDelaySeconds { get; set; } = 300;

        /// <summary>
        /// Gets or sets the number of immediate in-process Polly retries for transient errors before rescheduling.
        /// </summary>
        public int ImmediateRetryCount { get; set; } = 2;
    }
}
