// ------------------------------------------------------------------------------------------------
// <copyright file="IUserContext.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the contextual user information and assigned roles for authorization evaluation.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets or sets the unique identifier of the authenticated user account, if applicable.
        /// </summary>
        Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the application and domain roles currently assigned to the user.
        /// </summary>
        IReadOnlyList<RoleKind> CurrentRoles { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
