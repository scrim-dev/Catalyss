using System.Drawing;
using System.Text;
using Photino.NET;

namespace Catalyss
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            using var mutex = new Mutex(true, "CAT_INJ_APP", out bool isFirstInstance);
            if (!isFirstInstance)
            {
                MessageBox.Show("The injector is already opened!", "Catalyss", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string windowTitle = "Catalyss Injector v1.1";

            // Creating a new PhotinoWindow instance with the fluent API
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(1300, 750))
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                // Let's make this one fixed instead.
                .SetResizable(true)
                .RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
                {
                    contentType = "text/javascript";
                    return new MemoryStream(Encoding.UTF8.GetBytes(@"
                        (() =>{
                            window.setTimeout(() => {
                                alert(`Only inject when you're in the game's main menu!`);
                            }, 563);
                        })();
                    "));
                })
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler(InjectHandler.EventReceived)
                .Load("wwwroot/index.html"); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.WaitForClose(); // Starts the application event loop
        }
    }
}