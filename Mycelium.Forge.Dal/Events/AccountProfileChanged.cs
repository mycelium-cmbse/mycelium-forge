// ------------------------------------------------------------------------------------------------
// <copyright file="AccountProfileChanged.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Events
{
    using Mycelium.Forge.Orm.Outbox;

    /// <summary>
    /// Domain outbox event raised when an account profile's name or email changes in Forge.
    /// </summary>
    /// <param name="AccountId">The unique identifier of the updated account.</param>
    /// <param name="Email">The updated email address.</param>
    /// <param name="FirstName">The updated first name.</param>
    /// <param name="LastName">The updated last name.</param>
    public sealed record AccountProfileChanged(Guid AccountId, string Email, string FirstName, string LastName) : IOutboxEvent
    {
        /// <summary>
        /// Gets the event type discriminator used to route <see cref="AccountProfileChanged" /> messages.
        /// </summary>
        public static string EventType => "Account.ProfileChanged";

        /// <summary>
        /// Gets the aggregate identifier for the updated account.
        /// </summary>
        public Guid AggregateId => this.AccountId;
    }
}
