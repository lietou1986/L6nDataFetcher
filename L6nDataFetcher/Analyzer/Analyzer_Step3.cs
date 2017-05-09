using L6nDataFetcher.Utils;
using System;
using System.Data;
using System.IO;
using System.Threading;

namespace L6nDataFetcher.Analyzer
{
    /// <summary>
    /// 页面body写入数据库等待下次解析使用(采用人工模式方式采集)
    /// </summary>
    public class Analyzer_Step3 : AbsAnalyzer
    {
        protected static string insertSql = "INSERT INTO [body] VALUES(@URL,@BODY)";

        protected override string Name { get; set; } = "Analyzer_Step3";

        /// <summary>
        /// 判断是否被禁用
        /// </summary>
        protected bool IsForbid { get; set; }

        protected override void _Process(params object[] args)
        {
            Runtime.Instance.Info(AppSettings.Step2File);

            using (SafeSQLite conn = new SafeSQLite(AppSettings.DBConnection, true))
            {
                using (StreamReader stream = new StreamReader(AppSettings.Step2File))
                {
                    int index = 0;
                    string line = string.Empty;
                    while (!string.IsNullOrWhiteSpace(line = stream.ReadLine()))
                    {
                        try
                        {
                            //如果被禁用，我等你释放我
                            if (IsForbid) Thread.Sleep(TimeSpan.FromHours(AppSettings.ForbidWaitHour));

                            string[] data = line.Split(new char[] { '$' });
                            string url = data[1];

                            using (SafeIE ie = new SafeIE(url))
                            {
                                if (ie.HasBody)
                                {
                                    var body = ie.IE.Body.OuterHtml;
                                    if (body.Contains("errorpg") || body.Contains("恢复") || body.Length < 40000)//如果被禁用，跳出
                                    {
                                        IsForbid = true;
                                        Runtime.Instance.Info("------------------被禁止了---------------------------");
                                        continue;
                                    }

                                    IsForbid = false;

                                    var par1 = conn.CreateParameter("@URL", DbType.String, url);
                                    var par2 = conn.CreateParameter("@BODY", DbType.String, body);
                                    var cmd = conn.CreateCommand(insertSql, par1, par2);
                                    conn.ExecuteNonQuery(cmd);
                                }
                                index++;
                                Runtime.Instance.Info(index + "->" + url);
                                if (index % 100 == 0)//100条随机休息
                                {
                                    int sleep = new Random().Next(10);
                                    Runtime.Instance.Info("-----------------sleep [{0}] minutes---------------------", sleep);
                                    Thread.Sleep(TimeSpan.FromMinutes(sleep));
                                }
                                else
                                {
                                    int sleep = new Random().Next(2);
                                    Thread.Sleep(TimeSpan.FromSeconds(sleep));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Runtime.Instance.Error(ex.Message, ex);
                        }
                    }
                }
            }
        }
    }
}