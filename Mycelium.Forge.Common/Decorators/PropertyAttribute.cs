// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyAttribute.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Decorators
{
    using System;

    /// <summary>
    /// Attribute used to decorate properties using the properties sourced from the UML metamodel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class PropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute" /> class.
        /// </summary>
        /// <param name="xmiId">The unique identifier.</param>
        /// <param name="aggregation">The <see cref="AggregationKind" />.</param>
        /// <param name="lowerValue">The lower value (lower bound) of the property.</param>
        /// <param name="upperValue">The upper value (upper bound) of the property.</param>
        /// <param name="isOrdered">A value indicating whether this is an ordered property.</param>
        /// <param name="isReadOnly">A value indicating whether this is a readonly property.</param>
        /// <param name="isDerived">A value indicating whether this is a derived property.</param>
        /// <param name="isDerivedUnion">A value indicating whether this is a derived union property.</param>
        /// <param name="isUnique">A value indicating whether the values are unique.</param>
        /// <param name="isOwnerEnd">A value indicating whether this property is the owner end of an association.</param>
        /// <param name="defaultValue">The default value if any.</param>
        public PropertyAttribute(
            string xmiId = "",
            AggregationKind aggregation = AggregationKind.None,
            int lowerValue = 1,
            int upperValue = 1,
            bool isOrdered = false,
            bool isReadOnly = false,
            bool isDerived = false,
            bool isDerivedUnion = false,
            bool isUnique = true,
            bool isOwnerEnd = false,
            string defaultValue = null)
        {
            this.XmiId = xmiId;
            this.Aggregation = aggregation;
            this.LowerValue = lowerValue;
            this.UpperValue = upperValue;
            this.IsOrdered = isOrdered;
            this.IsReadOnly = isReadOnly;
            this.IsDerived = isDerived;
            this.IsDerivedUnion = isDerivedUnion;
            this.IsUnique = isUnique;
            this.IsOwnerEnd = isOwnerEnd;
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public string XmiId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AggregationKind" />.
        /// </summary>
        public AggregationKind Aggregation { get; set; }

        /// <summary>
        /// Gets or sets the lower value (lower bound) of the property
        /// </summary>
        public int LowerValue { get; set; }

        /// <summary>
        /// Gets or sets the upper value (upper bound) of the property
        /// </summary>
        public int UpperValue { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether this is an ordered property
        /// </summary>
        public bool IsOrdered { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether this is a readonly property
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether this is a derived property
        /// </summary>
        public bool IsDerived { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether this is a derived union property
        /// </summary>
        public bool IsDerivedUnion { get; set; }

        /// <summary>
        /// For a multivalued multiplicity, this attribute specifies whether the values in an
        /// instantiation of this MultiplicityElement are unique.
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is the owner end of an association.
        /// </summary>
        public bool IsOwnerEnd { get; set; }

        /// <summary>
        /// Gets or sets the default value if any.
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
