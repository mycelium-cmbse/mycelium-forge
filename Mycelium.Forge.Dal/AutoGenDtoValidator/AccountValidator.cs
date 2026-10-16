// ------------------------------------------------------------------------------------------------
// <copyright file="AccountValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Account"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class AccountValidator : ScopeValidator<IAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountValidator"/> class.
        /// </summary>
        public AccountValidator()
        {
            this.RuleForEach(x => x.ApiKey).NotEmpty();
            this.RuleForEach(x => x.OwnedOrganizationInvitation).NotEmpty();
            this.RuleFor(x => x.OwnedPackageInvitation).NotEmpty();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="Account"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
