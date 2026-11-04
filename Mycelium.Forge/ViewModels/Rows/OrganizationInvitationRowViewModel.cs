// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationRowViewModel.cs" company="Starion Group S.A.">
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
    /// Represents a row in the pending organization invitations list on the account settings page.
    /// </summary>
    public sealed class OrganizationInvitationRowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationInvitationRowViewModel" /> class.
        /// </summary>
        private OrganizationInvitationRowViewModel()
        {
        }

        /// <summary>
        /// Gets the pending organization invitation.
        /// </summary>
        public OrganizationInvitation Invitation { get; private init; }

        /// <summary>
        /// Gets the target organization.
        /// </summary>
        public Organization Organization { get; private init; }

        /// <summary>
        /// Gets the human-readable display role name for the invitation.
        /// </summary>
        public string RoleName { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the relative expiration description string.
        /// </summary>
        public string ExpiresIn { get; private init; } = string.Empty;

        /// <summary>
        /// Generates <see cref="OrganizationInvitationRowViewModel" /> instances for the specified invitations and organizations.
        /// </summary>
        /// <param name="invitations">The collection of invitations.</param>
        /// <param name="relatedThings">The collection of related Things.</param>
        /// <returns>An enumerable of <see cref="OrganizationInvitationRowViewModel" /> instances.</returns>
        public static IEnumerable<OrganizationInvitationRowViewModel> GenerateRows(IEnumerable<IOrganizationInvitation> invitations, IEnumerable<IThing> relatedThings)
        {
            var organizationsList = relatedThings?.OfType<Organization>().ToList() ?? [];

            foreach (var invitation in invitations.OfType<OrganizationInvitation>())
            {
                var organization = organizationsList.FirstOrDefault(org => org.Id == invitation.Organization);

                if (organization == null)
                {
                    continue;
                }

                yield return new OrganizationInvitationRowViewModel
                {
                    Invitation = invitation,
                    Organization = organization,
                    RoleName = invitation.OrganizationInvitationKind == OrganizationInvitationKind.ADMINISTRATOR ? "Administrator" : "Member",
                    ExpiresIn = invitation.ExperiesAt <= DateTime.UtcNow ? "Expired" : $"Expires in {invitation.ExperiesAt.ToTimeToCome()}"
                };
            }
        }
    }
}
