// ------------------------------------------------------------------------------------------------
// <copyright file="ImplementsAttribute.cs" company="Starion Group S.A.">
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
    /// Attribute used to decorate properties with, to indicate which class/property is being
    /// implemented.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ImplementsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementsAttribute"/> class.
        /// </summary>
        public ImplementsAttribute(string implementation)
        {
            this.Implementations = implementation;
        }

        /// <summary>
        /// Gets or sets the name of the property that is being implemented, in the form
        /// <c>ClassName.PropertyName</c>.
        /// </summary>
        public string Implementations { get; set; }
    }
}
