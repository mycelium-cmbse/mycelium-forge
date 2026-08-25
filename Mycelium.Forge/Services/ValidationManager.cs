// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationManager.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Manages field validation states and error collections for UI components and dialogs.
    /// </summary>
    public class ValidationManager
    {
        /// <summary>
        /// Stores the dictionary containing validation errors keyed by property name.
        /// </summary>
        private readonly Dictionary<string, List<string>> errors = new();

        /// <summary>
        /// Gets the collection of validation errors for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to query.</param>
        /// <returns>An enumerable of error messages, or an empty collection if none exist.</returns>
        public IEnumerable<string> this[string propertyName] =>
            this.errors.TryGetValue(propertyName, out var list) ? list : [];

        /// <summary>
        /// Gets a value indicating whether all managed fields are valid with zero errors.
        /// </summary>
        public bool IsValid => this.errors.Count == 0;

        /// <summary>
        /// Determines whether a validation error exists for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <returns><c>true</c> if an error exists for the property; otherwise, <c>false</c>.</returns>
        public bool HasError(string propertyName)
        {
            return this.errors.TryGetValue(propertyName, out var list) && list.Count > 0;
        }

        /// <summary>
        /// Evaluates a validation condition for a property and records or clears the error message.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="isValid">A value indicating whether the property is considered valid.</param>
        /// <param name="errorMessage">The error message to display when invalid.</param>
        /// <returns>The current <see cref="ValidationManager" /> instance for fluent chaining.</returns>
        public ValidationManager Check(string propertyName, bool isValid, string errorMessage)
        {
            if (!isValid)
            {
                this.errors[propertyName] = [errorMessage];
            }
            else
            {
                this.errors.Remove(propertyName);
            }

            return this;
        }

        /// <summary>
        /// Evaluates a predicate function for a property and records or clears the error message.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="predicate">A function returning whether the property is valid.</param>
        /// <param name="errorMessage">The error message to display when invalid.</param>
        /// <returns>The current <see cref="ValidationManager" /> instance for fluent chaining.</returns>
        public ValidationManager Check(string propertyName, Func<bool> predicate, string errorMessage)
        {
            var isValid = predicate != null && predicate();
            return this.Check(propertyName, isValid, errorMessage);
        }

        /// <summary>
        /// Clears validation errors for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to clear.</param>
        public void ClearError(string propertyName)
        {
            this.errors.Remove(propertyName);
        }
    }
}
