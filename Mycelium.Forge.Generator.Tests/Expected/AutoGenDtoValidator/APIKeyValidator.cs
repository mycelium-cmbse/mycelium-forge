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

    using FluentValidation;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.DtoValidator;

    /// <summary>
    /// DTO validator class for the <see cref="APIKey"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class APIKeyValidator : ThingValidator<IAPIKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIKeyValidator"/> class.
        /// </summary>
        public APIKeyValidator()
        {
            this.RuleFor(x => x.ExpiresAt).NotEmpty();
            this.RuleFor(x => x.LastUsedAt).NotEmpty();
            this.RuleFor(x => x.Name).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleForEach(x => x.Permissions).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.RevokedAt).NotEmpty();
            this.RuleFor(x => x.SecretHash).Must(value => value is { Count: >= 1 });
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="APIKey"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
