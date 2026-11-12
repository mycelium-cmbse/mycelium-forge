// ------------------------------------------------------------------------------------------------
// <copyright file="ITransferPackageOwnershipDialogViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageSettings
{
    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for the transfer package ownership dialog.
    /// </summary>
    public interface ITransferPackageOwnershipDialogViewModel
    {
        /// <summary>
        /// Validates the requested transfer target for inline display and resolves it to a scope.
        /// </summary>
        /// <param name="package">The package being transferred.</param>
        /// <param name="currentOwnerShortName">The short name of the current owning scope.</param>
        /// <param name="targetShortName">The short name of the requested target scope.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The resolved target <see cref="IScope" />, or validation errors describing why it cannot be used.</returns>
        Task<ErrorOr<IScope>> ValidateTargetAsync(IPackage package, string currentOwnerShortName, string targetShortName, CancellationToken token = default);
    }
}
