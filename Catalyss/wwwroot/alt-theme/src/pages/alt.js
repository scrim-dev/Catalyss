function GhBtn() {
    window.external.sendMessage('LoadGhLink');
}

function DiscBtn() {
    window.external.sendMessage('LoadDiscLink');
}

function ReloadGame() {
    window.external.sendMessage('Reload_Game');
}

function ResetGui() {
    window.external.sendMessage('Revert_GUI');
}

function InjectDll() {
    window.external.sendMessage('InjectMonoDll');
}

function UnloadDll() {
    window.external.sendMessage('EjectMonoDll');
}

function StartAtlyss() {
    window.external.sendMessage('StartGame');
}

function QuitAtlyss() {
    window.external.sendMessage('QuitGame');
}

function UseAltTheme() {
    window.location = "alt-theme/index.html";
    window.external.sendMessage('UseAlt');
}