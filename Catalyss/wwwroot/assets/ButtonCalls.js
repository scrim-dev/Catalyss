﻿function InjectDll() {
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