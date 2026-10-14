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
        },

        /**
         * Streams a large file directly from the file input DOM element to the specified HTTP upload endpoint over HTTP fetch,
         * bypassing the SignalR WebSocket circuit to prevent message size limits and connection degradation.
         * @param {string} inputId - The ID of the file input element.
         * @param {string} targetUrl - The target HTTP upload URL.
         * @param {string} [contentType='application/octet-stream'] - The MIME content type header.
         * @returns {Promise<string>} The upload token returned by the server.
         */
        uploadLargeFile: async function (inputId, targetUrl, contentType = 'application/octet-stream') {
            const inputElement = document.getElementById(inputId);
            if (!inputElement || !inputElement.files || inputElement.files.length === 0) {
                throw new Error('No package file is selected for upload.');
            }

            const file = inputElement.files[0];

            const headers = {
                'Content-Type': contentType,
                'X-File-Name': encodeURIComponent(file.name)
            };

            // In Blazor InteractiveServer, HttpContext is unavailable during WebSocket circuit invocations.
            // The antiforgery token is rendered into the DOM by <AntiforgeryToken /> and read here directly.
            // See: https://learn.microsoft.com/aspnet/core/blazor/security/server/interactive-server-antiforgery
            const antiforgeryInput = document.querySelector('input[name="__RequestVerificationToken"]');

            if (antiforgeryInput && antiforgeryInput.value) {
                headers['RequestVerificationToken'] = antiforgeryInput.value;
            }

            const response = await fetch(targetUrl, {
                method: 'POST',
                body: file,
                headers: headers
            });

            if (!response.ok || response.redirected) {
                const errorText = await response.text();

                throw new Error(response.redirected
                    ? `Upload failed: request was redirected (status ${response.status}).`
                    : (errorText || `Upload failed with status code ${response.status}`));
            }

            const responseContentType = response.headers.get('content-type') || '';

            if (!responseContentType.includes('application/json')) {
                const responseText = await response.text();
                throw new Error(`Upload failed: expected JSON response but received ${responseContentType || 'non-JSON'} content: ${responseText.slice(0, 100)}`);
            }

            const data = await response.json();
            return data.uploadToken;
        }
    };

    /**
     * Handles global keyboard shortcuts, focusing and selecting the search input when Ctrl or Cmd is pressed
     * together with the key configured in the input's data-shortcut-key attribute.
     * @param {KeyboardEvent} event - The keyboard event.
     * @returns {void}
     */
    function handleSearchShortcut(event) {
        if (!(event.ctrlKey || event.metaKey) || typeof event.key !== 'string') {
            return;
        }

        const searchInput = document.querySelector('.search-input-focusable');
        const shortcutKey = searchInput?.dataset.shortcutKey;

        if (event.key.toLowerCase() !== shortcutKey?.toLowerCase()) {
            return;
        }

        event.preventDefault();
        searchInput.focus();

        if (typeof searchInput.select === 'function') {
            searchInput.select();
        }
    }

    // Apply theme immediately on script evaluation to prevent flash of unstyled content.
    applyTheme(resolveDarkMode());

    // Register the Blazor Enhanced Navigation listener so the theme is re-applied after each
    // soft navigation. window.Blazor is only guaranteed to exist after DOMContentLoaded, so we
    // defer registration until then — but guard with readyState in case the script loads late
    // (i.e. DOMContentLoaded has already fired by the time this runs).
    if (typeof document !== 'undefined') {
        document.addEventListener('keydown', handleSearchShortcut);

        const registerEnhancedLoad = function () {
            window.Blazor?.addEventListener('enhancedload', function () {
                applyTheme(resolveDarkMode());
            });
        };

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', registerEnhancedLoad, { once: true });
        } else {
            registerEnhancedLoad();
        }
    }
})();

