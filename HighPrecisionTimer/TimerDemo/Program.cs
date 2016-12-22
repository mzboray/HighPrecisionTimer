using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HighPrecisionTimer;

namespace TimerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadingResult = TestThreadingTimer();
            var timerResult = TestMultimediaTimer();
            var taskResult = TestTask();

            Console.WriteLine($"Threading timer: {threadingResult.Summary()}");
            Console.WriteLine($"Multimedia timer: {timerResult.Summary()}");
            Console.WriteLine($"MultimediaTimer.Delay: {taskResult.Summary()}");
        }

        private static TimingResult TestMultimediaTimer()
        {
            TimeSpan total = TimeSpan.Zero;
            int iterations = 0;
            Stopwatch s = new Stopwatch();
            using (var timer = new MultimediaTimer() { Interval = 1 })
            {
                timer.Elapsed += (o, e) =>
                {
                    var ts = s.Elapsed;
                    lock (s)
                    {
                        total += ts;
                        iterations++;
                    }
                    Console.WriteLine(ts.TotalMilliseconds);
                };
                s.Start();
                timer.Start();
                Console.ReadKey(true);
                timer.Stop();
            }

            lock (s)
            {
                return new TimingResult()
                {
                    Elapsed = s.Elapsed,
                    Iterations = iterations
                };
            }
        }

        private static TimingResult TestThreadingTimer()
        {
            TimeSpan total = TimeSpan.Zero;
            int iterations = 0;
            Stopwatch s = new Stopwatch();
            using (var timer = new Timer(o =>
            {
                var ts = s.Elapsed;
                lock (s)
                {
                    total += ts;
                    iterations++;
                }
                Console.WriteLine(ts.TotalMilliseconds);

            }, null, 0, 1))
            {
                s.Start();
                Console.ReadKey(true);
            }

            lock (s)
            {
                return new TimingResult()
                {
                    Elapsed = s.Elapsed,
                    Iterations = iterations
                };
            }
        }

        private static TimingResult TestTask()
        {
            try
            {
                using (var cancelled = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (o, e) => { cancelled.Cancel(); };
                    var result = TestTaskInner(cancelled.Token).GetAwaiter().GetResult();
                    return result;
                }
            }
            catch (OperationCanceledException)
            {
                return default(TimingResult);
            }
        }

        private static async Task<TimingResult> TestTaskInner(CancellationToken token)
        {
            Stopwatch s = Stopwatch.StartNew();
            TimeSpan prevValue = TimeSpan.Zero;
            int i = 0;
            while (true)
            {
                Console.WriteLine(s.Elapsed.TotalMilliseconds);
                await MultimediaTimer.Delay(1, token);
                if (Console.KeyAvailable)
                {
                    break;
                }

                i++;
            }

            return new TimingResult()
            {
                Elapsed = s.Elapsed,
                Iterations = i
            };
        }
    }

    struct TimingResult
    {
        public TimeSpan Elapsed;

        public int Iterations;

        public string Summary()
        {
            return $"{Elapsed.TotalMilliseconds / Iterations} ms / iteration";
        }
    }
}
