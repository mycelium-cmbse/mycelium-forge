// ------------------------------------------------------------------------------------------------
// <copyright file="NamespaceValidator.cs" company="Starion Group S.A.">
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
    /// Partial class extending <see cref="NamespaceValidator{T}" /> with custom validation rules.
    /// </summary>
    /// <typeparam name="T">The <see cref="INamespace" /> type to validate.</typeparam>
    public abstract partial class NamespaceValidator<T> where T : class, INamespace
    {
        /// <summary>
        /// Regular expression used to validate namespace short name format.
        /// </summary>
        /// <returns>A compiled <see cref="Regex" /> instance.</returns>
        [GeneratedRegex("^[a-z0-9]([a-z0-9-]*[a-z0-9])?$", RegexOptions.None, 1000)]
        public static partial Regex ShortNameFormatRegex();

        /// <summary>
        /// Adds custom validation rules for the <see cref="INamespace" /> entity.
        /// </summary>
        protected override void AddCustomValidation()
        {
            base.AddCustomValidation();
            this.RuleFor(x => x.ShortName).Matches(ShortNameFormatRegex()).WithMessage("Short name must start and end with an alphanumeric character and contain only lowercase alphanumeric characters and hyphens.");
        }
    }
}
