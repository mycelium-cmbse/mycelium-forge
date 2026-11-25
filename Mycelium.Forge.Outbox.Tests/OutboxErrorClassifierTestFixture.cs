// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxErrorClassifierTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox.Tests
{
    using ErrorOr;

    using NUnit.Framework;

    [TestFixture]
    public class OutboxErrorClassifierTestFixture
    {
        [Test]
        public void VerifyIsPermanentError()
        {
            var validationError = Error.Validation("Outbox.InvalidPayload", "Invalid payload");
            var notFoundError = Error.NotFound("Keycloak.SyncFailed", "Not found");
            var conflictError = Error.Conflict("Keycloak.SyncFailed", "Conflict");
            var unauthorizedError = Error.Unauthorized("Keycloak.SyncFailed", "Unauthorized");
            var forbiddenError = Error.Forbidden("Keycloak.SyncFailed", "Forbidden");
            var failureError = Error.Failure("Keycloak.SyncFailed", "Service unavailable");
            var unexpectedError = Error.Unexpected("Keycloak.SyncFailed", "Unexpected error");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(OutboxErrorClassifier.IsPermanentError(null), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([validationError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([notFoundError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([conflictError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([unauthorizedError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([forbiddenError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([failureError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([unexpectedError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([failureError, validationError]), Is.True);
            }
        }
    }
}
