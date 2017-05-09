using System;

namespace L6nDataFetcher.Analyzer
{
    public abstract class AbsAnalyzer
    {
        protected abstract string Name { get; set; }

        protected abstract void _Process(params object[] args);

        public void Process(params object[] args)
        {
            try
            {
                Runtime.Instance.Info(string.Format("{0}-begin", Name));

                _Process(args);
            }
            catch (Exception ex)
            {
                Runtime.Instance.Error(ex.Message, ex);
            }
            finally
            {
                Runtime.Instance.Info(string.Format("{0}-end", Name));
            }
        }
    }
}