// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="Package"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class PackageValidator : NamespaceValidator<IPackage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageValidator"/> class.
        /// </summary>
        public PackageValidator()
        {
            this.RuleFor(x => x.Description).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Listed).NotNull();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleForEach(x => x.PackageMaintainer).NotEmpty();
            this.RuleFor(x => x.PackageOwner).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.PackageOwner).NotEmpty();
            this.RuleFor(x => x.PackageType).NotEmpty();
            this.RuleFor(x => x.Version).Must(value => value is { Count: >= 1 });
            this.RuleForEach(x => x.Version).NotEmpty();
            this.RuleFor(x => x.Visibility).NotNull();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
