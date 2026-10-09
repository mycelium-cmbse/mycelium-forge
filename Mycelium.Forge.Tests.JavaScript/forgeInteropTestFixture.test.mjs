import assert from 'node:assert/strict';
import { beforeEach, describe, it, mock } from 'node:test';
import { setupBrowser } from './browserTestSetup.mjs';

const SCRIPT_URL = new URL('../Mycelium.Forge/wwwroot/js/forgeInterop.js', import.meta.url);

describe('forgeInterop', () => {
    let browser;

    beforeEach(() => {
        browser = setupBrowser(SCRIPT_URL);
    });

    it('copyToClipboard', async () => {
        browser.loadScript();
        const writeText = mock.fn(async () => {});
        Object.defineProperty(globalThis, 'navigator', { value: { clipboard: { writeText } }, configurable: true });

        assert.equal(await window.forgeInterop.copyToClipboard(''), false);
        assert.equal(writeText.mock.callCount(), 0);

        assert.equal(await window.forgeInterop.copyToClipboard('text'), true);
        assert.deepEqual(writeText.mock.calls[0].arguments, ['text']);

        writeText.mock.mockImplementation(async () => {
            throw new Error('denied');
        });
        assert.equal(await window.forgeInterop.copyToClipboard('text'), false);
        assert.equal(console.warn.mock.callCount(), 1);
    });

    it('scrollToElement', () => {
        browser.loadScript();
        const scrollIntoView = mock.fn();
        browser.elements.target = { scrollIntoView };

        window.forgeInterop.scrollToElement('');
        window.forgeInterop.scrollToElement('missing');
        assert.equal(scrollIntoView.mock.callCount(), 0);

        window.forgeInterop.scrollToElement('target');
        assert.deepEqual(scrollIntoView.mock.calls[0].arguments, [{ behavior: 'smooth', block: 'start' }]);
    });

    it('getDarkMode', () => {
        browser.loadScript();

        assert.equal(window.forgeInterop.getDarkMode(), false);

        window.matchMedia = () => ({ matches: true });
        assert.equal(window.forgeInterop.getDarkMode(), true);

        browser.storage.set('forge_theme', 'light');
        assert.equal(window.forgeInterop.getDarkMode(), false);

        browser.storage.set('forge_theme', 'dark');
        window.matchMedia = () => ({ matches: false });
        assert.equal(window.forgeInterop.getDarkMode(), true);

        browser.storage.delete('forge_theme');
        browser.storageFailure = true;
        window.matchMedia = () => ({ matches: true });
        assert.equal(window.forgeInterop.getDarkMode(), true);
        assert.equal(console.warn.mock.callCount(), 1);
    });

    it('setDarkMode', () => {
        browser.loadScript();

        window.forgeInterop.setDarkMode(true);
        assert.equal(browser.documentElement.classList.contains('dark'), true);
        assert.equal(browser.documentElement.style.colorScheme, 'dark');
        assert.equal(browser.storage.get('forge_theme'), 'dark');

        window.forgeInterop.setDarkMode(false);
        assert.equal(browser.documentElement.classList.contains('dark'), false);
        assert.equal(browser.documentElement.style.colorScheme, 'light');
        assert.equal(browser.storage.get('forge_theme'), 'light');

        browser.storageFailure = true;
        window.forgeInterop.setDarkMode(true);
        assert.equal(browser.documentElement.classList.contains('dark'), true);
        assert.equal(console.warn.mock.callCount(), 1);
    });

    it('handleSearchShortcut', () => {
        browser.loadScript();
        browser.searchInput = {
            dataset: { shortcutKey: 'k' },
            focus: mock.fn(),
            select: mock.fn()
        };

        assert.equal(browser.dispatchKeydown({ key: 'k' }).defaultPrevented, false);
        assert.equal(browser.dispatchKeydown({ key: 'x', ctrlKey: true }).defaultPrevented, false);
        assert.equal(browser.dispatchKeydown({ key: undefined, ctrlKey: true }).defaultPrevented, false);
        assert.equal(browser.searchInput.focus.mock.callCount(), 0);

        assert.equal(browser.dispatchKeydown({ key: 'K', ctrlKey: true }).defaultPrevented, true);
        assert.equal(browser.dispatchKeydown({ key: 'k', metaKey: true }).defaultPrevented, true);
        assert.equal(browser.searchInput.focus.mock.callCount(), 2);
        assert.equal(browser.searchInput.select.mock.callCount(), 2);

        browser.searchInput.select = undefined;
        assert.equal(browser.dispatchKeydown({ key: 'k', ctrlKey: true }).defaultPrevented, true);

        browser.searchInput = null;
        assert.equal(browser.dispatchKeydown({ key: 'k', ctrlKey: true }).defaultPrevented, false);
    });

    it('initialization', () => {
        browser.storage.set('forge_theme', 'dark');
        const enhancedLoadListeners = [];
        window.Blazor = { addEventListener: mock.fn((type, listener) => enhancedLoadListeners.push([type, listener])) };

        browser.loadScript();
        assert.equal(browser.documentElement.classList.contains('dark'), true);
        assert.deepEqual(window.Blazor.addEventListener.mock.calls[0].arguments[0], 'enhancedload');

        browser.storage.set('forge_theme', 'light');
        enhancedLoadListeners[0][1]();
        assert.equal(browser.documentElement.classList.contains('dark'), false);

        browser.documentListeners.length = 0;
        window.Blazor.addEventListener.mock.resetCalls();
        browser.readyState = 'loading';
        browser.loadScript();
        assert.equal(window.Blazor.addEventListener.mock.callCount(), 0);

        browser.documentListeners.filter(([type]) => type === 'DOMContentLoaded').forEach(([, listener]) => listener());
        assert.equal(window.Blazor.addEventListener.mock.callCount(), 1);

        delete window.Blazor;
        browser.readyState = 'complete';
        browser.loadScript();
        assert.equal(typeof window.forgeInterop, 'object');
    });
});
