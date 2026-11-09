// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationStatusExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using BlazorBlueprint.Components;

    using Mycelium.Forge.Enums;

    /// <summary>
    /// Provides extension methods for <see cref="ValidationStatus" /> values.
    /// </summary>
    public static class ValidationStatusExtensions
    {
        /// <param name="status">The validation check status outcome.</param>
        extension(ValidationStatus status)
        {
            /// <summary>
            /// Resolves the badge visual variant for a validation status outcome.
            /// </summary>
            /// <returns>The corresponding <see cref="BadgeVariant" />.</returns>
            public BadgeVariant ToBadgeVariant()
            {
                return status switch
                {
                    ValidationStatus.Pass => BadgeVariant.SoftSuccess,
                    ValidationStatus.Fail => BadgeVariant.SoftDestructive,
                    ValidationStatus.Warning => BadgeVariant.SoftWarning,
                    ValidationStatus.Missing => BadgeVariant.Secondary,
                    _ => BadgeVariant.Outline
                };
            }
        }
    }
}
