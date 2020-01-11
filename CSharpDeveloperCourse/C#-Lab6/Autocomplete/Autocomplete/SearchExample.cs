namespace Autocomplete
{
    using System.Threading;

    public class SearchExample
    {
        private string stringExample;
        private CancellationTokenSource token;

        public SearchExample(string str, CancellationTokenSource ct)
        {
            this.stringExample = str;
            this.token = ct;
        }

        public string StringExample
        {
            get { return this.stringExample; }
            set { this.stringExample = value; }
        }

        public CancellationTokenSource Token
        {
            get { return this.token; }
            set { this.token = value; }
        }
    }
}