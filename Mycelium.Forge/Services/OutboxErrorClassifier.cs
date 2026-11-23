// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxErrorClassifier.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    /// <summary>
    /// Classifies outbox handler failures as permanent or transient to control immediate and durable retries.
    /// </summary>
    public static class OutboxErrorClassifier
    {
        /// <summary>
        /// The error metadata key storing an HTTP status code for transient versus permanent error classification.
        /// </summary>
        public const string StatusCodeMetadataKey = "StatusCode";

        /// <summary>
        /// Determines whether the supplied errors indicate a permanent failure that must not be retried.
        /// </summary>
        /// <param name="errors">The collection of errors returned by an outbox handler.</param>
        /// <returns><see langword="true" /> if the failure is permanent; otherwise <see langword="false" />.</returns>
        public static bool IsPermanentError(IEnumerable<Error> errors)
        {
            if (errors == null)
            {
                return false;
            }

            foreach (var error in errors)
            {
                if (error.Type == ErrorType.Validation)
                {
                    return true;
                }

                if (error.Metadata == null || !error.Metadata.TryGetValue(StatusCodeMetadataKey, out var statusCodeValue) || statusCodeValue is not int statusCode)
                {
                    continue;
                }

                if (statusCode is >= 400 and < 500 and not 408 and not 429)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the supplied errors indicate a transient failure eligible for retry.
        /// </summary>
        /// <param name="errors">The collection of errors returned by an outbox handler.</param>
        /// <returns><see langword="true" /> if the failure is transient; otherwise <see langword="false" />.</returns>
        public static bool IsTransientError(IEnumerable<Error> errors)
        {
            return !IsPermanentError(errors);
        }
    }
}
