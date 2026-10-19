// ------------------------------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Services
{
    using System.Collections.Immutable;

    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Comparers;
    using Mycelium.Forge.Common.Extensions;
    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Dal.DtoValidator;
    using Mycelium.Forge.Dal.PermissionService;
    using Mycelium.Forge.Orm;
    using Mycelium.Forge.Orm.Filters;

    using Npgsql;

    using NpgsqlTypes;

    /// <summary>
    /// Base class for domain entity services bringing validation, permission evaluation, and DAO operations together.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public abstract class ServiceBase<T> : IService<T> where T : class, IThing
    {
        /// <summary>
        /// The (injected) <see cref="IDao{T}" /> data access object.
        /// </summary>
        protected readonly IDao<T> Dao;

        /// <summary>
        /// The (injected) <see cref="IDatabaseSource" /> database connection provider.
        /// </summary>
        protected readonly IDatabaseSource DatabaseSource;

        /// <summary>
        /// The (injected) <see cref="IDtoComparer{T}" /> DTO comparer.
        /// </summary>
        protected readonly IDtoComparer<T> DtoComparer;

        /// <summary>
        /// The (injected) <see cref="IDtoValidator{T}" /> DTO validator.
        /// </summary>
        protected readonly IDtoValidator<T> DtoValidator;

        /// <summary>
        /// The (injected) <see cref="ILogger" /> logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// The (injected) <see cref="IPermissionService{T}" /> permission service.
        /// </summary>
        protected readonly IPermissionService<T> PermissionService;

        /// <summary>
        /// The (injected) <see cref="IReadFilter" /> read filter provider.
        /// </summary>
        protected readonly IReadFilter ReadFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase{T}" /> class.
        /// </summary>
        /// <param name="dao">The (injected) <see cref="IDao{T}" /> data access object.</param>
        /// <param name="dtoValidator">The (injected) <see cref="IDtoValidator{T}" /> DTO validator.</param>
        /// <param name="permissionService">The (injected) <see cref="IPermissionService{T}" /> permission service.</param>
        /// <param name="dtoComparer">The (injected) <see cref="IDtoComparer{T}" /> DTO comparer.</param>
        /// <param name="logger">The (injected) <see cref="ILogger" /> logger.</param>
        /// <param name="databaseSource">The (injected) <see cref="IDatabaseSource" /> database connection provider.</param>
        /// <param name="readFilter">The (injected) <see cref="IReadFilter" /> read filter provider.</param>
        protected ServiceBase(IDao<T> dao, IDtoValidator<T> dtoValidator, IPermissionService<T> permissionService, IDtoComparer<T> dtoComparer, ILogger logger, IDatabaseSource databaseSource, IReadFilter readFilter)
        {
            this.Dao = dao;
            this.DtoValidator = dtoValidator;
            this.PermissionService = permissionService;
            this.DtoComparer = dtoComparer;
            this.Logger = logger;
            this.DatabaseSource = databaseSource;
            this.ReadFilter = readFilter;
        }

        /// <summary>
        /// Gets the discriminator class kind for the entity in the Thing table.
        /// </summary>
        private static string ClassKind => typeof(T).Name.StartsWith('I') && typeof(T).IsInterface ? typeof(T).Name[1..] : typeof(T).Name;

        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to create.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Created}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Created>> CreateAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<T> dtos, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(dtos);

            var dtoList = dtos.ToList();

            if (dtoList.Count == 0)
            {
                return Result.Created;
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Starting CreateAsync for {Count} {ClassKind} instances within transaction", dtoList.Count, ClassKind);
            }

            var validationResult = await this.AreValid(dtoList);

            if (validationResult.IsError)
            {
                return validationResult.Errors;
            }

            foreach (var dto in dtoList)
            {
                var permissionResult = await this.PermissionService.IsAllowedToCreate(userContext, dto, transaction);

                if (permissionResult.IsError)
                {
                    return permissionResult.Errors;
                }

                var createResult = await this.Dao.CreateAsync(transaction, dto, token);

                if (createResult.IsError)
                {
                    return createResult.Errors;
                }
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Finished CreateAsync successfully for {Count} {ClassKind} instances", dtoList.Count, ClassKind);
            }

            return Result.Created;
        }

        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to create.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Created}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Created>> CreateAsync(IUserContext userContext, IEnumerable<T> dtos, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(dtos);

            try
            {
                await using var connection = await this.DatabaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var result = await this.CreateAsync(userContext, transaction, dtos, token);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(token);
                    return result.Errors;
                }

                await transaction.CommitAsync(token);
                return Result.Created;
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "CreateAsync for {ClassKind} failed with an error", ClassKind);

                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        public async Task<ErrorOr<ImmutableList<T>>> ReadAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, Guid[] iids = null)
        {
            ArgumentNullException.ThrowIfNull(userContext);

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Starting ReadAsync for {ClassKind} instances within transaction", ClassKind);
            }

            var filter = this.ReadFilter.FromUserContext(userContext);
            var readResult = await this.Dao.ReadAsync(transaction, token, iids, filter: filter);

            if (readResult.IsError)
            {
                return readResult.Errors;
            }

            List<T> permittedItems = [];

            foreach (var item in readResult.Value)
            {
                var permissionResult = await this.PermissionService.IsAllowedToRead(userContext, item, transaction);

                if (!permissionResult.IsError)
                {
                    permittedItems.Add(item);
                }
            }

            return permittedItems.ToImmutableList();
        }

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        public async Task<ErrorOr<ImmutableList<T>>> ReadAsync(IUserContext userContext, CancellationToken token, Guid[] iids = null)
        {
            ArgumentNullException.ThrowIfNull(userContext);

            try
            {
                await using var connection = await this.DatabaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var result = await this.ReadAsync(userContext, transaction, token, iids);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(token);
                    return result.Errors;
                }

                await transaction.CommitAsync(token);
                return result;
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "ReadAsync for {ClassKind} failed with an error", ClassKind);

                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to update.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Updated}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Updated>> UpdateAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<T> dtos, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(dtos);

            var dtoList = dtos.ToList();

            if (dtoList.Count == 0)
            {
                return Result.Updated;
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Starting UpdateAsync for {Count} {ClassKind} instances within transaction", dtoList.Count, ClassKind);
            }

            var validationResult = await this.AreValid(dtoList);

            if (validationResult.IsError)
            {
                return validationResult.Errors;
            }

            var filter = this.ReadFilter.FromUserContext(userContext);
            var existingDtosResult = await this.Dao.ReadAsync(transaction, token, [.. dtoList.Select(x => x.Id)], filter: filter);

            if (existingDtosResult.IsError)
            {
                return existingDtosResult.Errors;
            }

            var existingDtos = existingDtosResult.Value;

            foreach (var dto in dtoList)
            {
                var existing = existingDtos.Find(x => x.Id == dto.Id);

                if (existing == null)
                {
                    return Error.NotFound(description: $"The {typeof(T).Name} with ID '{dto.Id}' was not found for update.");
                }

                var permissionResult = await this.PermissionService.IsAllowedToUpdate(userContext, existing, dto, transaction);

                if (permissionResult.IsError)
                {
                    return permissionResult.Errors;
                }

                ServiceHelper.LogChanges(this.Logger, this.DtoComparer, existing, dto);

                var updateResult = await this.Dao.UpdateAsync(transaction, dto, token);

                if (updateResult.IsError)
                {
                    return updateResult.Errors;
                }
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Finished UpdateAsync successfully for {Count} {ClassKind} instances", dtoList.Count, ClassKind);
            }

            return Result.Updated;
        }

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to update.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Updated}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Updated>> UpdateAsync(IUserContext userContext, IEnumerable<T> dtos, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(dtos);

            try
            {
                await using var connection = await this.DatabaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var result = await this.UpdateAsync(userContext, transaction, dtos, token);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(token);
                    return result.Errors;
                }

                await transaction.CommitAsync(token);
                return Result.Updated;
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "UpdateAsync for {ClassKind} failed with an error", ClassKind);

                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Deleted}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Deleted>> DeleteAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<Guid> iids, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(iids);

            var idList = iids.ToList();

            if (idList.Count == 0)
            {
                return Result.Deleted;
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Starting DeleteAsync for {Count} {ClassKind} instances within transaction", idList.Count, ClassKind);
            }

            var filter = this.ReadFilter.FromUserContext(userContext);
            var existingDtosResult = await this.Dao.ReadAsync(transaction, token, [.. idList], filter: filter);

            if (existingDtosResult.IsError)
            {
                return existingDtosResult.Errors;
            }

            var existingDtos = existingDtosResult.Value;
            List<T> entitiesToDelete = [];

            foreach (var iid in idList)
            {
                var existing = existingDtos.Find(x => x.Id == iid);

                if (existing == null)
                {
                    return Error.NotFound(description: $"The {typeof(T).Name} with ID '{iid}' was not found for delete.");
                }

                var permissionResult = await this.PermissionService.IsAllowedToDelete(userContext, existing, transaction);

                if (permissionResult.IsError)
                {
                    return permissionResult.Errors;
                }

                entitiesToDelete.Add(existing);
            }

            var deleteResult = await this.Dao.DeleteAsync(transaction, entitiesToDelete, token);

            if (deleteResult.IsError)
            {
                return deleteResult.Errors;
            }

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Finished DeleteAsync successfully for {Count} {ClassKind} instances", idList.Count, ClassKind);
            }

            return Result.Deleted;
        }

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Deleted}" /> indicating success or failure.</returns>
        public async Task<ErrorOr<Deleted>> DeleteAsync(IUserContext userContext, IEnumerable<Guid> iids, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);
            ArgumentNullException.ThrowIfNull(iids);

            try
            {
                await using var connection = await this.DatabaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var result = await this.DeleteAsync(userContext, transaction, iids, token);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(token);
                    return result.Errors;
                }

                await transaction.CommitAsync(token);
                return Result.Deleted;
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "DeleteAsync for {ClassKind} failed with an error", ClassKind);

                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> matching the specified property values within an existing
        /// transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="propertyName">The name of the property to query.</param>
        /// <param name="values">An optional array of index values to read.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        protected async Task<ErrorOr<ImmutableList<T>>> ReadByIndexAsync(IUserContext userContext, NpgsqlTransaction transaction, string propertyName, string[] values, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);

            var sqlAttributeName = propertyName.ToLowerCaseFirst();

            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.Logger.LogDebug("Starting ReadByIndexAsync for {ClassKind} on {Attribute} within transaction", ClassKind, sqlAttributeName);
            }

            if (values is not { Length: > 0 })
            {
                return await this.ReadAsync(userContext, transaction, token);
            }

            var filter = (SqlFilter)this.ReadFilter.FromUserContext(userContext);

            filter.Where($"""
                          "Thing"."classKind" = '{ClassKind}' AND "Thing"."data"->>'{sqlAttributeName}' = ANY(@values)
                          """);

            filter.AddParameter("@values", (NpgsqlDbType)((int)NpgsqlDbType.Array | (int)NpgsqlDbType.Text), values);

            var readResult = await this.Dao.ReadAsync(transaction, token, filter: filter);

            if (readResult.IsError)
            {
                return readResult.Errors;
            }

            List<T> permittedItems = [];

            foreach (var item in readResult.Value)
            {
                var permissionResult = await this.PermissionService.IsAllowedToRead(userContext, item, transaction);

                if (!permissionResult.IsError)
                {
                    permittedItems.Add(item);
                }
            }

            return permittedItems.ToImmutableList();
        }

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> matching the specified property values managing its own
        /// connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="propertyName">The name of the property to query.</param>
        /// <param name="values">An optional array of index values to read.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        protected async Task<ErrorOr<ImmutableList<T>>> ReadByIndexAsync(IUserContext userContext, string propertyName, string[] values, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(userContext);

            var sqlAttributeName = propertyName.ToLowerCaseFirst();

            try
            {
                await using var connection = await this.DatabaseSource.OpenNewConnectionAsync(token);
                await using var transaction = await connection.BeginTransactionAsync(token);

                var result = await this.ReadByIndexAsync(userContext, transaction, propertyName, values, token);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(token);
                    return result.Errors;
                }

                await transaction.CommitAsync(token);
                return result;
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "ReadByIndexAsync for {ClassKind} on {Attribute} failed with an error", ClassKind, sqlAttributeName);

                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously validates a collection of <typeparamref name="T" /> instances.
        /// </summary>
        /// <param name="dtos">The collection of instances to validate.</param>
        /// <returns>A <see cref="ErrorOr{Success}" /> indicating whether all instances are valid.</returns>
        protected virtual async Task<ErrorOr<Success>> AreValid(IEnumerable<T> dtos)
        {
            foreach (var dto in dtos)
            {
                var validationResult = await this.DtoValidator.ValidateDto(dto);

                if (validationResult.IsError)
                {
                    return validationResult.Errors;
                }
            }

            return Result.Success;
        }
    }
}
