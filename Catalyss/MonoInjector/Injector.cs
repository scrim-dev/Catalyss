using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SharpMonoInjector;

public unsafe struct Injector : IDisposable
{
    const string getRootDomain = "mono_get_root_domain",
        threadAttach = "mono_thread_attach",
        openDataImage = "mono_image_open_from_data",
        openImageAsm = "mono_assembly_load_from_full",
        asmImage = "mono_assembly_get_image",
        matchClass = "mono_class_from_name",
        matchMethod = "mono_class_get_method_from_name",
        rtInvoke = "mono_runtime_invoke",
        asmClose = "mono_assembly_close",
        strErr = "mono_image_strerror",
        getClass = "mono_object_get_class",
        getName = "mono_class_get_name";

    readonly Dictionary<string, nint> exports = new()
    {
        { getRootDomain, 0 },
        { threadAttach, 0 },
        { openDataImage, 0 },
        { openImageAsm, 0 },
        { asmImage, 0 },
        { matchClass, 0 },
        { matchMethod, 0 },
        { rtInvoke, 0 },
        { asmClose, 0 },
        { strErr, 0 },
        { getClass, 0 },
        { getName, 0 }
    };

    readonly ProcessMemory memory;
    readonly Process process;
    readonly nint mono;

    bool attach;
    nint root;

    public bool Is64Bit { get; private set; }

    public Injector(ReadOnlySpan<char> processName) 
    {
        //processName = processName[..processName.IndexOf(".exe")];
        Is64Bit = ProcessUtils.Is64BitProcess(process = Process.GetProcessesByName(processName.ToString()).FirstOrDefault() ?? throw new InjectorException($"Couldn't find process with name '{processName}'"));
        if (!ProcessUtils.GetMonoModule(process, out mono)) throw new InjectorException("Error while finding mono in target process");

        memory = new(process);
    }
    public Injector(in int processId)
    {
        Is64Bit = ProcessUtils.Is64BitProcess(process = Process.GetProcessById(processId));
        if (!ProcessUtils.GetMonoModule(process, out mono)) throw new InjectorException("Error while finding mono in target process");
        memory = new(process);
    }
    public Injector(Process proc, in nint monoMod)
    {
        Is64Bit = ProcessUtils.Is64BitProcess(process = proc);
        mono = monoMod;
        memory = new(process);
    }
    public readonly void Dispose()
    {
        memory.Dispose();
        process.Dispose();
        exports.Clear();
    }

    public nint Inject(ReadOnlySpan<byte> rawAssembly, ReadOnlySpan<char> @namespace, ReadOnlySpan<char> className, ReadOnlySpan<char> methodName)
    {
        if (rawAssembly.IsEmpty) throw new ArgumentException($"{nameof(rawAssembly)} can't be empty", nameof(rawAssembly));
        if (className.IsEmpty) throw new ArgumentException($"{nameof(className)} can't be empty", nameof(className));
        if (methodName.IsEmpty) throw new ArgumentException($"{nameof(methodName)} can't be empty", nameof(methodName));

        ObtainMonoExports();
        root = GetRootDomain();
        attach = true;

        var assembly = OpenAssemblyFromImage(OpenImageFromData(rawAssembly));
        RuntimeInvoke(GetMethodFromName(GetClassFromName(GetImageFromAssembly(in assembly), @namespace, className), methodName));

        attach = false;
        return assembly;
    }
    public void Eject(in nint assembly, ReadOnlySpan<char> @namespace, ReadOnlySpan<char> className, ReadOnlySpan<char> methodName)
    {
        if (assembly == 0) throw new ArgumentException($"{nameof(assembly)} can't be zero", nameof(assembly));
        if (className.IsEmpty) throw new ArgumentException($"{nameof(className)} can't be empty", nameof(className));
        if (methodName.IsEmpty) throw new ArgumentException($"{nameof(methodName)} can't be empty", nameof(methodName));

        ObtainMonoExports();
        root = GetRootDomain();
        attach = true;

        RuntimeInvoke(GetMethodFromName(GetClassFromName(GetImageFromAssembly(in assembly), @namespace, className), methodName));
        CloseAssembly(in assembly);

        attach = false;
    }
    static void ThrowIfNull(in nint ptr, ReadOnlySpan<char> methodName)
    {
        if (ptr == 0) throw new InjectorException($"{methodName}() returned null");
    }

