// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaArchive.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Capella
{
    using Auriga.Capellamodeller;

    /// <summary>
    /// Represents the raw extracted contents of a Capella project archive.
    /// </summary>
    /// <param name="Project">The deserialized Capella project descriptor.</param>
    public sealed record CapellaArchive(IProject Project) : IArchive;
}
