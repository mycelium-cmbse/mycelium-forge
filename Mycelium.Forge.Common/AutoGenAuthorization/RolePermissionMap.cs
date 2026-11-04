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
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
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
                    PermissionKind.ManageOrganizationMembers,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.CreateOrganization,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.PlatformOperator, [
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.SuspendOrganizations,
                ]
            },
            {
                RoleKind.OrganizationAdministrator, [
                    PermissionKind.ManageOrganizationSettings,
                    PermissionKind.ManageOrganizationMembers,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.CreateOrganization,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.Account, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.Anonymous, [
                ]
            },
        };
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
