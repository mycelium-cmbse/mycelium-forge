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
    internal static class DatabaseSourceExtensions
    {
        /// <summary>
        /// Asynchronously reads and filters <typeparamref name="TThing" /> instances matching the specified filter, managing its
        /// own
        /// connection and transaction.
        /// </summary>
        /// <typeparam name="TThing">The entity type implementing <see cref="IThing" />.</typeparam>
        /// <param name="databaseSource">The database source connection provider.</param>
        /// <param name="readDaoAsync">The delegate function executing the DAO read operation within a transaction.</param>
        /// <param name="permissionService">The permission service to evaluate read access.</param>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>
        /// A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{TThing}" /> of permitted entities or
        /// an error.
        /// </returns>
        internal static async Task<ErrorOr<ImmutableList<TThing>>> ReadWithFilterAsync<TThing>(this IDatabaseSource databaseSource, Func<NpgsqlTransaction, CancellationToken, Task<ErrorOr<ImmutableList<TThing>>>> readDaoAsync,
            IPermissionService<TThing> permissionService, IUserContext userContext, ILogger logger, CancellationToken token = default) where TThing : IThing
        {
            try
            {
                await using var connection = await databaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var readResult = await readDaoAsync.Invoke(transaction, token);

                if (readResult.IsError)
                {
                    await transaction.RollbackAsync(token);
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

                await transaction.CommitAsync(token);
                return permittedItems.ToImmutableList();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "ReadWithFilterAsync for {EntityName} failed with an error", typeof(TThing).Name);
                return Error.Failure(description: exception.Message);
            }
        }
    }
}
