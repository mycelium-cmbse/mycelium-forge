// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxErrorClassifierTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System.Collections.Generic;

    using ErrorOr;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class OutboxErrorClassifierTestFixture
    {
        [Test]
        public void VerifyIsPermanentError()
        {
            var validationError = Error.Validation("Outbox.InvalidPayload", "Invalid payload");
            var badRequestError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Bad request", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 400 } });
            var unauthorizedError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Unauthorized", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 401 } });
            var notFoundError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Not found", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 404 } });
            var requestTimeoutError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Timeout", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 408 } });
            var tooManyRequestsError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Rate limited", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 429 } });
            var serverError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Service unavailable", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 503 } });
            var nonIntStatusCodeError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Non-int status", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, "400" } });
            var plainFailureError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Network failure");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(OutboxErrorClassifier.IsPermanentError(null!), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([validationError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([badRequestError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([unauthorizedError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([notFoundError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsPermanentError([requestTimeoutError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([tooManyRequestsError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([serverError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([nonIntStatusCodeError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([plainFailureError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsPermanentError([serverError, badRequestError]), Is.True);
            }
        }

        [Test]
        public void VerifyIsTransientError()
        {
            var validationError = Error.Validation("Outbox.InvalidPayload", "Invalid payload");
            var badRequestError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Bad request", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 422 } });
            var tooManyRequestsError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Rate limited", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 429 } });
            var serverError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Internal error", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 500 } });
            var plainFailureError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Connection refused");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(OutboxErrorClassifier.IsTransientError(null!), Is.True);
                Assert.That(OutboxErrorClassifier.IsTransientError([]), Is.True);
                Assert.That(OutboxErrorClassifier.IsTransientError([validationError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsTransientError([badRequestError]), Is.False);
                Assert.That(OutboxErrorClassifier.IsTransientError([tooManyRequestsError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsTransientError([serverError]), Is.True);
                Assert.That(OutboxErrorClassifier.IsTransientError([plainFailureError]), Is.True);
            }
        }
    }
}
