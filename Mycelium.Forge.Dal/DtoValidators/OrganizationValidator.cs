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
    using System.Text.RegularExpressions;

    using FluentValidation;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Partial class extending <see cref="OrganizationValidator" /> with custom validation rules.
    /// </summary>
    /// <remarks>
    /// Move this rule upward in the hierarchy of validators once [Validators]: Create hierarchy for the validators (#161) is
    /// implemented.
    /// </remarks>
    public sealed partial class OrganizationValidator
    {
        /// <summary>
        /// Regular expression used to validate organization scope format.
        /// </summary>
        /// <returns>A compiled <see cref="Regex" /> instance.</returns>
        [GeneratedRegex("^[a-z0-9][a-z0-9.-]*$", RegexOptions.None, 1000)]
        public static partial Regex ScopeFormatRegex();

        /// <summary>
        /// Adds custom validation rules for the <see cref="IOrganization" /> entity.
        /// </summary>
        /// <remarks>
        /// Move this rule upward in the hierarchy of validators once [Validators]: Create hierarchy for the validators (#161) is
        /// implemented.
        /// </remarks>
        public override void AddCustomValidation()
        {
            this.RuleFor(x => x.ShortName).Matches(ScopeFormatRegex()).WithMessage("Scope must start with an alphanumeric character and contain only lowercase alphanumeric characters, dots, and hyphens.");
        }
    }
}
