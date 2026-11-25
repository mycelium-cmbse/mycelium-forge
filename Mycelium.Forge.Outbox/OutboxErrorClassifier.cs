// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxErrorClassifier.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox
{
    using ErrorOr;

    /// <summary>
    /// Classifies outbox handler failures as permanent or transient from <see cref="ErrorType" /> to control durable retries.
    /// </summary>
    public static class OutboxErrorClassifier
    {
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

            return errors.Any(error => error.Type is ErrorType.Validation or ErrorType.NotFound or ErrorType.Conflict);
        }
    }
}
