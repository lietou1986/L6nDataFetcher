using L6nDataFetcher.Analyzer;

namespace L6nDataFetcher
{
    public class Startup
    {
        public static void Start(string[] args)
        {
            // new Analyzer_Step0().Process();
            // new Analyzer_Step1().Process();
            // new Analyzer_Step2().Process();
            new Analyzer_Step3().Process();
            new Analyzer_Step4().Process();
            new Analyzer_Step5().Process();
        }
    }
}