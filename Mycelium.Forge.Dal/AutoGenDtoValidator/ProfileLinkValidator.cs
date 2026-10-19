// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLinkValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="ProfileLink"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class ProfileLinkValidator : ThingValidator<IProfileLink>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileLinkValidator"/> class.
        /// </summary>
        public ProfileLinkValidator()
        {
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleFor(x => x.ProfileType).NotEmpty();
            this.RuleFor(x => x.Uri).Must(value => !string.IsNullOrWhiteSpace(value));
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="ProfileLink"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
