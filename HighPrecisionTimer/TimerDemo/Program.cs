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
    }
}
