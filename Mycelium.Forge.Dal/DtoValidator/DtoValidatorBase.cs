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

    using FluentResults;

    using FluentValidation;

    using Mycelium.Forge.Common;

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
        /// A new <see cref="Result" /> indicating whether validation was successful.
        /// </returns>
        public abstract Task<Result> ValidateDto(T dto);

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data for the specified fields.
        /// </summary>
        /// <param name="dto">The DTO instance to validate.</param>
        /// <param name="fields">The fields to validate.</param>
        /// <returns>
        /// A new <see cref="Result" /> indicating whether validation was successful.
        /// </returns>
        public abstract Task<Result> ValidateFields(T dto, params Expression<Func<T, object>>[] fields);

        /// <summary>
        /// Adds possibilities for custom validation rules.
        /// </summary>
        public virtual void AddCustomValidation()
        {
        }
    }
}
