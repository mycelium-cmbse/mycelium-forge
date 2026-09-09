// ------------------------------------------------------------------------------------------------
// <copyright file="BehaviorConfigurationBase.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Models
{
    using System;
    using System.Collections.Generic;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Base class for resolved entity behavior configurations.
    /// </summary>
    public abstract class BehaviorConfigurationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorConfigurationBase" /> class.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        protected BehaviorConfigurationBase(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            ArgumentNullException.ThrowIfNull(definition);
            ArgumentNullException.ThrowIfNull(behavior);

            this.EntityName = behavior.EntityName;
        }

        /// <summary>
        /// Gets the target entity name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Splits a comma-delimited configuration string into trimmed entries.
        /// </summary>
        /// <param name="value">The raw comma-delimited string.</param>
        /// <returns>An array of trimmed string values.</returns>
        protected static string[] SplitValues(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? []
                : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        /// <summary>
        /// Formats a service field name from an entity name (e.g. "Organization" to "organizationService").
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        /// <returns>The formatted field name.</returns>
        protected static string FormatServiceField(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                return string.Empty;
            }

            return $"{char.ToLowerInvariant(entityName[0])}{entityName[1..]}Service";
        }

        /// <summary>
        /// Formats a variable name from an entity name (e.g. "Organization" to "organization").
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        /// <returns>The formatted variable name.</returns>
        protected static string FormatVariableName(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                return string.Empty;
            }

            return $"{char.ToLowerInvariant(entityName[0])}{entityName[1..]}";
        }

        /// <summary>
        /// Gets a configuration value from the configuration dictionary, falling back to a default value, and optionally throwing if required and missing.
        /// </summary>
        /// <param name="config">The configuration dictionary.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="fallback">The fallback or default value.</param>
        /// <param name="isOptional">A value indicating whether the configuration value is optional.</param>
        /// <param name="errorMessage">The error message to use when throwing <see cref="InvalidOperationException" />.</param>
        /// <returns>The resolved configuration value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the value is required and both the key and fallback are missing or empty.</exception>
        protected static string GetValue(Dictionary<string, string> config, string key, string fallback = "", bool isOptional = false, string errorMessage = "")
        {
            var value = config.TryGetValue(key, out var val) && !string.IsNullOrWhiteSpace(val) ? val : fallback;

            if (isOptional || !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var message = !string.IsNullOrWhiteSpace(errorMessage)
                ? errorMessage
                : $"Configuration key '{key}' is required.";

            throw new InvalidOperationException(message);
        }
    }
}
