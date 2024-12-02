using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SharpMonoInjector;

public static class Native
{
    [DllImport("advapi32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern bool OpenProcessToken(SafeProcessHandle ProcessHandle, int DesiredAccess, out SafeAccessTokenHandle TokenHandle);

    [DllImport("psapi", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern bool EnumProcessModulesEx(SafeProcessHandle hProcess, nint lphModule, int cb, out int lpcbNeeded, int dwFilterFlag = 0x03);

    [DllImport("psapi")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern int GetModuleFileNameExA(SafeProcessHandle hProcess, nint hModule, nint lpBaseName, int nSize);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern nint CreateRemoteThread(SafeProcessHandle hProcess, nint lpThreadAttributes, int dwStackSize, nint lpStartAddress, nint lpParameter, int dwCreationFlags, out int lpThreadId);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern uint WaitForSingleObject(nint hHandle, int dwMilliseconds);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern bool IsWow64Process(SafeProcessHandle hProcess, out bool wow64Process);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern bool ReadProcessMemory(SafeProcessHandle hProcess, nint lpBaseAddress, nint lpBuffer, int nSize, int lpNumberOfBytesWritten = 0);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern bool WriteProcessMemory(SafeProcessHandle hProcess, nint lpBaseAddress, nint lpBuffer, int nSize, int lpNumberOfBytesRead = 0);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern nint VirtualAllocEx(SafeProcessHandle hProcess, nint lpAddress, int dwSize, int flAllocationType, int flProtect);

    [DllImport("kernel32", SetLastError = true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static extern bool VirtualFreeEx(SafeProcessHandle hProcess, nint lpAddress, int dwSize, int dwFreeType);
}