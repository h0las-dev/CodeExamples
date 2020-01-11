namespace Autocomplete
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public sealed class LiveSearch
    {
        private static readonly string[] SimpleWords = File.ReadAllLines(@"dict/words.txt");
        private static readonly string[] MovieTitles = File.ReadAllLines(@"dict/movies.txt");
        private static readonly string[] StageNames = File.ReadAllLines(@"dict/stage names.txt");

        private static SimilarLine stageResult;
        private static SimilarLine movieResult;
        private static SimilarLine wordResult;

        private static AutoThread thread1 = new AutoThread(null, new AutoResetEvent(false));
        private static AutoThread thread2 = new AutoThread(null, new AutoResetEvent(false));
        private static AutoThread thread3 = new AutoThread(null, new AutoResetEvent(false));

        private Thread searchThread;

        private SearchExample searchExample = new SearchExample(string.Empty, null);

        public string FindBestSimilar(string example)
        {
            if (this.searchExample.Token != null)
            {
                this.searchExample.Token.Cancel();

                if (thread1.Thread != null)
                {
                    thread1.Thread.Join();
                }

                if (thread2.Thread != null)
                {
                    thread2.Thread.Join();
                }

                if (thread3.Thread != null)
                {
                    thread3.Thread.Join();
                }
            }

            this.searchExample.Token = new CancellationTokenSource();
            this.searchExample.StringExample = example;

            thread1.HandleThread.Reset();
            thread1.Thread = new Thread(new ParameterizedThreadStart(SearchInWordsParallel));
            thread1.Thread.Start(this.searchExample);
            thread1.HandleThread.WaitOne();

            if (wordResult.Sim == -1)
            {
                return string.Empty;
            }

            thread2.HandleThread.Reset();
            thread2.Thread = new Thread(new ParameterizedThreadStart(SearchInStageParallel));
            thread2.Thread.Start(this.searchExample);
            thread2.HandleThread.WaitOne();

            if (stageResult.Sim == -1)
            {
                return string.Empty;
            }

            thread3.HandleThread.Reset();
            thread3.Thread = new Thread(new ParameterizedThreadStart(SearchInMovieParallel));
            thread3.Thread.Start(this.searchExample);
            thread3.HandleThread.WaitOne();

            if (movieResult.Sim == -1)
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

        public void HandleTyping(HintedControl control)
        {
            if (this.searchThread != null)
            {
                this.searchExample.Token.Cancel();
            }

            this.searchThread = new Thread(
                () =>
                {
                    control.Hint = this.FindBestSimilar(control.LastWord);
                });

            this.searchThread.Start();
        }

        internal static SimilarLine BestSimilarInArray(string[] lines, string example, CancellationTokenSource token)
        {
            /* return lines.Aggregate(
                 new SimilarLine
                 {
                     Line = string.Empty,
                     Sim = 0
                 },
                 (best, line) =>
                 {
                     var current = new SimilarLine
                     {
                         Line = line,
                         Sim = line.Similarity(example)
                     };

                     if ((current.Sim > best.Sim) || (current.Sim == best.Sim && current.Line.Length < best.Line.Length))
                     {
                         return current;
                     }

                     return best;
                 }); */

            var best = new SimilarLine() { Line = string.Empty, Sim = 0 };

            foreach (var line in lines)
            {
                if (token.IsCancellationRequested)
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
        }

        private static void SearchInWordsParallel(object example)
        {
            var searchExample = (SearchExample)example;
            wordResult = BestSimilarInArray(SimpleWords, searchExample.StringExample, searchExample.Token);
            thread1.HandleThread.Set();
        }

        private static void SearchInStageParallel(object example)
        {
            var searchExample = (SearchExample)example;
            stageResult = BestSimilarInArray(StageNames, searchExample.StringExample, searchExample.Token);
            thread2.HandleThread.Set();
        }

        private static void SearchInMovieParallel(object example)
        {
            var searchExample = (SearchExample)example;
            movieResult = BestSimilarInArray(MovieTitles, searchExample.StringExample, searchExample.Token);
            thread3.HandleThread.Set();
        }

        internal struct SimilarLine
        {
            internal string Line;
            internal int Sim;
        }
    }
}