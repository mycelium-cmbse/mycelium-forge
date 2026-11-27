// ------------------------------------------------------------------------------------------------
// <copyright file="ComponentExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Provides helper extension methods for querying rendered Blazor components in tests.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Checks whether a space-delimited class string contains the specified token.
        /// </summary>
        /// <param name="classValue">The space-delimited CSS class string.</param>
        /// <param name="cssClass">The CSS class token to locate.</param>
        /// <returns><c>true</c> if the token is present; otherwise, <c>false</c>.</returns>
        private static bool ContainsClassToken(string classValue, string cssClass)
        {
            return !string.IsNullOrWhiteSpace(classValue) && classValue.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(cssClass, StringComparer.Ordinal);
        }

        /// <summary>
        /// Resolves the <c>AdditionalAttributes</c> dictionary for supported components without reflection.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <returns>The additional attributes dictionary if supported; otherwise, an empty dictionary.</returns>
        private static IReadOnlyDictionary<string, object> GetAdditionalAttributes(IComponent component)
        {
            return component switch
            {
                BbButton { AdditionalAttributes: not null } button => button.AdditionalAttributes,
                BbBadge { AdditionalAttributes: not null } badge => badge.AdditionalAttributes,
                BbDropdownMenuItem { AdditionalAttributes: not null } menuItem => menuItem.AdditionalAttributes,
                BbMultiSelect<Guid> { AdditionalAttributes: not null } multiSelect => multiSelect.AdditionalAttributes,
                _ => ImmutableDictionary<string, object>.Empty
            };
        }

        /// <summary>
        /// Resolves the <c>Class</c> property for supported components without reflection.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <returns>The class string if supported; otherwise, <see cref="string.Empty" />.</returns>
        private static string GetClassString(IComponent component)
        {
            return component switch
            {
                BbButton button => button.Class ?? string.Empty,
                BbBadge badge => badge.Class ?? string.Empty,
                BbDropdownMenuItem menuItem => menuItem.Class ?? string.Empty,
                BbInput input => input.Class ?? string.Empty,
                _ => string.Empty
            };
        }

        /// <param name="component">The component instance to inspect.</param>
        extension(IComponent component)
        {
            /// <summary>
            /// Determines whether the component has an <c>id</c> attribute matching <paramref name="id" />.
            /// </summary>
            /// <param name="id">The expected element identifier.</param>
            /// <returns><c>true</c> if the component defines the matching identifier; otherwise, <c>false</c>.</returns>
            public bool HasId(string id)
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }

                if (component is BbInput input)
                {
                    return string.Equals(input.Id, id, StringComparison.Ordinal);
                }

                var attributes = GetAdditionalAttributes(component);

                return attributes.TryGetValue("id", out var value) && string.Equals(value.ToString(), id, StringComparison.Ordinal);
            }

            /// <summary>
            /// Determines whether the component contains the CSS class <paramref name="cssClass" />.
            /// </summary>
            /// <param name="cssClass">The expected CSS class token.</param>
            /// <returns><c>true</c> if the component includes the CSS class; otherwise, <c>false</c>.</returns>
            public bool HasClass(string cssClass)
            {
                if (string.IsNullOrWhiteSpace(cssClass))
                {
                    return false;
                }

                var classValue = GetClassString(component);

                if (!string.IsNullOrWhiteSpace(classValue) && ContainsClassToken(classValue, cssClass))
                {
                    return true;
                }

                var attributes = GetAdditionalAttributes(component);

                return attributes.TryGetValue("class", out var attributeClass) && ContainsClassToken(attributeClass.ToString() ?? string.Empty, cssClass);
            }
        }
    }
}
