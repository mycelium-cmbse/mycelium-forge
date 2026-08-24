(function () {
    const THEME_STORAGE_KEY = 'forge_theme';
    const DARK_THEME = 'dark';
    const LIGHT_THEME = 'light';

    /**
     * Checks if the user's system preference is set to dark mode using the 'prefers-color-scheme' media query.
     * @returns {boolean} True if the system prefers dark mode, false otherwise.
     */
    function getSystemPrefersDark() {
        return Boolean(window.matchMedia?.('(prefers-color-scheme: dark)')?.matches);
    }

    /**
     * Resolves the user's dark mode preference by checking local storage first, and falling back to the system preference if no stored value is found.
     * @returns {boolean} True if dark mode is active, false otherwise.
     */
    function resolveDarkMode() {
        try {
            const stored = localStorage.getItem(THEME_STORAGE_KEY);
            if (stored === DARK_THEME) {
                return true;
            }
            if (stored === LIGHT_THEME) {
                return false;
            }
        } catch (error) {
            console.warn('Storage access failed while reading theme preference:', error);
        }
        return getSystemPrefersDark();
    }

    /**
     * Applies the dark mode setting to the document root element and updates the color scheme accordingly.
     * @param {any} isDark - True to enable dark mode; false for light mode.
     */
    function applyTheme(isDark) {
        document.documentElement.classList.toggle(DARK_THEME, isDark);
        document.documentElement.style.colorScheme = isDark ? DARK_THEME : LIGHT_THEME;
    }

    window.forgeInterop = {
        /**
         * Copies the given text to the system clipboard using the browser Clipboard API.
         * @param {string} text - The text to copy to the clipboard.
         * @returns {Promise<boolean>} A promise that resolves to true if the copy succeeded, false otherwise.
         */
        copyToClipboard: async function (text) {
            if (!text) {
                return false;
            }

            try {
                await navigator.clipboard.writeText(text);
                return true;
            } catch (error) {
                console.warn('Clipboard writeText failed:', error);
                return false;
            }
        },

        /**
         * Smoothly scrolls the page to the element with the given id.
         * @param {string} id - The id of the target DOM element.
         * @returns {void}
         */
        scrollToElement: function (id) {
            if (!id) {
                return;
            }

            const element = document.getElementById(id);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        },

        /**
         * Retrieves the current dark mode preference from local storage, falling back to the
         * system colour-scheme media query when no stored value is found.
         * @returns {boolean} True if dark mode is active, false otherwise.
         */
        getDarkMode: function () {
            return resolveDarkMode();
        },

        /**
         * Applies the dark mode setting to the document root element and persists the preference
         * to local storage so it survives page reloads.
         * @param {boolean} isDark - True to enable dark mode; false for light mode.
         * @returns {void}
         */
        setDarkMode: function (isDark) {
            const active = Boolean(isDark);
            applyTheme(active);

            try {
                localStorage.setItem(THEME_STORAGE_KEY, active ? DARK_THEME : LIGHT_THEME);
            } catch (error) {
                console.warn('Storage access failed while writing theme preference:', error);
            }
        }
    };

    // Apply theme immediately on initial script evaluation
    applyTheme(resolveDarkMode());
})();

