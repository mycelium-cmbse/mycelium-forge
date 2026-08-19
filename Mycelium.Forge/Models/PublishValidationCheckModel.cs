// ------------------------------------------------------------------------------------------------
// <copyright file="PublishValidationCheckModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an individual pre-publishing validation rule evaluation result.
    /// </summary>
    /// <param name="Title">The human-readable label of the validation check.</param>
    /// <param name="StatusText">The badge display text representing the outcome.</param>
    /// <param name="Status">The evaluation status outcome value.</param>
    public record PublishValidationCheckModel(
        string Title,
        string StatusText,
        PublishValidationStatus Status = PublishValidationStatus.Pass);
}
