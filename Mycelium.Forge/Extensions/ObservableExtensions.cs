// ------------------------------------------------------------------------------------------------
// <copyright file="ObservableExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using System.Reactive.Linq;

    /// <summary>
    /// Extension class for the <see cref="IObservable{T}" />.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Subscribes to an <see cref="IObservable{T}" /> with async capabilities.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The source <see cref="IObservable{T}" />.</param>
        /// <param name="onNextAsync">The asynchronous handler invoked for each element.</param>
        /// <returns>The created <see cref="IDisposable" />.</returns>
        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync)
        {
            return source.Select(x => Observable.FromAsync(() => onNextAsync(x))).Concat().Subscribe();
        }
    }
}
