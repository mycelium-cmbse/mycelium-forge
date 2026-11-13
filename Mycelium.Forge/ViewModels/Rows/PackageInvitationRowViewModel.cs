// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationRowViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Rows
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents a row in the pending package invitations list on the account settings page.
    /// </summary>
    public sealed class PackageInvitationRowViewModel : IInvitationRowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageInvitationRowViewModel" /> class.
        /// </summary>
        private PackageInvitationRowViewModel()
        {
        }

        /// <summary>
        /// Gets the underlying invitation entity.
        /// </summary>
        IInvitation IInvitationRowViewModel.Invitation => this.Invitation;

        /// <summary>
        /// Gets the pending package invitation.
        /// </summary>
        public PackageInvitation Invitation { get; private init; }

        /// <summary>
        /// Gets the target package, if visible to the caller.
        /// </summary>
        public Package Package { get; private init; }

        /// <summary>
        /// Gets the owning scope of the target package, if visible to the caller.
        /// </summary>
        public IScope Scope { get; private init; }

        /// <summary>
        /// Gets the human-readable display role name for the invitation.
        /// </summary>
        public string RoleName { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the relative expiration description string.
        /// </summary>
        public string ExpiresIn { get; private init; } = string.Empty;

        /// <summary>
        /// Generates <see cref="PackageInvitationRowViewModel" /> instances for the specified invitations and related entities.
        /// </summary>
        /// <param name="invitations">The collection of package invitations.</param>
        /// <param name="relatedThings">The collection of related Things (packages and scopes).</param>
        /// <returns>An enumerable of <see cref="PackageInvitationRowViewModel" /> instances.</returns>
        public static IEnumerable<PackageInvitationRowViewModel> GenerateRows(IEnumerable<IPackageInvitation> invitations, IEnumerable<IThing> relatedThings)
        {
            var relatedThingsList = relatedThings?.ToList() ?? [];
            var packages = relatedThingsList.OfType<Package>().ToList();
            var scopes = relatedThingsList.OfType<IScope>().ToList();

            foreach (var invitation in invitations.OfType<PackageInvitation>())
            {
                var package = packages.FirstOrDefault(packageItem => packageItem.Id == invitation.Package);
                var scope = package != null ? scopes.FirstOrDefault(scopeItem => scopeItem.Id == package.Owner) : null;

                yield return new PackageInvitationRowViewModel
                {
                    Invitation = invitation,
                    Package = package,
                    Scope = scope,
                    RoleName = invitation.PackageInvitationKind == PackageInvitationKind.OWNER ? "Owner" : "Maintainer",
                    ExpiresIn = invitation.ExperiesAt <= DateTime.UtcNow ? "Expired" : $"Expires in {invitation.ExperiesAt.ToTimeToCome()}"
                };
            }
        }
    }
}
