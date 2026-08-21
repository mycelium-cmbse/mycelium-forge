window.forgeInterop = {
    copyToClipboard: async function (text) {
        if (!text) {
            return false;
        }

        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (error) {
            return false;
        }
    }
};
