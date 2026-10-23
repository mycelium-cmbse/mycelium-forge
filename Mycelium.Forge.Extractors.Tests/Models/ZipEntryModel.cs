// ------------------------------------------------------------------------------------------------
// <copyright file="ZipEntryModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Models
{
    /// <summary>
    /// Represents an entry name and content used to construct in-memory test archives.
    /// </summary>
    /// <param name="Name">The archive-relative entry name.</param>
    /// <param name="Content">The entry text content.</param>
    public record ZipEntryModel(string Name, string Content);
}
