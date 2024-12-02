namespace SharpMonoInjector;

public readonly struct ExportedFunction(string name, nint addr)
{
    public readonly string Name = name;
    public readonly nint Address = addr;
}