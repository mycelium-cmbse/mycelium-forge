// ------------------------------------------------------------------------------------------------
// <copyright file="ICloneable.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Defines a contract for creating a strongly typed copy of an instance.
    /// </summary>
    /// <typeparam name="T">The type of the cloned instance.</typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>A new <typeparamref name="T" /> instance with the same values as the current instance.</returns>
        T Clone();
    }
}
