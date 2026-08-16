// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtension.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;
    using uml4net.Values;

    /// <summary>
    /// Extension class for the <see cref="IProperty"/> 
    /// </summary>
    public static class PropertyExtension
    {
        /// <summary>
        /// Asserts that the <see cref="IProperty"/> is an enum type with a default value provided
        /// </summary>
        /// <param name="property">The <see cref="IProperty"/> to assert</param>
        /// <returns>True if the <see cref="IProperty"/> have a default value for an enum</returns>
        public static bool QueryIsEnumPropertyWithDefaultValue(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (!property.QueryIsEnum())
            {
                return false;
            }
            
            var defaultValue = property.QueryDefaultValueAsString();

            if (defaultValue == "null")
            {
                return false;
            }
            
            var valueSpecification = property.DefaultValue.FirstOrDefault();

            if (valueSpecification is IInstanceValue instanceValue)
            {
                return instanceValue.Instance is IEnumerationLiteral;
            }
            
            if (valueSpecification is ILiteralString literalString)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="property">The <see cref="IProperty"/></param>
        /// <returns>The <see cref="IProperty.Name"/> with the first letter lowered case in case of derived property, in upper case otherwise</returns>
        public static string QueryPropertyNameBasedOnUmlProperties(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property.IsDerived || property.IsDerivedUnion ? property.Name.LowerCaseFirstLetter() : property.Name.CapitalizeFirstLetter();
        }

    }
}
