// ------------------------------------------------------------------------------------------------
// <copyright file="ShortGuidRouteConstraint.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.RouteConstraints
{
    using System;
    using System.Globalization;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Constrains a route parameter to a single <see cref="ShortGuid"/> value - registered under the
    /// name <c>ShortGuid</c> (see <c>Program.cs</c>), so a route segment can be written
    /// <c>{identifier:ShortGuid}</c>.
    /// </summary>
    public class ShortGuidRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The router that this constraint belongs to.</param>
        /// <param name="routeKey">The name of the parameter that is being checked.</param>
        /// <param name="values">A dictionary that contains the parameters for the URL.</param>
        /// <param name="routeDirection">
        /// An object that indicates whether the constraint check is being performed
        /// when an incoming request is being handled or when a URL is being generated.
        /// </param>
        /// <returns><c>true</c> if the URL parameter contains a valid value; otherwise, <c>false</c>.</returns>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            ArgumentNullException.ThrowIfNull(routeKey);
            ArgumentNullException.ThrowIfNull(values);

            if (values.TryGetValue(routeKey, out var value) && value != null)
            {
                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

                try
                {
                    valueString!.FromShortGuid();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
