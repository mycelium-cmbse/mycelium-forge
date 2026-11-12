// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionValidator.cs" company="Starion Group S.A.">
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
    /// Partial class extending <see cref="PackageVersionValidator" /> with custom validation rules.
    /// </summary>
    public partial class PackageVersionValidator
    {
        /// <summary>
        /// Adds custom validation rules for the <see cref="IPackageVersion" /> entity.
        /// </summary>
        partial void AddCustomValidation()
        {
            this.RuleFor(x => x.Version).Matches(SemanticVersion.VersionFormatRegex()).WithMessage("Version must follow SemVer 2.0.0 format (e.g. 1.0.0).");
        }
    }
}
