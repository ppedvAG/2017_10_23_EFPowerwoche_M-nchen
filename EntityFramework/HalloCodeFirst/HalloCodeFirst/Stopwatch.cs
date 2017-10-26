using System;

namespace HalloCodeFirst
{
    internal class Stopwatch : IDisposable
    {
        private readonly Action<long> stopped;
        private readonly System.Diagnostics.Stopwatch stopwatch;

        public Stopwatch(Action<long> stopped)
        {
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            this.stopped = stopped;
        }

        public void Dispose()
        {
            stopwatch.Stop();
            stopped(stopwatch.ElapsedMilliseconds);
        }
    }
}
