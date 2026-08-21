// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Publish
{
    /// <summary>
    /// Represents a discrete step in the package publishing wizard workflow.
    /// </summary>
    public class PublishStepModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishStepModel" /> class.
        /// </summary>
        public PublishStepModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishStepModel" /> class with specified properties.
        /// </summary>
        /// <param name="number">The sequential 1-based index number of the step.</param>
        /// <param name="title">The human-readable label of the step.</param>
        /// <param name="isCurrent">A value indicating whether this step is currently active.</param>
        /// <param name="isCompleted">A value indicating whether this step has been completed.</param>
        public PublishStepModel(
            int number,
            string title,
            bool isCurrent = false,
            bool isCompleted = false)
        {
            this.Number = number;
            this.Title = title;
            this.IsCurrent = isCurrent;
            this.IsCompleted = isCompleted;
        }

        /// <summary>
        /// Gets or sets the sequential 1-based index number of the step.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the human-readable label of the step.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this step is currently active.
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step has been completed.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
