// Rich-text authoring interop for the LTS content editor (Sprint 8, F4.1).
window.ltsAuthoring = {
    setContent: function (element, html) {
        element.innerHTML = html || '';
    },
    getContent: function (element) {
        return element.innerHTML;
    },
    execCommand: function (command) {
        document.execCommand(command, false, null);
    }
};
