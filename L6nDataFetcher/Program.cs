using System;
using System.Threading;

namespace L6nDataFetcher
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                Console.Title = "linkin data fetch";
                Console.ForegroundColor = ConsoleColor.Green;
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Thread.Sleep(TimeSpan.FromMinutes(AppSettings.StartupWaitMinute));
                Startup.Start(args);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.ToString());
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            Exception e = ex.ExceptionObject as Exception;
            Console.WriteLine(e.ToString());
        }
    }
}