// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSourceExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Extensions
{
    using System.Collections.Immutable;

    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Dal.PermissionService;

    using Npgsql;

    /// <summary>
    /// Provides extension methods for <see cref="IDatabaseSource" /> instances.
    /// </summary>
    public static class DatabaseSourceExtensions
    {
        /// <param name="databaseSource">The database source connection provider.</param>
        extension(IDatabaseSource databaseSource)
        {
            /// <summary>
            /// Asynchronously reads and filters <typeparamref name="TThing" /> instances matching the specified filter, managing its
            /// own
            /// connection and transaction.
            /// </summary>
            /// <typeparam name="TThing">The entity type implementing <see cref="IThing" />.</typeparam>
            /// <param name="readDaoAsync">The delegate function executing the DAO read operation within a transaction.</param>
            /// <param name="permissionService">The permission service to evaluate read access.</param>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="logger">The logger instance.</param>
            /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
            /// <returns>
            /// A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{TThing}" /> of permitted entities or
            /// an error.
            /// </returns>
            public async Task<ErrorOr<ImmutableList<TThing>>> ReadWithFilterAsync<TThing>(Func<NpgsqlTransaction, CancellationToken, Task<ErrorOr<ImmutableList<TThing>>>> readDaoAsync,
                IPermissionService<TThing> permissionService, IUserContext userContext, ILogger logger, CancellationToken token = default) where TThing : IThing
            {
                ArgumentNullException.ThrowIfNull(readDaoAsync);
                ArgumentNullException.ThrowIfNull(permissionService);
                ArgumentNullException.ThrowIfNull(userContext);
                ArgumentNullException.ThrowIfNull(logger);

                return await databaseSource.ExecuteInTransactionAsync<ImmutableList<TThing>>(
                    async transaction =>
                    {
                        var readResult = await readDaoAsync.Invoke(transaction, token);

                        if (readResult.IsError)
                        {
                            return readResult.Errors;
                        }

                        List<TThing> permittedItems = [];

                        foreach (var item in readResult.Value)
                        {
                            var permissionResult = await permissionService.IsAllowedToRead(userContext, item, transaction);

                            if (!permissionResult.IsError)
                            {
                                permittedItems.Add(item);
                            }
                        }

                        return permittedItems.ToImmutableList();
                    },
                    token);
            }
        }
    }
}
