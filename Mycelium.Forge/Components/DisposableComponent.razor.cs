// ------------------------------------------------------------------------------------------------
// <copyright file="DisposableComponent.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Base component that implements <see cref="IDisposable" /> and manages a collection of disposable resources.
    /// Inherit from this class to simplify cleanup of event subscriptions and other disposable resources.
    /// </summary>
    public partial class DisposableComponent : ComponentBase, IDisposable
    {
        /// <summary>
        /// Gets the collection of <see cref="IDisposable" /> instances to be disposed when this component is disposed.
        /// </summary>
        protected List<IDisposable> Disposables { get; } = [];

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            foreach (var disposable in this.Disposables)
            {
                disposable.Dispose();
            }

            this.Disposables.Clear();
        }
    }
}
