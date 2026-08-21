// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationLayout.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Layout
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Code-behind logic for the application layout dedicated to documentation pages.
    /// </summary>
    public partial class DocumentationLayout : LayoutComponentBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the application theme management service.
        /// </summary>
        [Inject]
        public IThemeService ThemeService { get; set; }

        /// <summary>
        /// Subscribes to theme change notifications on initialization.
        /// </summary>
        protected override void OnInitialized()
        {
            this.ThemeService.OnChange += this.StateHasChanged;
        }

        /// <summary>
        /// Unsubscribes from theme change notifications when the layout is disposed.
        /// </summary>
        public void Dispose()
        {
            this.ThemeService.OnChange -= this.StateHasChanged;
            GC.SuppressFinalize(this);
        }
    }
}
