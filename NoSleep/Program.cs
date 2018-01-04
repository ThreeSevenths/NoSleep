using System;

namespace NoSleep
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Platform not supported. This app only works with Windows");
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine("NoSleep. Your PC will not go to sleep or turn off the display.");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Press any key to restore normal sleep...");
            Console.ResetColor();
            Console.WriteLine();

            var result = NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);

            Console.Read();
            
            Console.WriteLine("Restoring sleep operation");

            result = NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}
