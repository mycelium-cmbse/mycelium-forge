// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionServiceExtensions.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Dal.PermissionService;

    /// <summary>
    /// Provides extension methods for <see cref="IPermissionService{TThing}" /> instances.
    /// </summary>
    public static class PermissionServiceExtensions
    {
        /// <param name="service">The permission service instance.</param>
        /// <typeparam name="TThing">The entity type.</typeparam>
        extension<TThing>(IPermissionService<TThing> service) where TThing : IThing
        {
            /// <summary>
            /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified entity,
            /// managing an isolated database transaction.
            /// </summary>
            /// <param name="databaseSource">The database source used to open connections and transactions.</param>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="thing">The entity to evaluate.</param>
            /// <param name="token">The cancellation token.</param>
            /// <returns>A task indicating whether updating is permitted.</returns>
            public async Task<ErrorOr<Success>> IsAllowedToUpdate(IDatabaseSource databaseSource, IUserContext userContext, TThing thing, CancellationToken token = default)
            {
                ArgumentNullException.ThrowIfNull(service);
                ArgumentNullException.ThrowIfNull(databaseSource);
                ArgumentNullException.ThrowIfNull(userContext);
                ArgumentNullException.ThrowIfNull(thing);

                try
                {
                    await using var connection = await databaseSource.OpenNewConnectionAsync(token);

                    var transaction = connection != null 
                        ? await connection.BeginTransactionAsync(token) 
                        : null;

                    try
                    {
                        var result = await service.IsAllowedToUpdate(userContext, thing, thing, transaction);

                        if (result.IsError)
                        {
                            if (transaction != null)
                            {
                                await transaction.RollbackAsync(token);
                            }

                            return result.Errors;
                        }

                        if (transaction != null)
                        {
                            await transaction.CommitAsync(token);
                        }

                        return Result.Success;
                    }
                    finally
                    {
                        if (transaction != null)
                        {
                            await transaction.DisposeAsync();
                        }
                    }
                }
                catch (Exception exception)
                {
                    return Error.Failure(description: exception.Message);
                }
            }
        }
    }
}
