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
                    PermissionKind.AssignOrganizationMemberships,
                    PermissionKind.ReserveScopeSlug,
                    PermissionKind.ConfigureMirroring,
                    PermissionKind.ViewInstallationMetrics,
                    PermissionKind.ViewAuditLog,
                    PermissionKind.DeletePackage,
                    PermissionKind.ErasePackageVersion,
                    PermissionKind.RevokeAnyApiKey,
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.SuspendOrganizations,
                    PermissionKind.CreateOrganization,
                    PermissionKind.ManageOrganizationSettings,
                    PermissionKind.InviteOrganizationMembers,
                    PermissionKind.RemoveOrganizationMembers,
                    PermissionKind.ManageOrganizationRoles,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.LeaveOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.PlatformOperator, [
                    PermissionKind.ViewInstallationMetrics,
                    PermissionKind.MonitorPlatformHealth,
                    PermissionKind.PerformPlatformMaintenance,
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.ManageBillingAndQuotas,
                    PermissionKind.SuspendOrganizations,
                    PermissionKind.ReadPublicPackage,
                ]
            },
            {
                RoleKind.OrganizationAdministrator, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.ManageOrganizationSettings,
                    PermissionKind.InviteOrganizationMembers,
                    PermissionKind.RemoveOrganizationMembers,
                    PermissionKind.ManageOrganizationRoles,
                    PermissionKind.ConfigurePublishingPolicy,
                    PermissionKind.TransferOrganizationAdministration,
                    PermissionKind.ConfigureDefaultPackageVisibility,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.LeaveOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.OrganizationMember, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.ViewOrganizationPackageList,
                    PermissionKind.ViewOrganizationMemberList,
                    PermissionKind.LeaveOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.PackageOwner, [
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.PackageMaintainer, [
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.PackageReader, [
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadOrganizationVisiblePackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.Account, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                ]
            },
            {
                RoleKind.Anonymous, [
                    PermissionKind.ReadPublicPackage,
                ]
            },
        };
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
