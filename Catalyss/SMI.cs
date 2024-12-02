using SharpMonoInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Catalyss
{
    //https://github.com/warbler/SharpMonoInjector
    //Modified from: https://github.com/warbler/SharpMonoInjector/blob/master/src/SharpMonoInjector.Console/Program.cs
    internal class SMI
    {
        //CatalyssMod
        //Entry
        //Load
        public static void Inject(Injector injector)
        {
            byte[] Assem = File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\CatalyssMod.dll"); //Path
            using (injector)
            {
                nint remoteAssembly = 0;

                try
                {
                    remoteAssembly = injector.Inject(Assem, "CatalyssMod", "Entry", "Load");
                }
                catch (InjectorException ie)
                {
                    MessageBox.Show($"Failed to inject assembly: {ie}", "Catalyss.SharpMonoInjector");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to inject assembly (unknown error): {e}", "Catalyss.SharpMonoInjector");
                }

                if (remoteAssembly == 0)
                    return;

                MessageBox.Show($"{Path.GetFileName($"{Directory.GetCurrentDirectory()}\\CatalyssMod.dll")}: " +
                    (injector.Is64Bit
                    ? $"0x{remoteAssembly.ToInt64():X16}"
                    : $"0x{remoteAssembly.ToInt32():X8}"));
            }
        }

        //Need to change this before release
        public static void Eject(Injector injector)
        {
            //IntPtr Assem = 
            using (injector)
            {
                /*try
                {
                    injector.Eject(Assem, "CatalyssMod", "Entry", "Unload");
                    MessageBox.Show("Ejection successful");
                }
                catch (InjectorException ie)
                {
                    MessageBox.Show("Ejection failed: " + ie);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ejection failed (unknown error): " + exc);
                }*/
            }
        }
    }
}
