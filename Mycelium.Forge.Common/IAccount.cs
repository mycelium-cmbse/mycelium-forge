// ------------------------------------------------------------------------------------------------
// <copyright file="IAccount.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Partial interface extending <see cref="IAccount" /> with verification status.
    /// </summary>
    /// <remarks>
    /// This partial interface is temporary and will be removed once the domain metamodel is updated and DTOs are regenerated.
    /// </remarks>
    public partial interface IAccount
    {
        /// <summary>
        /// Gets or sets a value indicating whether the account has verified credentials.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once account verification is incorporated into the domain model.
        /// </remarks>
        bool IsVerified { get; set; }
    }
}
