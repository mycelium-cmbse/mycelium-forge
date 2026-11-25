// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeValidator.cs" company="Starion Group S.A.">
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
    /// Partial class extending <see cref="ScopeValidator{T}" /> with custom validation rules.
    /// </summary>
    /// <typeparam name="T">The <see cref="IScope" /> type to validate.</typeparam>
    public abstract partial class ScopeValidator<T> where T : class, IScope
    {
        /// <summary>
        /// Regular expression used to validate the email address format.
        /// </summary>
        /// <returns>A compiled <see cref="Regex" /> instance.</returns>
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.None, 1000)]
        public static partial Regex EmailFormatRegex();

        /// <summary>
        /// Determines whether the specified value is a valid absolute http or https URL.
        /// </summary>
        /// <param name="website">The website value to validate.</param>
        /// <returns><c>true</c> if the value is a valid absolute http or https URL; otherwise, <c>false</c>.</returns>
        public static bool IsValidWebsite(string website)
        {
            return Uri.TryCreate(website, UriKind.Absolute, out var uri) && !string.IsNullOrWhiteSpace(uri.Host) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Adds custom validation rules for the <see cref="IScope" /> entity.
        /// </summary>
        partial void AddCustomValidation()
        {
            this.RuleFor(x => x.Email).Matches(EmailFormatRegex()).WithMessage("Email must be a valid email address.");
            this.RuleFor(x => x.BillingEmail).Matches(EmailFormatRegex()).When(x => !string.IsNullOrEmpty(x.BillingEmail)).WithMessage("Billing email must be a valid email address.");
            this.RuleFor(x => x.Website).Must(IsValidWebsite).When(x => !string.IsNullOrEmpty(x.Website)).WithMessage("Website must be a valid absolute http or https URL.");
        }
    }
}
