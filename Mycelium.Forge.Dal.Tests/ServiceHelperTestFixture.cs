// ------------------------------------------------------------------------------------------------
// <copyright file="ServiceHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Common.Comparers;

    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="ServiceHelper" /> class.
    /// </summary>
    [TestFixture]
    public class ServiceHelperTestFixture
    {
        /// <summary>
        /// The mock logger used in tests.
        /// </summary>
        private Mock<ILogger> loggerMock;

        /// <summary>
        /// The mock DTO comparer used in tests.
        /// </summary>
        private Mock<IDtoComparer<IAPIKey>> comparerMock;

        /// <summary>
        /// Sets up mock dependencies before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger>();
            this.comparerMock = new Mock<IDtoComparer<IAPIKey>>();
        }

        /// <summary>
        /// Verifies that <see cref="ServiceHelper.FormatChangeValue" /> correctly formats different data types into string
        /// representations.
        /// </summary>
        [Test]
        public void VerifyFormatChangeValue()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ServiceHelper.FormatChangeValue(null), Is.EqualTo("null"));
                Assert.That(ServiceHelper.FormatChangeValue(string.Empty), Is.EqualTo("\"\""));
                Assert.That(ServiceHelper.FormatChangeValue("custom-string"), Is.EqualTo("\"custom-string\""));
                Assert.That(ServiceHelper.FormatChangeValue(42), Is.EqualTo("42"));
                Assert.That(ServiceHelper.FormatChangeValue(new List<string> { "first", "second" }), Is.EqualTo("[\"first\", \"second\"]"));
                Assert.That(ServiceHelper.FormatChangeValue(new List<object> { null, string.Empty, "item", 100 }), Is.EqualTo("[null, \"\", \"item\", 100]"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="ServiceHelper.LogChanges{T}" /> logs property changes when differences exist, does not log
        /// when
        /// differences are empty, ignores logging when log level is disabled, and throws when null arguments are passed.
        /// </summary>
        [Test]
        public void VerifyLogChanges()
        {
            var originalApiKey = new APIKey { Id = Guid.NewGuid(), Name = "OriginalName" };
            var updatedApiKey = new APIKey { Id = originalApiKey.Id, Name = "UpdatedName" };

            using (Assert.EnterMultipleScope())
            {
                Assert.Throws<ArgumentNullException>(() => ServiceHelper.LogChanges(null, this.comparerMock.Object, originalApiKey, updatedApiKey));
                Assert.Throws<ArgumentNullException>(() => ServiceHelper.LogChanges<IAPIKey>(this.loggerMock.Object, null, originalApiKey, updatedApiKey));
            }

            // Case: Log level is not enabled
            this.loggerMock.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(false);
            ServiceHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            this.comparerMock.Verify(x => x.Compare(It.IsAny<IAPIKey>(), It.IsAny<IAPIKey>()), Times.Never);

            // Case: Log level is enabled, but no changes exist
            this.loggerMock.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
            this.comparerMock.Setup(x => x.Compare(originalApiKey, updatedApiKey)).Returns([]);

            ServiceHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            this.loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);

            // Case: Log level is enabled, changes exist and are logged
            var propertyChanges = new List<PropertyChange>
            {
                new(nameof(IAPIKey.Name), "OriginalName", "UpdatedName")
            };

            this.comparerMock.Setup(x => x.Compare(originalApiKey, updatedApiKey)).Returns(propertyChanges);

            ServiceHelper.LogChanges(this.loggerMock.Object, this.comparerMock.Object, originalApiKey, updatedApiKey);

            this.loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
