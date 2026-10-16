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
        /// Initializes a new instance of the <see cref="DtoValidatorBase{T}" /> class.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="AddCustomValidation" /> to allow derived and partial validator classes to register custom
        /// FluentValidation rules during instantiation.
        /// </remarks>
        protected DtoValidatorBase()
        {
            // Safe virtual invocation, rule registration does not depend on uninitialized state.
            // ReSharper disable once VirtualMemberCallInConstructor
            this.AddCustomValidation();
        }

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

        /// <summary>
        /// Adds possibilities for custom validation rules.
        /// </summary>
        /// <remarks>
        /// Overriding implementations in handwritten partial validator classes should register custom rules
        /// and call <c>base.AddCustomValidation()</c> to preserve rules defined across the inheritance chain.
        /// </remarks>
        protected virtual void AddCustomValidation()
        {
        }
    }
}
