using Dorado.Extensions;
using HtmlAgilityPack;
using System.IO;
using System.Text;

namespace L6nDataFetcher.Analyzer
{
    /// <summary>
    /// 解析body字段
    /// </summary>
    public class Analyzer_Step4 : AbsAnalyzer
    {
        private static string nameXpath = "//h1[@class='title']";
        private static string name1Xpath = "//h1[@class='org-top-card-module__name mb1 Sans-26px-black-85%-light']";

        private static string addressXpath = "//h4[@class='subtitle']";
        private static string address1Xpath = "//div[@class='school-location Sans-17px-black-100%-semibold mb1']";

        private static string siteUrlXPath = "//dd[@class='school-meta-data']/a";
        private static string siteUrl1XPath = "//dd[@class='org-about-company-module__company-page-url Sans-15px-black-70%']/a";
        private static string industryXPath = "//dd[@class='org-about-company-module__industry Sans-15px-black-70%']";
        private static string typeXPath = "//dd[@class='org-about-company-module__company-type Sans-15px-black-70%']";

        protected override string Name { get; set; } = "Analyzer_Step4";

        protected override void _Process(params object[] args)
        {
            //TODO:从数据库中分页读取，解析具体字段

            var body = string.Empty;
            var url = string.Empty;
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(body);
            HtmlNode nameNode = document.DocumentNode.SelectSingleNode(nameXpath);
            HtmlNode name1Node = document.DocumentNode.SelectSingleNode(name1Xpath);
            string name = string.Empty;
            string address = string.Empty;
            string siteUrl = string.Empty;
            string industry = string.Empty;
            string type = string.Empty;
            string arg_69_0 = string.Empty;
            if (nameNode != null)
            {
                name = nameNode.InnerText;
                HtmlNode node = document.DocumentNode.SelectSingleNode(addressXpath);
                if (node != null)
                {
                    address = node.InnerText;
                }
                node = document.DocumentNode.SelectSingleNode(siteUrlXPath);
                if (node != null && node.Attributes["href"] != null)
                {
                    siteUrl = node.Attributes["href"].Value;
                }
            }
            else if (name1Node != null)
            {
                name = name1Node.InnerText;
                HtmlNode node2 = document.DocumentNode.SelectSingleNode(address1Xpath);
                if (node2 != null)
                {
                    address = node2.InnerText;
                }
                node2 = document.DocumentNode.SelectSingleNode(siteUrl1XPath);
                if (node2 != null && node2.Attributes["href"] != null)
                {
                    siteUrl = node2.Attributes["href"].Value;
                }
                node2 = document.DocumentNode.SelectSingleNode(industryXPath);
                if (node2 != null)
                {
                    industry = node2.InnerText;
                }
                node2 = document.DocumentNode.SelectSingleNode(typeXPath);
                if (node2 != null)
                {
                    type = node2.InnerText;
                }
            }
            File.AppendAllText(AppSettings.Step3File, string.Format("{0}${1}${2}${3}${4}${5}\r\n", new object[]
            {
                name.ClearHtmlLite(),
                address.ClearHtmlLite(),
                siteUrl,
                industry.ClearHtmlLite(),
                type.ClearHtmlLite(),
                url
            }), Encoding.UTF8);
        }
    }
}