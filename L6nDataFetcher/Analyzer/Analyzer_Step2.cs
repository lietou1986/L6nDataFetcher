using Dorado;
using HtmlAgilityPack;
using L6nDataFetcher.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace L6nDataFetcher.Analyzer
{
    public class Analyzer_Step2 : AbsAnalyzer
    {
        private string xpath = "//ul[@class='column dual-column']/li";

        protected override string Name { get; set; } = "Analyzer_Step2";

        protected override void _Process(params object[] args)
        {
            using (StreamReader stream = new StreamReader(AppSettings.Step1File))
            {
                string line = string.Empty;
                while (!string.IsNullOrWhiteSpace(line = stream.ReadLine()))
                {
                    string[] data = line.Split(new char[] { '$' });
                    string url = data[1];
                    using (SafeIE ie = new SafeIE(url))
                    {
                        if (ie.HasBody)
                        {
                            var body = ie.IE.Body.OuterHtml;

                            try
                            {
                                HtmlDocument document = new HtmlDocument();
                                document.LoadHtml(body);
                                HtmlNodeCollection dataNodeList = document.DocumentNode.SelectNodes(xpath);

                                if (dataNodeList == null)
                                    throw new CoreException("没有找到内容html节点");

                                var aList = dataNodeList.Select(node => node.SelectSingleNode("./a")).Where(n => n != null).ToList();
                                List<string> lines = new List<string>();
                                aList.ForEach(n =>
                                {
                                    string title = string.Empty, href = string.Empty;
                                    if (n.Attributes["title"] != null)
                                    {
                                        title = n.Attributes["title"].Value;
                                    }
                                    if (n.Attributes["href"] != null)
                                    {
                                        href = n.Attributes["href"].Value;
                                    }

                                    lines.Add(string.Format("{0}${1}", title, href));
                                });
                                //写入url到文件
                                File.AppendAllLines(AppSettings.Step2File, lines, Encoding.UTF8);
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
}