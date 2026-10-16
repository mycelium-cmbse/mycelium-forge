// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Organization"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class OrganizationValidator : ScopeValidator<IOrganization>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationValidator"/> class.
        /// </summary>
        public OrganizationValidator()
        {
            this.RuleFor(x => x.Administrator).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.Administrator).NotEmpty();
            this.RuleFor(x => x.Member).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.Member).NotEmpty();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="Organization"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
