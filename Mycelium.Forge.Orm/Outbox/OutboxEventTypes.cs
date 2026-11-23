// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxEventTypes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    /// <summary>
    /// Defines event type discriminator constants used to route transactional outbox messages to handlers.
    /// </summary>
    public static class OutboxEventTypes
    {
        /// <summary>
        /// The event type identifier for synchronizing an account profile update to Keycloak.
        /// </summary>
        public const string AccountKeycloakSync = "Account.KeycloakSync";
    }
}
