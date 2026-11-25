// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncOutboxHandler.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    using Mycelium.Forge.Dal.Events;
    using Mycelium.Forge.Outbox;

    /// <summary>
    /// Outbox event handler that synchronizes Forge account profile updates to Keycloak via
    /// <see cref="IKeycloakAdminClient" />.
    /// </summary>
    public class KeycloakAccountSyncOutboxHandler : OutboxEventHandler<AccountProfileChanged>
    {
        /// <summary>
        /// The injected <see cref="IKeycloakAdminClient" /> used to update users in Keycloak.
        /// </summary>
        private readonly IKeycloakAdminClient keycloakAdminClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAccountSyncOutboxHandler" /> class.
        /// </summary>
        /// <param name="keycloakAdminClient">The injected <see cref="IKeycloakAdminClient" />.</param>
        /// <param name="logger">The injected <see cref="ILogger{KeycloakAccountSyncOutboxHandler}" />.</param>
        public KeycloakAccountSyncOutboxHandler(IKeycloakAdminClient keycloakAdminClient, ILogger<KeycloakAccountSyncOutboxHandler> logger) : base(logger)
        {
            this.keycloakAdminClient = keycloakAdminClient;
        }

        /// <summary>
        /// Asynchronously processes the deserialized <see cref="AccountProfileChanged" /> event by updating the corresponding
        /// Keycloak user.
        /// </summary>
        /// <param name="outboxEvent">The deserialized <see cref="AccountProfileChanged" /> event.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether synchronization succeeded.</returns>
        protected override Task<ErrorOr<Success>> HandleAsync(AccountProfileChanged outboxEvent, CancellationToken token)
        {
            return this.keycloakAdminClient.UpdateUserAsync(outboxEvent.AccountId, outboxEvent.Email, outboxEvent.FirstName, outboxEvent.LastName, token);
        }
    }
}
