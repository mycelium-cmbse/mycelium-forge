// ------------------------------------------------------------------------------------------------
// <copyright file="PackagePermissionServiceExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenPermissionService;
    using Mycelium.Forge.Dal.DatabaseSource;

    /// <summary>
    /// Provides extension methods for <see cref="IPackagePermissionService" /> instances.
    /// </summary>
    public static class PackagePermissionServiceExtensions
    {
        /// <param name="service">The <see cref="IPackagePermissionService" /> instance.</param>
        extension(IPackagePermissionService service)
        {
            /// <summary>
            /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified package,
            /// managing an isolated database transaction.
            /// </summary>
            /// <param name="databaseSource">The database source used to open connections and transactions.</param>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="package">The package entity to evaluate.</param>
            /// <param name="token">The cancellation token.</param>
            /// <returns>A task indicating whether updating is permitted.</returns>
            public async Task<ErrorOr<Success>> IsAllowedToUpdate(IDatabaseSource databaseSource, IUserContext userContext, IPackage package, CancellationToken token = default)
            {
                ArgumentNullException.ThrowIfNull(service);
                ArgumentNullException.ThrowIfNull(databaseSource);
                ArgumentNullException.ThrowIfNull(userContext);
                ArgumentNullException.ThrowIfNull(package);

                try
                {
                    await using var connection = await databaseSource.OpenNewConnectionAsync(token);
                    await using var transaction = await connection.BeginTransactionAsync(token);

                    var result = await service.IsAllowedToUpdate(userContext, package, package, transaction);

                    if (result.IsError)
                    {
                        await transaction.RollbackAsync(token);
                        return result.Errors;
                    }

                    await transaction.CommitAsync(token);
                    return Result.Success;
                }
                catch (Exception exception)
                {
                    return Error.Failure(description: exception.Message);
                }
            }
        }
    }
}
