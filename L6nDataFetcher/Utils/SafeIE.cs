using System;
using System.Threading;
using WatiN.Core;

namespace L6nDataFetcher.Utils
{
    public class SafeIE : IDisposable
    {
        public string Url { get; private set; }
        public IE IE { get; private set; }

        public SafeIE(string url)
        {
            Url = url;
            try
            {
                this.IE = new IE(url);
            }
            catch (Exception ex)
            {
                Runtime.Instance.Error("初始化IE异常:url=" + url, ex);
            }
        }

        public bool HasBody { get { return IE != null && IE.Body != null; } }

        public void Dispose()
        {
            if (this.IE != null)
                this.IE.Close();

            Thread.Sleep(1000);
        }
    }
}