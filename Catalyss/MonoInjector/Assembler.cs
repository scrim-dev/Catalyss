using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpMonoInjector;

public readonly ref struct Assembler
{
    readonly List<byte> ops;
    public Assembler() => ops = [];

    public void CallRax() => AddStack([0xFF, 0xD0]);
    public void MovRax(in nint arg)
    {
        AddStack([0x48, 0xB8]);
        AddArgAsBytes(in arg);
    }
    public void MovRaxTo(in nint arg)
    {
        AddStack([0x48, 0xA3]);
        AddArgAsBytes(in arg);
    }
    public void MovRcx(in nint arg)
    {
        AddStack([0x48, 0xB9]);
        AddArgAsBytes(in arg);
    }
    public void MovRdx(in nint arg)
    {
        AddStack([0x48, 0xBA]);
        AddArgAsBytes(in arg);
    }

    public void CallEax() => AddStack([0xFF, 0xD0]);
    public unsafe void MovEax(in nint arg)
    {
        ops.Add(0xB8);
        AddArgAsBytes(in Unsafe.AsRef<int>((void*)arg));
    }
    public void MovEaxTo(in nint arg)
    {
        ops.Add(0xA3);
        AddArgAsBytes(in arg);
    }
    public void MovR8(in nint arg)
    {
        AddStack([0x49, 0xB8]);
        AddArgAsBytes(in arg);
    }
    public void MovR9(in nint arg)
    {
        AddStack([0x49, 0xB9]);
        AddArgAsBytes(in arg);
    }

    public void SubRsp(in byte arg) => AddStack([0x48, 0x83, 0xEC, arg]);
    public void AddRsp(in byte arg) => AddStack([0x48, 0x83, 0xC4, arg]);
    public void AddEsp(in byte arg) => AddStack([0x83, 0xC4, arg]);

    public unsafe void Push(in nint arg)
    {
        var ptr = (void*)arg;
        ref readonly var intArg = ref Unsafe.AsRef<int>(ptr);

        ops.Add(intArg < 128 ? (byte)0x6A : (byte)0x68);
        if (intArg > 255) AddArgAsBytes(in intArg);
        else ops.Add(Unsafe.AsRef<byte>(ptr));
    }

    public void Return() => ops.Add(0xC3);
    public ReadOnlySpan<byte> Compile() => CollectionsMarshal.AsSpan(ops);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    unsafe void AddArgAsBytes<T>(in T arg) where T : unmanaged
    {
        fixed (void* ptr = &arg) AddStack(new(ptr, sizeof(T)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void AddStack(in ReadOnlySpan<byte> arg) => ops.AddRange(arg);
}