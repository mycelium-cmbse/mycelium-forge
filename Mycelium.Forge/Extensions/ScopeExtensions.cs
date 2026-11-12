// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides extension methods for <see cref="IScope" /> instances.
    /// </summary>
    public static class ScopeExtensions
    {
        /// <param name="scope">The scope instance.</param>
        extension(IScope scope)
        {
            /// <summary>
            /// Determines whether the scope publisher has verified status.
            /// </summary>
            /// <returns><see langword="true" /> if the scope is verified; otherwise, <see langword="false" />.</returns>
            public bool IsVerified()
            {
                return scope switch
                {
                    IAccount account => account.IsVerified,
                    IOrganization => true,
                    _ => false
                };
            }

            /// <summary>
            /// Formats the display short name of the scope prefixed with '@'.
            /// </summary>
            /// <returns>The formatted scope short name, or an empty string if the scope is null.</returns>
            public string GetDisplayShortName()
            {
                return scope != null && !string.IsNullOrWhiteSpace(scope.ShortName)
                    ? $"@{scope.ShortName}"
                    : string.Empty;
            }
        }
    }
}
