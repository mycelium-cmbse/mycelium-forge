// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="IScope"/> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="IScope"/> type to validate.</typeparam>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public abstract partial class ScopeValidator<T> : NamespaceValidator<T> where T : class, IScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeValidator{T}"/> class.
        /// </summary>
        protected ScopeValidator()
        {
            this.RuleForEach(x => x.Address).NotEmpty();
            this.RuleFor(x => x.DefaultPackageVisibility).NotNull();
            this.RuleFor(x => x.Email).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Origin).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleForEach(x => x.OwnedPackage).NotEmpty();
            this.RuleFor(x => x.PrimaryAddress).Must(value => !value.HasValue || value.Value != Guid.Empty);
            this.RuleForEach(x => x.ProfileLink).NotEmpty();
            this.RuleFor(x => x.Status).NotNull();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
