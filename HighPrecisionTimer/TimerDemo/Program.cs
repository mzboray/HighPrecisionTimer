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
            TestThreadingTimer();
            TestMultimediaTimer();
            TestTask();
        }

        private static void TestMultimediaTimer()
        {
            Stopwatch s = new Stopwatch();
            using (var timer = new MultimediaTimer() { Interval = 1 })
            {
                timer.Elapsed += (o, e) => Console.WriteLine(s.ElapsedMilliseconds);
                s.Start();
                timer.Start();
                Console.ReadKey(true);
                timer.Stop();
            }
        }

        private static void TestThreadingTimer()
        {
            Stopwatch s = new Stopwatch();
            using (var timer = new Timer(o => Console.WriteLine(s.ElapsedMilliseconds), null, 0, 1))
            {
                s.Start();
                Console.ReadKey(true);
            }
        }

        private static void TestTask()
        {
            try
            {
                using (var cancelled = new CancellationTokenSource())
                {
                    Console.CancelKeyPress += (o, e) => { cancelled.Cancel(); };
                    TestTaskInner(cancelled.Token).GetAwaiter().GetResult();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private static async Task TestTaskInner(CancellationToken token)
        {
            Stopwatch s = Stopwatch.StartNew();
            TimeSpan prevValue = TimeSpan.Zero;
            int i = 0;
            while (true)
            {
                Console.WriteLine(s.ElapsedMilliseconds);
                await MultimediaTimer.Delay(1, token);
                if (Console.KeyAvailable)
                {
                    return;
                }

                i++;
            }
        }
    }
}
