// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMaintainerModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Package
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents a maintainer of a package, optionally wrapping their user account DTO.
    /// </summary>
    public class PackageMaintainerModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMaintainerModel" /> class.
        /// </summary>
        public PackageMaintainerModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMaintainerModel" /> class with specified properties.
        /// </summary>
        /// <param name="name">The maintainer display name.</param>
        /// <param name="initials">The maintainer initials.</param>
        /// <param name="isVerified">A value indicating whether the maintainer is verified.</param>
        /// <param name="role">The maintainer role.</param>
        /// <param name="account">The optional underlying account DTO.</param>
        public PackageMaintainerModel(
            string name,
            string initials,
            bool isVerified = false,
            PackageInvitationKind role = PackageInvitationKind.MAINTAINER,
            IAccount account = null)
        {
            this.Name = name;
            this.Initials = initials;
            this.IsVerified = isVerified;
            this.Role = role;
            this.Account = account;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMaintainerModel" /> class from an account DTO.
        /// </summary>
        /// <param name="account">The user account DTO.</param>
        /// <param name="role">The role.</param>
        /// <param name="isVerified">Whether the account is verified.</param>
        public PackageMaintainerModel(IAccount account, PackageInvitationKind role = PackageInvitationKind.MAINTAINER, bool isVerified = false)
            : this(
                account?.Name ?? string.Empty,
                (account?.Name).ToInitials(),
                isVerified,
                role,
                account)
        {
        }

        /// <summary>
        /// Gets or sets the maintainer display name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maintainer initials.
        /// </summary>
        public string Initials { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the maintainer is verified.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the maintainer role.
        /// </summary>
        public PackageInvitationKind Role { get; set; } = PackageInvitationKind.MAINTAINER;

        /// <summary>
        /// Gets or sets the optional underlying account DTO.
        /// </summary>
        public IAccount Account { get; set; }
    }
}
