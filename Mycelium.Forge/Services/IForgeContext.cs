// ------------------------------------------------------------------------------------------------
// <copyright file="IForgeContext.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Provides application-level context for the single Forge registry instance.
    /// </summary>
    public interface IForgeContext
    {
        /// <summary>
        /// Gets the unique identifier of the Forge instance.
        /// </summary>
        Guid ForgeId { get; }

        /// <summary>
        /// Initializes the Forge context by querying the Forge service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the initialization.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}
