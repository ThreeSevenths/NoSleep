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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace NoSleep
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("NoSleep. Your PC will not go to sleep or turn off the display.");
            Console.WriteLine();
            
            if (OperatingSystem.IsWindows())
            {
                

                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Press any key to restore normal sleep...");
                Console.ResetColor();
                Console.WriteLine();

                var result = NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);

                Console.Read();

                Console.WriteLine("Restoring sleep operation");

                result = NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
            }
            else if (OperatingSystem.IsLinux())
            {
                using var dbus = new DBusConnection(DBusAddress.System);

                await dbus.ConnectAsync();

                var services = await dbus.ListServicesAsync();

                var loginservicename = services.FirstOrDefault(svc => svc.StartsWith("org.freedesktop.login1", StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrWhiteSpace((loginservicename)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("DBus does not have a service org.freedesktop.login1.");
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    var login1service = new login1.DBus.Manager(dbus, loginservicename, "/org/freedesktop/login1");

                    var maxinhibitors = await login1service.GetInhibitorsMaxAsync();

                    if (maxinhibitors < 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("GetInhibitorsMax reports less than 1 supported inhibitor.");
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                    
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Press any key to restore normal sleep...");
                    Console.ResetColor();
                    Console.WriteLine();

                    using (var inhibithandle = await login1service.InhibitAsync("sleep", "NoSleep",
                               "User is running NoSleep. Sleep is blocked until closed.", "block"))
                    {
                        Console.Read();
                        Console.WriteLine("Restoring sleep operation");
                    }
                    
                    
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: Platform not supported. This app only works with Windows");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
}
