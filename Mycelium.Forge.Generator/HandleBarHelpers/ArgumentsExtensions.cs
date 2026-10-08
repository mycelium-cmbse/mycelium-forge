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
            /// Thrown when exactly one argument of type <typeparamref name="T" /> was not supplied.
            /// </exception>
            public T QuerySingle<T>(string helperName)
            {
                if (arguments.Length != 1)
                {
                    throw new HandlebarsException($"{helperName} requires exactly one argument.");
                }

                return arguments[0] is T argument
                    ? argument
                    : throw new HandlebarsException($"{helperName} requires {DescribeExpectedType<T>()} argument.");
            }

            /// <summary>
            /// Queries the first of several arguments supplied to a helper.
            /// </summary>
            /// <typeparam name="T">The type the first argument must have.</typeparam>
            /// <param name="helperName">The helper name used in validation messages.</param>
            /// <returns>The first supplied argument.</returns>
            /// <exception cref="HandlebarsException">
            /// Thrown when the first argument is absent or is not of type <typeparamref name="T" />.
            /// </exception>
            public T QueryFirst<T>(string helperName)
            {
                return arguments.Length > 0 && arguments[0] is T argument
                    ? argument
                    : throw new HandlebarsException($"{helperName} requires {DescribeExpectedType<T>()} first argument.");
            }
        }

        /// <summary>
        /// Describes an expected argument type with the article that reads correctly before it.
        /// </summary>
        /// <typeparam name="T">The expected argument type.</typeparam>
        /// <returns>The article and the type name - for example <c>an IClass</c>.</returns>
        private static string DescribeExpectedType<T>()
        {
            var typeName = typeof(T).Name;

            return "AEIOU".Contains(typeName[0]) ? $"an {typeName}" : $"a {typeName}";
        }
    }
}
