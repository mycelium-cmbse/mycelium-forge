// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncPayload.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Services
{
    /// <summary>
    /// Represents the outbox payload for synchronizing an account profile update to Keycloak.
    /// </summary>
    /// <param name="AccountId">The unique identifier of the account to update in Keycloak.</param>
    /// <param name="Email">The updated email address.</param>
    /// <param name="FirstName">The updated first name.</param>
    /// <param name="LastName">The updated last name.</param>
    public sealed record KeycloakAccountSyncPayload(Guid AccountId, string Email, string FirstName, string LastName);
}
