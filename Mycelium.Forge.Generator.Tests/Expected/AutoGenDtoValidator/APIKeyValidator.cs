// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyValidator.cs" company="Starion Group S.A.">
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

    using ErrorOr;

    using FluentValidation;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.DtoValidator;
    using Mycelium.Forge.Dal.Extensions;

    /// <summary>
    /// DTO validator class for the <see cref="APIKey"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public sealed partial class APIKeyValidator : DtoValidatorBase<IAPIKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIKeyValidator"/> class.
        /// </summary>
        public APIKeyValidator()
        {
            this.RuleFor(x => x.CreatedAt).NotEmpty();
            this.RuleFor(x => x.ExpiresAt).NotEmpty();
            this.RuleFor(x => x.Id).NotNull();
            this.RuleFor(x => x.LastUsedAt).NotEmpty();
            this.RuleFor(x => x.ModifiedAt).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleFor(x => x.Permissions).NotEmpty();
            this.RuleFor(x => x.RevokedAt).NotEmpty();
            this.RuleFor(x => x.SecretHash).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data.
        /// </summary>
        /// <param name="dto">The <see cref="IAPIKey"/> to validate.</param>
        /// <returns>
        /// A new <see cref="ErrorOr{Success}"/> indicating whether the validation was successful.
        /// </returns>
        public override async Task<ErrorOr<Success>> ValidateDto(IAPIKey dto)
        {
            var validationResult = await this.ValidateAsync(dto);
            return validationResult.ToErrorOr();
        }

        /// <summary>
        /// Asynchronously checks if a DTO contains valid data for the specified fields.
        /// </summary>
        /// <param name="dto">The <see cref="IAPIKey"/> to validate.</param>
        /// <param name="fields">The fields to validate.</param>
        /// <returns>
        /// A new <see cref="ErrorOr{Success}"/> indicating whether the validation was successful.
        /// </returns>
        public override async Task<ErrorOr<Success>> ValidateFields(IAPIKey dto, params Expression<Func<IAPIKey, object>>[] fields)
        {
            var validationResult = await this.ValidateAsync(dto, options => options.IncludeProperties(fields));
            return validationResult.ToErrorOr();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
