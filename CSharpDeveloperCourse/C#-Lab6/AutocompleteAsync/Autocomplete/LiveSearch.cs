namespace Autocomplete
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LiveSearch
    {
        private static readonly string[] SimpleWords = File.ReadAllLines(@"dict/words.txt");
        private static readonly string[] MovieTitles = File.ReadAllLines(@"dict/movies.txt");
        private static readonly string[] StageNames = File.ReadAllLines(@"dict/stage names.txt");

        private CancellationTokenSource token;

        public async Task<string> FindBestSimilarAsync(string example)
        {
            if (this.token != null)
            {
                this.token.Cancel();
            }

            this.token = new CancellationTokenSource();

            var stageResult = await BestSimilarInArrayAsync(StageNames, example, this.token);

            if (stageResult.Sim == -1)
            {
                return string.Empty;
            }

            var movieResult = await BestSimilarInArrayAsync(MovieTitles, example, this.token);

            if (movieResult.Sim == -1)
            {
                return string.Empty;
            }

            var wordResult = await BestSimilarInArrayAsync(SimpleWords, example, this.token);

            if (wordResult.Sim == -1)
            {
                return string.Empty;
            }

            if (wordResult.Sim > movieResult.Sim && wordResult.Sim > stageResult.Sim)
            {
                return wordResult.Line;
            }

            if (movieResult.Sim > stageResult.Sim || (movieResult.Sim == stageResult.Sim && movieResult.Line.Length <= stageResult.Line.Length))
            {
                return movieResult.Line;
            }

            return stageResult.Line;
        }

        public async void HandleTyping(HintedControl control)
        {
            control.Hint = await this.FindBestSimilarAsync(control.LastWord);
        }

        internal static async Task<SimilarLine> BestSimilarInArrayAsync(string[] lines, string example, CancellationTokenSource currentToken)
        {
            var task = Task.Factory.StartNew<SimilarLine>(
            (obj) =>
            {
                var ct = (CancellationTokenSource)obj;
                var best = new SimilarLine() { Line = string.Empty, Sim = 0 };

                foreach (var line in lines)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return new SimilarLine() { Line = string.Empty, Sim = -1 };
                    }

                    var sim = line.Similarity(example);
                    if ((sim > best.Sim) || (sim == best.Sim && line.Length < best.Line.Length))
                    {
                        best.Line = line;
                        best.Sim = sim;
                    }
                }

                return best;
                }, 
            currentToken);

            return await task;
        }

        internal struct SimilarLine
        {
            internal string Line;
            internal int Sim;
        }
    }
}