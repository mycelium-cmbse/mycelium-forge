// ------------------------------------------------------------------------------------------------
// <copyright file="NotificationComponent.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using BlazorBlueprint.Components;

    using DynamicData;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Model;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Component responsible for listening to reactive notifications from <see cref="INotificationService" /> and rendering
    /// toast notifications.
    /// </summary>
    public partial class NotificationComponent : DisposableComponent
    {
        /// <summary>
        /// Gets or sets the injected <see cref="INotificationService" />.
        /// </summary>
        [Inject]
        public INotificationService NotificationService { get; set; }

        /// <summary>
        /// Gets or sets the injected BlazorBlueprint <see cref="ToastService" />.
        /// </summary>
        [Inject]
        public ToastService ToastService { get; set; }

        /// <summary>
        /// Initializes the component lifecycle and subscribes to incoming notification records.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var subscription = this.NotificationService.Results
                .Connect()
                .WhereReasonsAre(ListChangeReason.Add, ListChangeReason.AddRange)
                .Subscribe(_ =>
                {
                    foreach (var notification in this.NotificationService.Results.Items)
                    {
                        this.DisplayToastNotification(notification);
                    }

                    this.NotificationService.Results.Clear();
                });

            this.Disposables.Add(subscription);
        }

        /// <summary>
        /// Displays a toast notification on the screen from a given notification record.
        /// </summary>
        /// <param name="notification">The <see cref="NotificationContent" /> to display.</param>
        private void DisplayToastNotification(NotificationContent notification)
        {
            if (notification == null || string.IsNullOrWhiteSpace(notification.Message))
            {
                return;
            }

            var variant = notification.Type.ToToastVariant();
            this.ToastService.Show(notification.Message, notification.Title, variant);
        }
    }
}
