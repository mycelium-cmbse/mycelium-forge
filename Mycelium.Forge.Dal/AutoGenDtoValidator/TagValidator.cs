// ------------------------------------------------------------------------------------------------
// <copyright file="TagValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Tag"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
    public partial class TagValidator : ThingValidator<ITag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagValidator"/> class.
        /// </summary>
        public TagValidator()
        {
            this.RuleFor(x => x.Name).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Owner).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="Tag"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
