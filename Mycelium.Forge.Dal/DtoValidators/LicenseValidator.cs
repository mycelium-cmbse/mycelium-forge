// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidator.cs" company="Starion Group S.A.">
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
    /// Partial class extending <see cref="LicenseValidator" /> with custom validation rules.
    /// </summary>
    public partial class LicenseValidator
    {
        /// <summary>
        /// Regular expression used to validate SPDX license identifier format.
        /// </summary>
        /// <returns>A compiled <see cref="Regex" /> instance.</returns>
        /// <remarks>
        /// Follows the SPDX ABNF <c>idstring = 1*(ALPHA / DIGIT / "-" / ".")</c> rule with an alphanumeric leading
        /// character; document-scoped <c>DocumentRef-&lt;idstring&gt;:</c> prefixes are intentionally excluded for
        /// registry-level license entities.
        /// See <see href="https://spdx.github.io/spdx-spec/v2.3/SPDX-license-expressions/" />.
        /// </remarks>
        [GeneratedRegex("^[A-Za-z0-9][A-Za-z0-9.-]*$", RegexOptions.None, 1000)]
        public static partial Regex SpdxIdentifierFormatRegex();

        /// <summary>
        /// Adds custom validation rules for the <see cref="ILicense" /> entity.
        /// </summary>
        partial void AddCustomValidation()
        {
            this.RuleFor(x => x.SpdxIdentifier).Matches(SpdxIdentifierFormatRegex()).WithMessage("SPDX identifier must start with an alphanumeric character and contain only ASCII letters, digits, dots, and hyphens.");
        }
    }
}
