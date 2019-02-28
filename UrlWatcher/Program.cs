using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace UrlWatcher
{
    class Program
    {
        private const int RefreshIntervalMs = 5000;

        private static void Main(string[] args)
        {
            while (true)
            {
                PrintInfo("Checking...");
                Thread.Sleep(RefreshIntervalMs);
                RssChecker.Refresh();
            }
        }

        internal static void PrintInfo(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
        }

        internal static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        [DllImport("user32.dll", EntryPoint = "FlashWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _flashWindow(
            IntPtr hWnd,
            [MarshalAs(UnmanagedType.Bool)]
            bool bInvert);

        internal static void FlashWindow()
        {
            var handle = Process.GetCurrentProcess().MainWindowHandle;
            _flashWindow(handle, false);
        }
    }
}
