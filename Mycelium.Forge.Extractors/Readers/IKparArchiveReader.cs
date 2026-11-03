// ------------------------------------------------------------------------------------------------
// <copyright file="IKparArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    /// <summary>
    /// Reads and deserializes a SysML v2 or KerML project interchange package (<c>.kpar</c>) from a stream.
    /// </summary>
    public interface IKparArchiveReader : IArchiveReader;
}
