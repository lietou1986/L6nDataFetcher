using Dorado.Core;
using Dorado.Core.Logger;
using System;

namespace L6nDataFetcher
{
    public class Runtime : LazySingleton<Runtime>
    {
        public void Error(string title, Exception ex)
        {
            Console.WriteLine(ex);
            LoggerWrapper.Logger.Error(title, ex);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(string.Format(message, args));
        }
    }
}