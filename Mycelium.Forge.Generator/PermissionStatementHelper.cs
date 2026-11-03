// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionStatementHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator
{
    /// <summary>
    /// Helper methods for generating C# return statements for sync and async permission methods.
    /// </summary>
    public static class PermissionStatementHelper
    {
        /// <summary>
        /// Gets the C# return statement for the given result expression depending on whether the method is asynchronous.
        /// </summary>
        /// <param name="expression">The result expression to return.</param>
        /// <param name="isAsync">A value indicating whether the enclosing method is asynchronous.</param>
        /// <returns>A string representing either <c>return {expression};</c> or <c>return Task.FromResult({expression});</c>.</returns>
        public static string GetReturnStatement(string expression, bool isAsync)
        {
            return isAsync ? $"return {expression};" : $"return Task.FromResult({expression});";
        }

        /// <summary>
        /// Gets the C# return statement for a successful result depending on whether the method is asynchronous.
        /// </summary>
        /// <param name="isAsync">A value indicating whether the enclosing method is asynchronous.</param>
        /// <returns>A string representing either <c>return Result.Success;</c> or <c>return Task.FromResult&lt;ErrorOr&lt;Success&gt;&gt;(Result.Success);</c>.</returns>
        public static string GetOkReturn(bool isAsync)
        {
            return isAsync ? "return Result.Success;" : "return Task.FromResult<ErrorOr<Success>>(Result.Success);";
        }

        /// <summary>
        /// Gets the C# return statement for a failed result with the given message expression depending on whether the method is
        /// asynchronous.
        /// </summary>
        /// <param name="messageExpression">The message expression or raw string literal.</param>
        /// <param name="isAsync">A value indicating whether the enclosing method is asynchronous.</param>
        /// <returns>
        /// A string representing either <c>return Error.Forbidden(description: {messageExpression});</c> or
        /// <c>return Task.FromResult&lt;ErrorOr&lt;Success&gt;&gt;(Error.Forbidden(description: {messageExpression}));</c>.
        /// </returns>
        public static string GetFailReturn(string messageExpression, bool isAsync)
        {
            return isAsync ? $"return Error.Forbidden(description: {messageExpression});" : $"return Task.FromResult<ErrorOr<Success>>(Error.Forbidden(description: {messageExpression}));";
        }

        /// <summary>
        /// Gets the C# return statement for an unauthorized result with the given message expression depending on whether the method is
        /// asynchronous.
        /// </summary>
        /// <param name="messageExpression">The message expression or raw string literal.</param>
        /// <param name="isAsync">A value indicating whether the enclosing method is asynchronous.</param>
        /// <returns>
        /// A string representing either <c>return Error.Unauthorized(description: {messageExpression});</c> or
        /// <c>return Task.FromResult&lt;ErrorOr&lt;Success&gt;&gt;(Error.Unauthorized(description: {messageExpression}));</c>.
        /// </returns>
        public static string GetUnauthorizedReturn(string messageExpression, bool isAsync)
        {
            return isAsync ? $"return Error.Unauthorized(description: {messageExpression});" : $"return Task.FromResult<ErrorOr<Success>>(Error.Unauthorized(description: {messageExpression}));";
        }

        /// <summary>
        /// Gets the C# boolean expression checking if the user context has a specific permission or any of the specified
        /// permissions.
        /// </summary>
        /// <param name="permissions">The collection of permission names.</param>
        /// <returns>A C# boolean expression string, or an empty string if no permissions are provided.</returns>
        public static string FormatHasPermissionCheck(params string[] permissions)
        {
            var list = permissions.Where(permission => !string.IsNullOrWhiteSpace(permission)).Distinct().ToList();

            return list.Count switch
            {
                0 => string.Empty,
                1 => $"PermissionGuard.HasPermission(userContext, PermissionKind.{list[0]})",
                _ => $"PermissionGuard.HasAnyPermission(userContext, {string.Join(", ", list.Select(permission => $"PermissionKind.{permission}"))})"
            };
        }
    }
}
