// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="License"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
    public partial class LicenseValidator : NamespaceValidator<ILicense>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseValidator"/> class.
        /// </summary>
        public LicenseValidator()
        {
            this.RuleFor(x => x.LicenseText).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.SpdxIdentifier).Must(value => !string.IsNullOrWhiteSpace(value));
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="License"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
