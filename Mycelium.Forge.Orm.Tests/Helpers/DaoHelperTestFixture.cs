// ------------------------------------------------------------------------------------------------
// <copyright file="DaoHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Tests.Helpers
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Comparers;
    using Mycelium.Forge.Orm.Helpers;

    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="DaoHelper" /> class.
    /// </summary>
    [TestFixture]
    public class DaoHelperTestFixture
    {
        private Mock<ILogger> loggerMock;
        private Mock<IDtoComparer<IAPIKey>> comparerMock;

        /// <summary>
        /// Sets up the test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger>();
            this.comparerMock = new Mock<IDtoComparer<IAPIKey>>();
        }

        /// <summary>
        /// Verifies that <see cref="DaoHelper.FormatChangeValue" /> correctly formats different data types into string
        /// representations for logging.
        /// </summary>
        [Test]
        public void VerifyFormatChangeValue()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(DaoHelper.FormatChangeValue(null), Is.EqualTo("null"));
                Assert.That(DaoHelper.FormatChangeValue(string.Empty), Is.EqualTo("\"\""));
                Assert.That(DaoHelper.FormatChangeValue("custom-string"), Is.EqualTo("\"custom-string\""));
                Assert.That(DaoHelper.FormatChangeValue(42), Is.EqualTo("42"));
                Assert.That(DaoHelper.FormatChangeValue(new List<string> { "first", "second" }), Is.EqualTo("[\"first\", \"second\"]"));
                Assert.That(DaoHelper.FormatChangeValue(new List<object> { null, string.Empty, "item", 100 }), Is.EqualTo("[null, \"\", \"item\", 100]"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="DaoHelper.LogChanges{T}" /> logs property changes when differences exist, does not log when
        /// logging is disabled or there are no differences, and validates arguments.
        /// </summary>
        [Test]
        [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Moq expression tree in unit test.")]
        public void VerifyLogChanges()
        {
            var entityId = Guid.NewGuid();
            var originalApiKey = new APIKey { Id = entityId, Name = "OriginalKey" };
            var updatedApiKey = new APIKey { Id = entityId, Name = "UpdatedKey" };

            using (Assert.EnterMultipleScope())
            {
                Assert.Throws<ArgumentNullException>(() => DaoHelper.LogChanges(null!, this.comparerMock.Object, originalApiKey, updatedApiKey));
                Assert.Throws<ArgumentNullException>(() => DaoHelper.LogChanges<IAPIKey>(this.loggerMock.Object, null!, originalApiKey, updatedApiKey));
            }

            this.loggerMock.Setup(logger => logger.IsEnabled(LogLevel.Information)).Returns(false);

            DaoHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            using (Assert.EnterMultipleScope())
            {
                this.comparerMock.Verify(comparer => comparer.Compare(It.IsAny<IAPIKey>(), It.IsAny<IAPIKey>()), Times.Never);

                this.loggerMock.Verify(
                    logger => logger.Log(
                        It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Never);
            }

            this.loggerMock.Setup(logger => logger.IsEnabled(LogLevel.Information)).Returns(true);
            this.comparerMock.Setup(comparer => comparer.Compare(originalApiKey, updatedApiKey)).Returns([]);

            DaoHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            this.loggerMock.Verify(
                logger => logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);

            var propertyChanges = new List<PropertyChange>
            {
                new(nameof(IAPIKey.Name), "OriginalKey", "UpdatedKey"),
                new(nameof(IAPIKey.ExpiresAt), null, new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)),
                new(nameof(IAPIKey.SecretHash), new List<byte> { 1, 2 }, new List<byte> { 3, 4 })
            };

            this.comparerMock.Setup(comparer => comparer.Compare(originalApiKey, updatedApiKey)).Returns(propertyChanges);

            var loggedMessage = string.Empty;

            this.loggerMock
                .Setup(logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var state = invocation.Arguments[2];
                    loggedMessage = state.ToString()!;
                }));

            DaoHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            using (Assert.EnterMultipleScope())
            {
                this.loggerMock.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);

                Assert.That(loggedMessage, Does.Contain($"Updating {nameof(IAPIKey)} {entityId} with 3 changes:"));
                Assert.That(loggedMessage, Does.Contain($"   -> {nameof(IAPIKey.Name)} changed from \"OriginalKey\" to \"UpdatedKey\""));
                Assert.That(loggedMessage, Does.Contain($"   -> {nameof(IAPIKey.ExpiresAt)} changed from null to"));
                Assert.That(loggedMessage, Does.Contain($"   -> {nameof(IAPIKey.SecretHash)} changed from [1, 2] to [3, 4]"));
            }
        }
    }
}
