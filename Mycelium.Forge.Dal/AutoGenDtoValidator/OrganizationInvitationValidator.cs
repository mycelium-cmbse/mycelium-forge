// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationValidator.cs" company="Starion Group S.A.">
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
    /// DTO validator class for the <see cref="OrganizationInvitation"/> class.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "0.5.0")]
    public partial class OrganizationInvitationValidator : InvitationValidator<IOrganizationInvitation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationInvitationValidator"/> class.
        /// </summary>
        public OrganizationInvitationValidator()
        {
            this.RuleFor(x => x.Organization).NotEmpty();
            this.RuleFor(x => x.OrganizationInvitationKind).NotNull();
            this.RuleFor(x => x.Owner).NotEmpty();
            this.RuleFor(x => x.Target).NotEmpty();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
