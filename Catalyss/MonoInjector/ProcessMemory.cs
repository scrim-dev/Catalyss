using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpMonoInjector;

public sealed class ProcessMemory(Process process) : IDisposable
{
    readonly List<(nint, int)> allocs = [];

    public string ReadString(in nint addr, int length, Encoding encoding)
    {
        Span<byte> bytes = stackalloc byte[length];
        for (var i = 0; i < length; ++i)
        {
            var read = Read<byte>(addr + i);
            if (read == 0)
            {
                length = i;
                break;
            }
            bytes[i] = read;
        }
        return encoding.GetString(bytes[..length]);
    }
    public unsafe T Read<T>(in nint addr) where T : unmanaged
    {
        T ret;
        if (!Native.ReadProcessMemory(process.SafeHandle, addr, (nint)(&ret), sizeof(T))) throw new InjectorException("Failed to read process memory", new Win32Exception());
        return ret;
    }

    public nint AllocateAndWrite(ReadOnlySpan<byte> data)
    {
        var addr = Allocate(data.Length);
        Write(addr, data);
        return addr;
    }
    public nint AllocateAndWrite(ReadOnlySpan<char> data)
    {
        var chars = ArrayPool<char>.Shared.Rent(data.Length);
        data.CopyTo(chars);

        var buffer = ArrayPool<byte>.Shared.Rent(Encoding.ASCII.GetByteCount(chars, 0, data.Length));
        try
        {
            var length = Encoding.ASCII.GetBytes(chars, 0, data.Length, buffer, 0);
            return AllocateAndWrite(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(buffer), length));
        }
        finally
        {
            ArrayPool<char>.Shared.Return(chars);
            ArrayPool<byte>.Shared.Return(buffer); 
        }
    }
    public nint Allocate(in int size)
    {
        var addr = Native.VirtualAllocEx(process.SafeHandle, 0, size, 0x00001000, 0x40);
        if (addr == 0) throw new InjectorException("Failed to allocate process memory", new Win32Exception());

        allocs.Add((addr, size));
        return addr;
    }
    public unsafe void Write(in nint addr, ReadOnlySpan<byte> data)
    {
        fixed (void* ptr = data) if (!Native.WriteProcessMemory(process.SafeHandle, addr, (nint)ptr, data.Length)) throw new InjectorException("Failed to write process memory", new Win32Exception());
    }

    ~ProcessMemory() => Dispose(false);
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    void Dispose(bool disposing)
    {
        allocs.AsParallel().ForAll(pair => Native.VirtualFreeEx(process.SafeHandle, pair.Item1, pair.Item2, 0x00008000));
        if (disposing) allocs.Clear();
    }
}