﻿using Photino.NET;
using SharpMonoInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyss
{
    internal class InjectHandler
    {
        private static Injector MonoInjector;

        private static void LaunchGame()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "steam://rungameid/2768430",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        public static void EventReceived(object sender, string message)
        {
            //var window = (PhotinoWindow)sender;
            /*string response = $"Received message: \"{message}\"";
            window.SendWebMessage(response);*/
            Console.WriteLine($"{sender}: {message}");

            if (message == "InjectMonoDll")
            {
                Console.WriteLine($"{sender}: {message}");
                MessageBox.Show("Press OK to start injection process.", "Catalyss");

                if (IsAtlyssProcessRunning())
                {
                    MonoInjector = new Injector("ATLYSS");
                    SMI.Inject(MonoInjector);
                }
                else
                {
                    MessageBox.Show("Atlyss game process is NOT active!");
                }
            }
            else if (message == "EjectMonoDll")
            {
                Console.WriteLine($"{sender}: {message}");
                MessageBox.Show("Press OK to unload .dll", "Catalyss");

                if (IsAtlyssProcessRunning())
                {
                    MonoInjector = new Injector("ATLYSS");
                    SMI.Eject(MonoInjector);
                }
                else
                {
                    MessageBox.Show("Atlyss game process is NOT active!");
                }
            }
            else if (message == "StartGame")
            {
                LaunchGame();
            }
            else if(message == "QuitGame")
            {
                QuitGame();
            }
        }

        private static void QuitGame()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("ATLYSS");
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch { }
        }

        private static bool IsAtlyssProcessRunning()
        {
            Process[] processes = Process.GetProcessesByName("ATLYSS");
            return processes.Length > 0;
        }
    }
}
