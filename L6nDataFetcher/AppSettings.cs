using Dorado.Extensions;
using System.Configuration;

namespace L6nDataFetcher
{
    public class AppSettings
    {
        public static string DBConnection { get => ConfigurationManager.ConnectionStrings["Default"].ToString(); }
        public static string Step1File { get => ConfigurationManager.AppSettings.Get("Step1File"); }
        public static string Step2File { get => ConfigurationManager.AppSettings.Get("Step2File"); }
        public static string Step3File { get => ConfigurationManager.AppSettings.Get("Step3File"); }
        public static int ForbidWaitHour { get => ConfigurationManager.AppSettings.Get("ForbidWaitHour").Int(); }
        public static int StartupWaitMinute { get => ConfigurationManager.AppSettings.Get("StartupWaitMinute").Int(); }
    }
}