// ------------------------------------------------------------------------------------------------
// <copyright file="CountryValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Country"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class CountryValidator : ThingValidator<ICountry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryValidator"/> class.
        /// </summary>
        public CountryValidator()
        {
            this.RuleFor(x => x.Alpha2Code).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Alpha3Code).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Name).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.NumericCode).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Owner).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="Country"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