    readonly void ObtainMonoExports()
    {
        using ProcessMemory memory = new(process);

        var exportDir = mono + memory.Read<int>(mono + memory.Read<int>(mono + 0x3C) + 0x18 + (Is64Bit ? 0x70 : 0x60));
        var names = mono + memory.Read<int>(exportDir + 0x20);
        var ordinals = mono + memory.Read<int>(exportDir + 0x24);
        var funcs = mono + memory.Read<int>(exportDir + 0x1C);

        for (var i = 0; i < memory.Read<int>(exportDir + 0x18); ++i)
        {
            var name = memory.ReadString(mono + memory.Read<int>(names + i * 4), 32, Encoding.ASCII);
            if (exports.ContainsKey(name))
            {
                var addr = mono + memory.Read<int>(funcs + memory.Read<short>(ordinals + i * 2) * 4);
                if (addr == 0) throw new InjectorException($"Failed to get address of {name}()");
                exports[name] = addr;
            }
        }
    }
    readonly nint GetRootDomain()
    {
        var rootDomain = Execute(exports[getRootDomain], []);
        ThrowIfNull(in rootDomain, getRootDomain);
        return rootDomain;
    }
    readonly nint OpenImageFromData(ReadOnlySpan<byte> assembly)
    {
        var statusPtr = memory.Allocate(4);
        var rawImage = Execute(exports[openDataImage], [memory.AllocateAndWrite(assembly), assembly.Length, 1, statusPtr]);

        var status = memory.Read<int>(in statusPtr);
        if (status != 0) throw new InjectorException($"{openDataImage}() failed: {memory.ReadString(Execute(exports[strErr], [status]), 256, Encoding.ASCII)}");
        return rawImage;
    }
    readonly nint OpenAssemblyFromImage(in nint image)
    {
        var statusPtr = memory.Allocate(4);
        var assembly = Execute(exports[openImageAsm], [image, memory.Allocate(1), statusPtr, 0]);

        var status = memory.Read<int>(in statusPtr);
        if (status != 0) throw new InjectorException($"{openImageAsm}() failed: {memory.ReadString(Execute(exports[strErr], [status]), 256, Encoding.ASCII)}");
        return assembly;
    }
    readonly nint GetImageFromAssembly(in nint assembly)
    {
        var image = Execute(exports[asmImage], new(in assembly));
        ThrowIfNull(in image, asmImage);
        return image;
    }
    readonly nint GetClassFromName(in nint image, ReadOnlySpan<char> @namespace, ReadOnlySpan<char> className)
    {
        var @class = Execute(exports[matchClass], [image, memory.AllocateAndWrite(@namespace), memory.AllocateAndWrite(className)]);
        ThrowIfNull(in @class, matchClass);
        return @class;
    }
    readonly nint GetMethodFromName(in nint @class, ReadOnlySpan<char> methodName)
    {
        var method = Execute(exports[matchMethod], [@class, memory.AllocateAndWrite(methodName), 0]);
        ThrowIfNull(in method, matchMethod);
        return method;
    }
    readonly ReadOnlySpan<char> GetClassName(in nint monoObject)
    {
        var @class = Execute(exports[getClass], new(in monoObject));
        ThrowIfNull(in @class, getClass);

        var className = Execute(exports[getName], new(in @class));
        ThrowIfNull(in className, getName);

        return memory.ReadString(in className, 256, Encoding.ASCII);
    }
    readonly ReadOnlySpan<char> ReadMonoString(in nint monoString)
        => memory.ReadString(monoString + (Is64Bit ? 0x14 : 0xC), memory.Read<int>(monoString + (Is64Bit ? 0x10 : 0x8)) * 2, Encoding.Unicode);

    readonly void RuntimeInvoke(in nint method)
    {
        var excPtr = Is64Bit ? memory.Allocate(8) : memory.Allocate(4);
        Execute(exports[rtInvoke], [method, 0, 0, excPtr]);

        var exc = (nint)memory.Read<int>(in excPtr);
        if (exc != 0) throw new InjectorException($"Managed method threw exception: ({GetClassName(exc)}) {ReadMonoString(memory.Read<int>(exc + (Is64Bit ? 0x20 : 0x10)))}");
    }
    readonly void CloseAssembly(in nint assembly) => ThrowIfNull(Execute(exports[asmClose], new(in assembly)), asmClose);

    readonly nint Execute(in nint addr, ReadOnlySpan<nint> args)
    {
        var retValPtr = Is64Bit ? memory.Allocate(8) : memory.Allocate(4);

        var thread = Native.CreateRemoteThread(process.SafeHandle, 0, 0, memory.AllocateAndWrite(Assemble(in addr, in retValPtr, args)), 0, 0, out _);
        if (thread == 0) throw new InjectorException("Failed to create remote thread", new Win32Exception());

        if (Native.WaitForSingleObject(thread, -1) == 0xFFFFFFFF) throw new InjectorException("Failed to wait for remote thread", new Win32Exception());
        var ret = Is64Bit ? (nint)memory.Read<long>(in retValPtr) : memory.Read<int>(in retValPtr);
        if (ret == 0x00000000C0000005) throw new InjectorException($"Access violation while executing {exports.First(e => e.Value == ret).Key}()");

        return ret;
    }

    readonly ReadOnlySpan<byte> Assemble(in nint funcPtr, in nint retValPtr, ReadOnlySpan<nint> args) => Is64Bit ? Assemble64(in funcPtr, in retValPtr, args) : Assemble86(in funcPtr, in retValPtr, args);
    readonly ReadOnlySpan<byte> Assemble86(in nint funcPtr, in nint retValPtr, ReadOnlySpan<nint> args)
    {
        Assembler asm = new();
        if (attach)
        {
            asm.Push(in root);
            asm.MovEax(exports[threadAttach]);
            asm.CallEax();
            asm.AddEsp(4);
        }

        var size = args.Length;
        for (var i = size - 1; i >= 0; --i) asm.Push(in args[i]);

        asm.MovEax(in funcPtr);
        asm.CallEax();
        asm.AddEsp((byte)(size << 2));
        asm.MovEaxTo(in retValPtr);
        asm.Return();

        return asm.Compile();
    }
    readonly ReadOnlySpan<byte> Assemble64(in nint funcPtr, in nint retValPtr, ReadOnlySpan<nint> args)
    {
        Assembler asm = new();

        asm.SubRsp(40);
        if (attach)
        {
            asm.MovRax(exports[threadAttach]);
            asm.MovRcx(in root);
            asm.CallRax();
        }
        asm.MovRax(funcPtr);

        for (var i = 0; i < args.Length; ++i) switch (i)
        {
            case 0: asm.MovRcx(in args[i]); break;
            case 1: asm.MovRdx(in args[i]); break;
            case 2: asm.MovR8(in args[i]); break;
            case 3: asm.MovR9(in args[i]); break;
        }

        asm.CallRax();
        asm.AddRsp(40);
        asm.MovRaxTo(in retValPtr);
        asm.Return();

        return asm.Compile();
    }
}