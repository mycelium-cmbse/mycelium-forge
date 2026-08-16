// ------------------------------------------------------------------------------------------------
// <copyright file="SubsettedPropertyAttribute.cs" company="Starion Group S.A.">
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
    /// Attribute used to decorate properties when these are subsetted properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class SubsettedPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubsettedPropertyAttribute"/> class.
        /// </summary>
        public SubsettedPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the subsetted property, in the form
        /// <c>ClassName.PropertyName</c>.
        /// </summary>
        public string PropertyName { get; set; }
    }
}
