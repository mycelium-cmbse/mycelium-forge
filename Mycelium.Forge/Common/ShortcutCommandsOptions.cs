// ------------------------------------------------------------------------------------------------
// <copyright file="ShortcutCommandsOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Maps a shortcut command name to the key that triggers it (combined with Ctrl or Cmd),
    /// bound from the <see cref="SectionName" /> section of the application settings.
    /// </summary>
    public class ShortcutCommandsOptions : Dictionary<string, string>
    {
        /// <summary>
        /// The name of the configuration section holding the shortcut commands.
        /// </summary>
        public const string SectionName = "ShortcutCommands";

        /// <summary>
        /// The command name of the search input focus shortcut.
        /// </summary>
        public const string Search = "search";

        /// <summary>
        /// The key used for the search command when none is configured.
        /// </summary>
        public const string DefaultSearchKey = "k";

        /// <summary>
        /// Gets the key configured for the search command.
        /// </summary>
        /// <returns>
        /// The configured key in upper case, or the default key when none is configured.
        /// </returns>
        public string GetSearchKey()
        {
            var key = this.GetValueOrDefault(Search);

            return (string.IsNullOrWhiteSpace(key) ? DefaultSearchKey : key.Trim()).ToUpperInvariant();
        }
    }
}
