// ------------------------------------------------------------------------------------------------
// <copyright file="ArgumentsExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using HandlebarsDotNet;

    /// <summary>
    /// Unpacks the arguments a template supplied to a Handlebars helper.
    /// </summary>
    public static class ArgumentsExtensions
    {
        /// <param name="arguments">The <see cref="Arguments" /> to unpack.</param>
        extension(Arguments arguments)
        {
            /// <summary>
            /// Queries the single argument supplied to a helper.
            /// </summary>
            /// <typeparam name="T">The type the argument must have.</typeparam>
            /// <param name="helperName">The helper name used in validation messages.</param>
            /// <returns>The supplied argument.</returns>
            /// <exception cref="HandlebarsException">
            /// Thrown when exactly one argument of type <typeparamref name="T" /> was not
            /// supplied.
            /// </exception>
            public T QuerySingle<T>(string helperName)
            {
                if (arguments.Length != 1)
                {
                    throw new HandlebarsException($"{helperName} requires exactly one argument.");
                }

                return arguments[0] is T argument
                    ? argument
                    : throw new HandlebarsException($"{helperName} requires a {typeof(T).Name} argument.");
            }
        }
    }
}
