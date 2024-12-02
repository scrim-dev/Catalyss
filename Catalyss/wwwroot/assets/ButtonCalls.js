function InjectDll() {
    window.external.sendMessage('InjectMonoDll');
}

function UnloadDll() {
    window.external.sendMessage('EjectMonoDll');
}