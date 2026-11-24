// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationValidator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.AutoGenDtoValidator
{
    using FluentValidation;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Partial class extending <see cref="OrganizationValidator" /> with custom validation rules.
    /// </summary>
    public partial class OrganizationValidator
    {
        /// <summary>
        /// Adds custom validation rules for the <see cref="IOrganization" /> entity.
        /// </summary>
        partial void AddCustomValidation()
        {
            this.RuleFor(x => x.BillingEmail).Matches(EmailFormatRegex()).When(x => !string.IsNullOrEmpty(x.BillingEmail)).WithMessage("Billing email must be a valid email address.");
        }
    }
}
