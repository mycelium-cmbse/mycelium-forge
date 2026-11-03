// ------------------------------------------------------------------------------------------------
// <copyright file="KparArchive.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Kpar
{
    using SysML2.NET.ModelInterchange;

    /// <summary>
    /// Represents the raw extracted contents of a SysML v2 or KerML project interchange file (<c>.kpar</c>).
    /// </summary>
    /// <param name="Archive">The deserialized SysML2 kpar archive.</param>
    public sealed record KparArchive(Archive Archive) : IArchive;
}
