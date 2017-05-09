using System.IO;

namespace L6nDataFetcher.Analyzer
{
    /// <summary>
    /// 生成中间文件
    /// </summary>
    public class Analyzer_Step0 : AbsAnalyzer
    {
        protected override string Name { get; set; } = "Analyzer_Step0";

        protected override void _Process(params object[] args)
        {
            CreateFile(AppSettings.Step1File, AppSettings.Step2File, AppSettings.Step3File);
        }

        private static void CreateFile(params string[] fileNames)
        {
            foreach (string f in fileNames)
            {
                if (!File.Exists(f))
                {
                    File.Create(f);
                }
            }
        }
    }
}