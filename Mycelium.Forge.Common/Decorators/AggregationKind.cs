// ------------------------------------------------------------------------------------------------
// <copyright file="AggregationKind.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Decorators
{
    /// <summary>
    /// AggregationKind is an Enumeration for specifying the kind of aggregation of a Property.
    /// </summary>
    /// <remarks>
    /// Mirrors <c>uml4net.Classification.AggregationKind</c> value-for-value. Declared locally
    /// (rather than referencing uml4net's own type) so that <c>Mycelium.Forge.Common</c> - the
    /// runtime DTO library - never needs uml4net as a dependency; only the design-time generator
    /// does.
    /// </remarks>
    public enum AggregationKind
    {
        /// <summary>
        /// Indicates that the Property has no aggregation.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the Property has shared aggregation.
        /// </summary>
        Shared,

        /// <summary>
        /// Indicates that the Property is aggregated compositely, i.e., the composite object has
        /// responsibility for the existence and storage of the composed objects (parts).
        /// </summary>
        Composite,
    }
}
