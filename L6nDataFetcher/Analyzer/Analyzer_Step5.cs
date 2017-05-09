using System.IO;

namespace L6nDataFetcher.Analyzer
{
    /// <summary>
    /// 解析结果导入数据库
    /// </summary>
    public class Analyzer_Step5 : AbsAnalyzer
    {
        protected override string Name { get; set; } = "Analyzer_Step5";

        protected override void _Process(params object[] args)
        {
            //TODO:把文件数据导入数据库

            using (StreamReader stream = new StreamReader(AppSettings.Step3File))
            {
                string line = string.Empty;
                while (!string.IsNullOrWhiteSpace(line = stream.ReadLine()))
                {
                    line.Split(new char[]
                    {
                        '$'
                    });
                }
            }
        }
    }
}