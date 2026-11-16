// ------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides extension methods for <see cref="IPackage" /> instances.
    /// </summary>
    public static class PackageExtensions
    {
        /// <param name="package">The package instance.</param>
        extension(IPackage package)
        {
            /// <summary>
            /// Determines whether the specified user context is authorized to manage versions of the package.
            /// </summary>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="owner">The optional owning scope of the package, used to verify organization administrator privileges.</param>
            /// <returns><c>true</c> if the user can manage package versions; otherwise, <c>false</c>.</returns>
            public bool CanManageVersions(IUserContext userContext, IScope owner = null)
            {
                if (package == null || userContext is not { AccountId: not null })
                {
                    return false;
                }

                var accountId = userContext.AccountId.Value;

                return (package.Owner == accountId || package.PackageOwner.Contains(accountId) || package.PackageMaintainer.Contains(accountId) || package.IsOrganizationAdministrator(accountId, owner))
                    && PermissionGuard.HasAnyPermission(userContext, PermissionKind.PublishPackageVersion, PermissionKind.UnlistPackageVersion, PermissionKind.RelistPackageVersion);
            }

            /// <summary>
            /// Determines whether the specified account is an administrator of the owning organization for this package.
            /// </summary>
            /// <param name="accountId">The account identifier to evaluate.</param>
            /// <param name="owner">The owning scope of the package.</param>
            /// <returns><c>true</c> if <paramref name="owner" /> is an <see cref="IOrganization" /> matching the package owner and <paramref name="accountId" /> is listed as an administrator; otherwise, <c>false</c>.</returns>
            public bool IsOrganizationAdministrator(Guid accountId, IScope owner)
            {
                return package != null
                    && owner is IOrganization organization
                    && organization.Id == package.Owner
                    && organization.Administrator.Contains(accountId);
            }

            /// <summary>
            /// Computes the fully qualified package name in the format @scope/packageName.
            /// </summary>
            /// <param name="owner">The owning scope instance.</param>
            /// <returns>The fully qualified package identifier string.</returns>
            public string GetFullName(IScope owner)
            {
                if (package == null || owner == null)
                {
                    return string.Empty;
                }

                return package.GetFullName(owner.ShortName);
            }

            /// <summary>
            /// Computes the fully qualified package name in the format @scope/packageName.
            /// </summary>
            /// <param name="scopeShortName">The scope short name or publisher slug.</param>
            /// <returns>The fully qualified package identifier string.</returns>
            public string GetFullName(string scopeShortName)
            {
                if (package == null || string.IsNullOrWhiteSpace(scopeShortName))
                {
                    return string.Empty;
                }

                var cleanScope = scopeShortName.CleanScope();
                return $"@{cleanScope}/{package.Name}";
            }
        }
    }
}
