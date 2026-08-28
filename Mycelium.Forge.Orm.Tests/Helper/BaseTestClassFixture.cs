// ------------------------------------------------------------------------------------------------
// <copyright file="BaseTestClassFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Tests.Helper
{
    using FluentResults;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Serializer.Json;

    using Npgsql;

    using NUnit.Framework;

    using Serilog;

    using Testcontainers.PostgreSql;

    /// <summary>
    /// Base test fixture providing a PostgreSQL test container, schema initialization, and logger factory.
    /// </summary>
    public abstract class BaseTestClassFixture
    {
        private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
            .WithImage("postgres:18.0-alpine3.22")
            .Build();

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// 
        protected ISerializer Serializer;

        /// <summary>
        /// Gets the test logger factory.
        /// </summary>
        protected ILoggerFactory TestLoggerFactory;

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        protected string ConnectionString { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the active PostgreSQL connection.
        /// </summary>
        public NpgsqlConnection Connection { get; private set; }

        /// <summary>
        /// Initializes the test container, connection, and logging before running tests.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [OneTimeSetUp]
        public async Task InitializeTests()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            this.TestLoggerFactory = LoggerFactory.Create(builder => { builder.AddSerilog(); });

            this.Serializer = new Serializer();

            await this.postgres.StartAsync();

            this.ConnectionString = this.postgres.GetConnectionString();
            this.Connection = new NpgsqlConnection(this.ConnectionString);
            await this.Connection.OpenAsync();
        }

        /// <summary>
        /// Drops and recreates the database schema before each test.
        /// </summary>
        [SetUp]
        public async Task CleanDataBase()
        {
            await using var sqlRemoveCommand = new NpgsqlCommand("DROP SCHEMA IF EXISTS \"Forge\" CASCADE;", this.Connection);
            sqlRemoveCommand.ExecuteNonQuery();

            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "Script0001_InitialSchema.sql");
            var schemaSql = await File.ReadAllTextAsync(path);

            await using var sqlCreateCommand = new NpgsqlCommand(schemaSql, this.Connection);
            sqlCreateCommand.ExecuteNonQuery();

            await this.PostSetup();
        }

        /// <summary>
        /// Cleans up the connection and stops the PostgreSQL test container after all tests in the fixture run.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [OneTimeTearDown]
        public async Task Cleanup()
        {
            this.TestLoggerFactory?.Dispose();

            if (this.Connection != null)
            {
                await this.Connection.CloseAsync();
                await this.Connection.DisposeAsync();
            }

            await this.postgres.DisposeAsync();
        }

        /// <summary>
        /// Invoked after setup has completed.
        /// </summary>
        /// <returns>A <see cref="Task" /></returns>
        protected virtual Task PostSetup()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes an action inside a database transaction, committing on success and rolling back on failure.
        /// </summary>
        /// <param name="action">The database action taking the transaction.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        protected async Task Insert(Func<NpgsqlTransaction, Task<Result>> action)
        {
            await using var transaction = await this.Connection.BeginTransactionAsync();

            try
            {
                var result = await action.Invoke(transaction);

                if (result.IsFailed)
                {
                    Assert.Fail(result.ToString());
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
