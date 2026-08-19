// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a discrete step in the package publishing wizard workflow.
    /// </summary>
    /// <param name="Number">The sequential 1-based index number of the step.</param>
    /// <param name="Title">The human-readable label of the step.</param>
    /// <param name="IsCurrent">A value indicating whether this step is currently active.</param>
    /// <param name="IsCompleted">A value indicating whether this step has been completed.</param>
    public record PublishStepModel(
        int Number,
        string Title,
        bool IsCurrent = false,
        bool IsCompleted = false);
}
