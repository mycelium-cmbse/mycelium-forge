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
    },
    scrollToElement: function (id) {
        if (!id) {
            return;
        }

        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
};

