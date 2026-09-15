// ------------------------------------------------------------------------------------------------
// <copyright file="ModelVersionHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System;

    using HandlebarsDotNet;

    /// <summary>
    /// Provides Handlebars helpers for writing the model version.
    /// </summary>
    public static class ModelVersionHelper
    {
        /// <summary>
        /// Registers the model version Handlebars helpers.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> context.</param>
        /// <param name="fallbackVersion">The fallback model version to write when none is resolved.</param>
        public static void RegisterModelVersionHelper(this IHandlebars handlebars, string fallbackVersion = "0.1.0")
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("Forge.ModelVersion", (writer, context, arguments) => WriteModelVersion(writer, context, arguments, fallbackVersion));
        }

        /// <summary>
        /// Writes the model version to the specified <see cref="EncodedTextWriter" />.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" /> to write to.</param>
        /// <param name="context">The Handlebars execution <see cref="Context" />.</param>
        /// <param name="arguments">The Handlebars helper <see cref="Arguments" />.</param>
        /// <param name="fallbackVersion">The fallback model version to write when none is resolved.</param>
        public static void WriteModelVersion(EncodedTextWriter writer, Context context, Arguments arguments, string fallbackVersion = "0.1.0")
        {
            if (arguments.Length > 0 && arguments[0] is string argVersion)
            {
                writer.WriteSafeString(argVersion);
                return;
            }

            writer.WriteSafeString(fallbackVersion);
        }
    }
}
