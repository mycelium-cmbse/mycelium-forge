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
        /// <returns>A string representing either <c>return Result.Ok();</c> or <c>return Task.FromResult(Result.Ok());</c>.</returns>
        public static string GetOkReturn(bool isAsync)
        {
            return GetReturnStatement("Result.Ok()", isAsync);
        }

        /// <summary>
        /// Gets the C# return statement for a failed result with the given message expression depending on whether the method is
        /// asynchronous.
        /// </summary>
        /// <param name="messageExpression">The message expression or raw string literal.</param>
        /// <param name="isAsync">A value indicating whether the enclosing method is asynchronous.</param>
        /// <returns>
        /// A string representing either <c>return Result.Fail({messageExpression});</c> or
        /// <c>return Task.FromResult(Result.Fail({messageExpression}));</c>.
        /// </returns>
        public static string GetFailReturn(string messageExpression, bool isAsync)
        {
            return GetReturnStatement($"Result.Fail({messageExpression})", isAsync);
        }
    }
}
