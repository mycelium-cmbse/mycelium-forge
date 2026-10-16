// ------------------------------------------------------------------------------------------------
// <copyright file="NamespaceValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="INamespace"/> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="INamespace"/> type to validate.</typeparam>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public abstract partial class NamespaceValidator<T> : ThingValidator<T> where T : class, INamespace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceValidator{T}"/> class.
        /// </summary>
        protected NamespaceValidator()
        {
            this.RuleFor(x => x.Name).Must(value => !string.IsNullOrWhiteSpace(value));
            this.RuleFor(x => x.ShortName).Must(value => !string.IsNullOrWhiteSpace(value));
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
