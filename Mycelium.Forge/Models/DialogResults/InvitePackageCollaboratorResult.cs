// ------------------------------------------------------------------------------------------------
// <copyright file="InvitePackageCollaboratorResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.DialogResults
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the result payload containing the target account and invitation role when inviting a collaborator to a
    /// package.
    /// </summary>
    public class InvitePackageCollaboratorResult
    {
        /// <summary>
        /// Gets or sets the target account being invited.
        /// </summary>
        public IAccount TargetAccount { get; set; }

        /// <summary>
        /// Gets or sets the invitation role to be granted to the target account upon acceptance.
        /// </summary>
        public PackageInvitationKind Role { get; set; }
    }
}
