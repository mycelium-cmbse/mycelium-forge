// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Forge"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class ForgeValidator : NamespaceValidator<IForge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForgeValidator"/> class.
        /// </summary>
        public ForgeValidator()
        {
            this.RuleFor(x => x.Account).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.Account).NotEmpty();
            this.RuleFor(x => x.Administrator).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.Administrator).NotEmpty();
            this.RuleForEach(x => x.Country).NotEmpty();
            this.RuleFor(x => x.Description).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleForEach(x => x.Organization).NotEmpty();
            this.RuleForEach(x => x.PackageType).NotEmpty();
            this.RuleForEach(x => x.ProfileType).NotEmpty();
            this.RuleForEach(x => x.Tag).NotEmpty();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
