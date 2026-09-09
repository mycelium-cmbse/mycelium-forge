// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Extensions
{
    using ErrorOr;

    using FluentValidation.Results;

    /// <summary>
    /// Provides extension methods for <see cref="ValidationResult" /> instances.
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Converts a <see cref="ValidationResult" /> to an <see cref="ErrorOr{Success}" />.
        /// </summary>
        /// <param name="validationResult">The <see cref="ValidationResult" /> to convert.</param>
        /// <returns>A <see cref="Result.Success" /> if valid; otherwise, a list of <see cref="Error.Validation" /> errors.</returns>
        public static ErrorOr<Success> ToErrorOr(this ValidationResult validationResult)
        {
            ArgumentNullException.ThrowIfNull(validationResult);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ConvertAll(error => Error.Validation(error.PropertyName, error.ErrorMessage));
            }

            return Result.Success;
        }
    }
}
