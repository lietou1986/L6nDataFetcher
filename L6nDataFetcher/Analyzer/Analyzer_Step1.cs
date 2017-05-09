using Dorado;
using HtmlAgilityPack;
using L6nDataFetcher.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace L6nDataFetcher.Analyzer
{
    public class Analyzer_Step1 : AbsAnalyzer
    {
        private static List<char> chars = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        private string xpath = "//ul[@class='column dual-column']/li";

        protected override string Name { get; set; } = "Analyzer_Step1";

        protected override void _Process(params object[] args)
        {
            chars.ForEach(n =>
            {
                var url = string.Format("https://www.linkedin.com/directory/universities-{0}/", n);

                using (SafeIE ie = new SafeIE(url))
                {
                    if (ie.HasBody)
                    {
                        var body = ie.IE.Body.OuterHtml;
                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(body);
                        HtmlNodeCollection dataNodeList = document.DocumentNode.SelectNodes(xpath);

                        if (dataNodeList == null)
                            throw new CoreException("没有找到内容html节点");

                        List<string> dataUrlList = dataNodeList.Select(node => node.SelectSingleNode("./a")).Where(m => m != null).Select(m => m.Attributes["href"].Value).ToList();
                        List<string> lines = new List<string>();
                        dataUrlList.ForEach(d =>
                        {
                            lines.Add(string.Format("{0}${1}", args[0], d));
                        });

                        //写入url到文件
                        File.AppendAllLines(AppSettings.Step1File, lines, Encoding.UTF8);
                    }
                }
            });
        }
    }
}