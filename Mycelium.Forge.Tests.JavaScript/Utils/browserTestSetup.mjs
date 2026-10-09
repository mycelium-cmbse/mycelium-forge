import { readFileSync } from 'node:fs';
import { mock } from 'node:test';
import vm from 'node:vm';

/**
 * Creates fake browser globals (document, window, localStorage) and installs them on globalThis.
 * The returned object exposes mutable state so tests can change what the fakes report between calls.
 * @param {URL} scriptUrl - Location of the script under test, evaluated by loadScript.
 * @returns {object} The fake browser, including loadScript and dispatchKeydown helpers.
 */
export function setupBrowser(scriptUrl) {
    mock.restoreAll();

    const scriptSource = readFileSync(scriptUrl, 'utf8');
    const classes = new Set();
    const browser = {
        documentListeners: [],
        elements: {},
        searchInput: null,
        storage: new Map(),
        storageFailure: false,
        readyState: 'complete',
        documentElement: {
            style: {},
            classList: {
                toggle(name, force) {
                    force ? classes.add(name) : classes.delete(name);
                },
                contains: (name) => classes.has(name)
            }
        },

        /**
         * Evaluates the script under test again so its top-level side effects run against the current fakes.
         */
        loadScript() {
            vm.runInThisContext(scriptSource, { filename: scriptUrl.href });
        },

        /**
         * Raises a fake keydown event on the document and returns it.
         * @param {object} init - Properties of the event (key, ctrlKey, metaKey...).
         * @returns {object} The event, so tests can inspect defaultPrevented.
         */
        dispatchKeydown(init) {
            const event = {
                defaultPrevented: false,
                preventDefault() {
                    this.defaultPrevented = true;
                },
                ...init
            };
            browser.documentListeners
                .filter(([type]) => type === 'keydown')
                .forEach(([, listener]) => listener(event));
            return event;
        }
    };

    globalThis.document = {
        documentElement: browser.documentElement,
        get readyState() {
            return browser.readyState;
        },
        getElementById: (id) => browser.elements[id] ?? null,
        querySelector: () => browser.searchInput,
        addEventListener: (type, listener) => browser.documentListeners.push([type, listener])
    };

    globalThis.window = {};

    globalThis.localStorage = {
        getItem(key) {
            if (browser.storageFailure) {
                throw new Error('blocked');
            }
            return browser.storage.has(key) ? browser.storage.get(key) : null;
        },
        setItem(key, value) {
            if (browser.storageFailure) {
                throw new Error('blocked');
            }
            browser.storage.set(key, value);
        }
    };

    mock.method(console, 'warn', () => {});

    return browser;
}
