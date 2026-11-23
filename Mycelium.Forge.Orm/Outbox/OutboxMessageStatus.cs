// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxMessageStatus.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Outbox
{
    /// <summary>
    /// Specifies the processing status of a transactional outbox message.
    /// </summary>
    public enum OutboxMessageStatus
    {
        /// <summary>
        /// The outbox message is pending delivery.
        /// </summary>
        Pending,

        /// <summary>
        /// The outbox message has permanently failed delivery.
        /// </summary>
        Failed
    }
}
