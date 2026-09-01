// ------------------------------------------------------------------------------------------------
// <copyright file="AddressValidator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.AutoGenDtoValidator
{
    using System.CodeDom.Compiler;
    using System.Linq.Expressions;

    using FluentResults;

    using FluentValidation;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.DtoValidator;

    /// <summary>
    /// DTO validator class for the <see cref="Address"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public sealed partial class AddressValidator : DtoValidatorBase<IAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidator"/> class.
        /// </summary>
        public AddressValidator()
        {
            this.RuleFor(x => x.AddressLine1).NotEmpty();
            this.RuleFor(x => x.Country).NotEmpty();
            this.RuleFor(x => x.CreatedAt).NotEmpty();
            this.RuleFor(x => x.Id).NotNull();
            this.RuleFor(x => x.Locality).NotEmpty();
            this.RuleFor(x => x.ModifiedAt).NotEmpty();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data.
        /// </summary>
        /// <param name="dto">The <see cref="IAddress"/> to validate.</param>
        /// <returns>
        /// A new <see cref="Result"/> indicating whether the validation was successful.
        /// </returns>
        public override async Task<Result> ValidateDto(IAddress dto)
        {
            var validationResult = await this.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return Result.Fail(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            return Result.Ok();
        }

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data for the specified fields.
        /// </summary>
        /// <param name="dto">The <see cref="IAddress"/> to validate.</param>
        /// <param name="fields">The fields to validate.</param>
        /// <returns>
        /// A new <see cref="Result"/> indicating whether the validation was successful.
        /// </returns>
        public override async Task<Result> ValidateFields(IAddress dto, params Expression<Func<IAddress, object>>[] fields)
        {
            var validationResult = await this.ValidateAsync(dto, options => options.IncludeProperties(fields));

            if (!validationResult.IsValid)
            {
                return Result.Fail(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            return Result.Ok();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
