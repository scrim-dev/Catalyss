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
            string windowTitle = "Catalyss Injector v1.0";

            // Creating a new PhotinoWindow instance with the fluent API
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(1000, 650))
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
                            }, 1000);
                        })();
                    "));
                })
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler(InjectHandler.EventReceived)
                .Load("wwwroot/index.html"); // Can be used with relative path strings or "new URI()" instance to load a website.
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

            window.WaitForClose(); // Starts the application event loop
        }
    }
}