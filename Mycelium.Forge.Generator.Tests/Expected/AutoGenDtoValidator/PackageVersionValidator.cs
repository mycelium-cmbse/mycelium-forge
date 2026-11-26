// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="PackageVersion"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.6.0")]
    public partial class PackageVersionValidator : ThingValidator<IPackageVersion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionValidator"/> class.
        /// </summary>
        public PackageVersionValidator()
        {
            this.RuleForEach(x => x.ApplicableLicenses).NotEmpty();
            this.RuleFor(x => x.CheckSum).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.DownloadCount).NotNull();
            this.RuleFor(x => x.FileExtension).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.Listed).NotNull();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleFor(x => x.PublicationDate).NotEmpty();
            this.RuleFor(x => x.SizeInBytes).NotNull();
            this.RuleFor(x => x.Version).Must(value => !string.IsNullOrWhiteSpace(value));
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="PackageVersion"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
