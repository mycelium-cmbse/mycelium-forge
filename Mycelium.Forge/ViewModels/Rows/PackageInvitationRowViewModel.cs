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
    using Mycelium.Forge.Common.Models;
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
        /// Gets the minimal display metadata of the target package.
        /// </summary>
        public PackageTargetSummary Package { get; private init; }

        /// <summary>
        /// Gets the owning scope of the target package.
        /// </summary>
        public IScope Scope { get; private init; }

        /// <summary>
        /// Gets a value indicating whether the caller currently has read access to the package.
        /// </summary>
        public bool IsPackageReadable { get; private init; }

        /// <summary>
        /// Gets the human-readable display role name for the invitation.
        /// </summary>
        public string RoleName { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the relative expiration description string.
        /// </summary>
        public string ExpiresIn { get; private init; } = string.Empty;

        /// <summary>
        /// Generates <see cref="PackageInvitationRowViewModel" /> instances for the specified invitations, target package
        /// summaries, and owning scopes.
        /// </summary>
        /// <param name="invitations">The collection of package invitations.</param>
        /// <param name="packageTargets">The collection of minimal target package summaries.</param>
        /// <param name="scopes">The collection of owning scopes.</param>
        /// <param name="readablePackageIds">
        /// Optional identifiers of packages the caller can currently read; when <c>null</c>, resolved packages are treated as readable.
        /// </param>
        /// <returns>An enumerable of <see cref="PackageInvitationRowViewModel" /> instances.</returns>
        public static IEnumerable<PackageInvitationRowViewModel> GenerateRows(IEnumerable<IPackageInvitation> invitations, IEnumerable<PackageTargetSummary> packageTargets, IEnumerable<IScope> scopes, IReadOnlyCollection<Guid> readablePackageIds = null)
        {
            var targetsList = packageTargets.ToList();
            var scopesList = scopes.ToList();

            foreach (var invitation in invitations.OfType<PackageInvitation>())
            {
                var package = targetsList.FirstOrDefault(packageItem => packageItem.Id == invitation.Package);
                var scope = package != null ? scopesList.FirstOrDefault(scopeItem => scopeItem.Id == package.Owner) : null;

                if (package == null || scope == null)
                {
                    continue;
                }

                var isPackageReadable = readablePackageIds == null || readablePackageIds.Contains(package.Id);

                yield return new PackageInvitationRowViewModel
                {
                    Invitation = invitation,
                    Package = package,
                    Scope = scope,
                    IsPackageReadable = isPackageReadable,
                    RoleName = invitation.PackageInvitationKind == PackageInvitationKind.OWNER ? "Owner" : "Maintainer",
                    ExpiresIn = invitation.ExperiesAt <= DateTime.UtcNow ? "Expired" : $"Expires in {invitation.ExperiesAt.ToTimeToCome()}"
                };
            }
        }
    }
}
