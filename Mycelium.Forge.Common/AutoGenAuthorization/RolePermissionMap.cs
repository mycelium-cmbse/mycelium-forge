// ------------------------------------------------------------------------------------------------
// <copyright file="RolePermissionMap.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    /// <summary>
    /// Static mapping of <see cref="RoleKind"/> to their granted <see cref="PermissionKind"/> sets.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public static class RolePermissionMap
    {
        /// <summary>
        /// Gets the dictionary mapping each <see cref="RoleKind"/> to its list of granted <see cref="PermissionKind"/> values.
        /// </summary>
        public static IReadOnlyDictionary<RoleKind, IReadOnlyList<PermissionKind>> RoleToPermissions { get; } = new Dictionary<RoleKind, IReadOnlyList<PermissionKind>>
        {
            {
                RoleKind.InstallationAdministrator, [
                    PermissionKind.ViewAllOrganizations,
                    PermissionKind.ManageOrganizations,
                    PermissionKind.ViewAllAccounts,
                    PermissionKind.ManageAccounts,
                    PermissionKind.ViewInstallationMetrics,
                    PermissionKind.DeletePackage,
                    PermissionKind.ErasePackageVersion,
                    PermissionKind.RevokeAnyApiKey,
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.SuspendOrganizations,
                    PermissionKind.ManageOrganizationSettings,
                    PermissionKind.InviteOrganizationMembers,
                    PermissionKind.RemoveOrganizationMembers,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.RevokeOrganizationInvitation,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateAddress,
                    PermissionKind.ReadAddress,
                    PermissionKind.UpdateAddress,
                    PermissionKind.DeleteAddress,
                    PermissionKind.CreateProfileLink,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.PlatformOperator, [
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.SuspendOrganizations,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.OrganizationAdministrator, [
                    PermissionKind.ManageOrganizationSettings,
                    PermissionKind.InviteOrganizationMembers,
                    PermissionKind.RemoveOrganizationMembers,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.RevokeOrganizationInvitation,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateAddress,
                    PermissionKind.ReadAddress,
                    PermissionKind.UpdateAddress,
                    PermissionKind.DeleteAddress,
                    PermissionKind.CreateProfileLink,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.OrganizationMember, [
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateAddress,
                    PermissionKind.ReadAddress,
                    PermissionKind.UpdateAddress,
                    PermissionKind.DeleteAddress,
                    PermissionKind.CreateProfileLink,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.Account, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateAddress,
                    PermissionKind.ReadAddress,
                    PermissionKind.UpdateAddress,
                    PermissionKind.DeleteAddress,
                    PermissionKind.CreateProfileLink,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.Anonymous, [
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
        };
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
