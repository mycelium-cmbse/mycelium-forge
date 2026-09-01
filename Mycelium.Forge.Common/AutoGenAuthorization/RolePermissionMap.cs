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
                    PermissionKind.MonitorPlatformHealth,
                    PermissionKind.PerformPlatformMaintenance,
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.ManageBillingAndQuotas,
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
                    PermissionKind.DeclineOrganizationInvitation,
                    PermissionKind.RevokeOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UpdatePackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadInternalPackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.DeclinePackageInvitation,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
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
                    PermissionKind.ViewInstallationMetrics,
                    PermissionKind.ViewAuditLog,
                    PermissionKind.MonitorPlatformHealth,
                    PermissionKind.PerformPlatformMaintenance,
                    PermissionKind.ConfigurePlatformDefaults,
                    PermissionKind.ManageBillingAndQuotas,
                    PermissionKind.SuspendOrganizations,
                    PermissionKind.ReadInternalPackage,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
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
                    PermissionKind.DeclineOrganizationInvitation,
                    PermissionKind.RevokeOrganizationInvitation,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UpdatePackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadInternalPackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.DeclinePackageInvitation,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
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
                    PermissionKind.LeaveOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.DeclineOrganizationInvitation,
                    PermissionKind.RevokeOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.UpdatePackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.ManageOwnProfile,
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
                RoleKind.PackageOwner, [
                    PermissionKind.DeletePackage,
                    PermissionKind.ErasePackageVersion,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageToOrganizationScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UpdatePackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.SetPackageVisibility,
                    PermissionKind.ManagePackageTeam,
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadInternalPackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.DeclinePackageInvitation,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.ManageOwnProfile,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateProfileLink,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                ]
            },
            {
                RoleKind.PackageMaintainer, [
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.UpdatePackageVersion,
                    PermissionKind.UnlistPackageVersion,
                    PermissionKind.RelistPackageVersion,
                    PermissionKind.ReadInternalPackage,
                    PermissionKind.ReadPrivatePackage,
                    PermissionKind.AcceptPackageInvitation,
                    PermissionKind.DeclinePackageInvitation,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.ViewOwnMemberships,
                    PermissionKind.IssueApiKey,
                    PermissionKind.RevokeOwnApiKey,
                    PermissionKind.ListOwnApiKeys,
                    PermissionKind.CreateAddress,
                    PermissionKind.ReadCountry,
                    PermissionKind.ReadPackageType,
                    PermissionKind.ReadProfileType,
                ]
            },
            {
                RoleKind.PackageReader, [
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.ManagePackageSettings,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.ReadProfileLink,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                ]
            },
            {
                RoleKind.Account, [
                    PermissionKind.CreateOrganization,
                    PermissionKind.AcceptOrganizationInvitation,
                    PermissionKind.DeclineOrganizationInvitation,
                    PermissionKind.PublishPackageToPersonalScope,
                    PermissionKind.PublishPackageVersion,
                    PermissionKind.ReadPublicPackage,
                    PermissionKind.DeclinePackageInvitation,
                    PermissionKind.RevokePackageInvitation,
                    PermissionKind.ViewOwnMemberships,
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
                    PermissionKind.TransferPackageOwnership,
                    PermissionKind.UpdateProfileLink,
                    PermissionKind.DeleteProfileLink,
                    PermissionKind.ReadCountry,
                ]
            },
        };
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
