using Dorado;
using L6nDataFetcher.Services;
using L6nDataFetcher.Utils;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using WatiN.Core;

/// <summary>
///  bcookie="v=2&2d5db89f-d445-4e38-881a-9b8ca908937e";
///  bscookie="v=1&201609220605361a073dad-ee0b-48c2-8e6e-6728b8c184a6AQF0eHgx6yK4ZP1mGkOGpHj36wBAHLWK";
/// </summary>
namespace L6nDataFetcher.Analyzer
{
    /// <summary>
    /// 页面body写入数据库等待下次解析使用(使用传统抓取方式)
    /// </summary>
    public class Analyzer_Step3ByProxy : Analyzer_Step3
    {
        private CookieContainer CreateCookies()
        {
            try
            {
                var ie = new IE();
                return ie.GetCookieContainerForUrl(new Uri("https://www.linkedin.com/edu/a.m.-reddy-memorial-college-of-pharmacy-206173"));
            }
            catch (Exception ex)
            {
                Runtime.Instance.Error("初始化CookieContainer异常", ex);
                return null;
            }
        }

        protected override void _Process(params object[] args)
        {
            CookieContainer cookieContainer = CreateCookies();
            if (cookieContainer == null) throw new CoreException("无法初始化cookie");

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

                            RequestService requestService = new RequestService(url);
                            requestService.SetCookieContainer(cookieContainer);
                            var requestResult = requestService.Request<string>();
                            if (requestResult.Status == OperateStatus.Success)
                            {
                                string body = requestResult.Data;

                                if (body.Contains("403") || body.Contains("errorpg") || body.Contains("恢复") || body.Length < 40000)//如果被禁用，跳出
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

                                index++;
                                Runtime.Instance.Info(index + "->" + url);
                                if (index % 100 == 0)//100条随机休息
                                {
                                    int sleep = new Random().Next(10);
                                    Runtime.Instance.Info("-----------------sleep [{0}] seconds---------------------", sleep);
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