// ------------------------------------------------------------------------------------------------
// <copyright file="DisposableViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using ReactiveUI;

    /// <summary>
    /// Base view model class that implements the <see cref="IDisposable" /> pattern and manages disposable resources.
    /// </summary>
    public abstract class DisposableViewModel : ReactiveObject, IDisposable
    {
        /// <summary>
        /// Gets the collection of <see cref="IDisposable" /> instances to be disposed when this view model is disposed.
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
        /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
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
