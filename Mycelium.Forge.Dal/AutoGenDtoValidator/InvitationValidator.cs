// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="IInvitation"/> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="IInvitation"/> type to validate.</typeparam>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public abstract partial class InvitationValidator<T> : ThingValidator<T> where T : class, IInvitation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationValidator{T}"/> class.
        /// </summary>
        protected InvitationValidator()
        {
            this.RuleFor(x => x.ExperiesAt).NotEmpty();
            this.RuleFor(x => x.Status).NotNull();
            this.AddCustomValidation();
        }

        /// <summary>
        /// Adds possibilities for custom validation rules for the <see cref="IInvitation"/> entity.
        /// </summary>
        partial void AddCustomValidation();
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
