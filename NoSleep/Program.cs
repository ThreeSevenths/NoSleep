/*
 * THE MIT LICENSE
 *
 * Copyright 2018 TREVOR DAVIS
 *
 * Permission is hereby granted, free of charge, to any person obtaining a 
 * copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 * 
*/
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
