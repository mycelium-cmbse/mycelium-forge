// ------------------------------------------------------------------------------------------------
// <copyright file="DtoValidatorBase.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.DtoValidator
{
    using System.Linq.Expressions;

    using ErrorOr;

    using FluentValidation;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Extensions;

    /// <summary>
    /// Abstract base class for DTO validators.
    /// </summary>
    /// <typeparam name="T">The <see cref="IThing" /> type to validate.</typeparam>
    public abstract class DtoValidatorBase<T> : AbstractValidator<T>, IDtoValidator<T> where T : class, IThing
    {
        /// <summary>
        /// Asynchronously checks if a DTO contains valid data.
        /// </summary>
        /// <param name="dto">The DTO instance to validate.</param>
        /// <returns>
        /// A new <see cref="ErrorOr{Success}" /> indicating whether validation was successful.
        /// </returns>
        public virtual async Task<ErrorOr<Success>> ValidateDto(T dto)
        {
            var validationResult = await this.ValidateAsync(dto);
            return validationResult.ToErrorOr();
        }

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data for the specified fields.
        /// </summary>
        /// <param name="dto">The DTO instance to validate.</param>
        /// <param name="fields">The fields to validate.</param>
        /// <returns>
        /// A new <see cref="ErrorOr{Success}" /> indicating whether validation was successful.
        /// </returns>
        public virtual async Task<ErrorOr<Success>> ValidateFields(T dto, params Expression<Func<T, object>>[] fields)
        {
            var validationResult = await this.ValidateAsync(dto, options => options.IncludeProperties(fields));
            return validationResult.ToErrorOr();
        }
    }
}
