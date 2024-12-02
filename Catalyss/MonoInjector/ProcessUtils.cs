using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SharpMonoInjector;

public static class ProcessUtils
{
    public unsafe static bool GetMonoModule(Process process, out nint monoModule)
    {
        if (!Native.EnumProcessModulesEx(process.SafeHandle, 0, 0, out var bytesNeeded))
            throw new InjectorException("Failed to get process module count", new Win32Exception());

        var count = bytesNeeded / (Is64BitProcess(process) ? 8 : 4);
        var hModules = stackalloc nint[count];

        if (!Native.EnumProcessModulesEx(process.SafeHandle, (nint)hModules, bytesNeeded, out _))
            throw new InjectorException("Failed to enumerate process modules", new Win32Exception());

        const int MAX_PATH = 260;
        var path = stackalloc sbyte[MAX_PATH];

        for (var i = 0; i < count; ++i) try
        {
            var hMod = hModules[i];
            var length = Native.GetModuleFileNameExA(process.SafeHandle, hMod, (nint)path, MAX_PATH);
            if (length == 0) throw new InjectorException("Failed to get module info", new Win32Exception());

            if (new string(path, 0, length).Contains("MonoBleedingEdge", StringComparison.Ordinal))
            {
                monoModule = hMod;
                return true;
            }
        }
        catch (Exception e)
        {
            Trace.WriteLine("[ProcessUtils] GetMono - ERROR: " + e.Message);
        }

        monoModule = 0;
        return false;
    }
    internal static bool Is64BitProcess(Process proc)
    {
        try
        {
            if (!Environment.Is64BitOperatingSystem) return false;

            if (!Native.IsWow64Process(proc.SafeHandle, out bool isWOW64)) throw new Win32Exception();
            return !isWOW64;
        }
        catch (Exception e) 
        { 
            Trace.WriteLine("[ProcessUtils] x64Bit - ERROR: " + e.Message); 
        }
        return true;
    }
    public static bool AntivirusInstalled()
    {
        try
        {
            List<string> avs = [];
            using ManagementObjectSearcher searcher = new(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
            using var instances = searcher.Get();

            if (instances.Count > 0)
            {
                Trace.WriteLine("Antivirus Installed: True");
                StringBuilder installedAVs = new("Installed Antivirus:\n");

                foreach (ManagementBaseObject av in instances) using (av)
                {
                    var avInstalled = ((string)av.GetPropertyValue("pathToSignedProductExe")).Replace("//", "") + " " + (string)av.GetPropertyValue("pathToSignedReportingExe");
                    avs.Add(avInstalled.ToLower());
                    lock (installedAVs) installedAVs.AppendLine("   " + avInstalled);
                }
                Trace.WriteLine(installedAVs.ToString());
            }
            else Trace.WriteLine("Antivirus Installed: False");

            Parallel.ForEach(Process.GetProcesses(), p =>
            {
                using (p) Parallel.ForEach(avs, detect =>
                {
                    if (detect.EndsWith(p.ProcessName + ".exe", StringComparison.OrdinalIgnoreCase)) Trace.WriteLine("Antivirus Running: " + detect);
                });
            });
            return instances.Count > 0;
        }
        catch (Exception e)
        {
            Trace.WriteLine("Error checking for Antivirus: " + e.ToString());
        }
        return false;
    }
}