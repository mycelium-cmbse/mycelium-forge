// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncHandler.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Dal.Services;

    /// <summary>
    /// Synchronizes Forge account updates to Keycloak via <see cref="IKeycloakAdminClient" />.
    /// </summary>
    public class KeycloakAccountSyncHandler : IAccountSyncHandler
    {
        /// <summary>
        /// The injected <see cref="IKeycloakAdminClient" /> used to update users in Keycloak.
        /// </summary>
        private readonly IKeycloakAdminClient keycloakAdminClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakAccountSyncHandler" /> class.
        /// </summary>
        /// <param name="keycloakAdminClient">The injected <see cref="IKeycloakAdminClient" />.</param>
        public KeycloakAccountSyncHandler(IKeycloakAdminClient keycloakAdminClient)
        {
            this.keycloakAdminClient = keycloakAdminClient;
        }

        /// <summary>
        /// Asynchronously synchronizes changes between the existing and updated account to Keycloak.
        /// </summary>
        /// <param name="existing">The account state prior to the update.</param>
        /// <param name="updated">The updated account state.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the synchronization succeeded.</returns>
        public Task<ErrorOr<Success>> SyncAccountAsync(IAccount existing, IAccount updated, CancellationToken token)
        {
            var (firstName, lastName) = updated.Name.SplitFullName();
            return this.keycloakAdminClient.UpdateUserAsync(updated.Id, updated.Email, firstName, lastName, token);
        }
    }
}
