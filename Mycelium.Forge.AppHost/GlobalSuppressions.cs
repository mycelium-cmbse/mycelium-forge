// ------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "S5332:Using clear-text HTTP protocol is insecure. Use https instead.", Scope = "module", Justification = "Internal Docker network communication between services and local development container URLs use clear-text HTTP.")]
[assembly: SuppressMessage("Maintainability", "S1075:URIs should not be hardcoded", Scope = "module", Justification = "Local development endpoints in AppHost.")]
