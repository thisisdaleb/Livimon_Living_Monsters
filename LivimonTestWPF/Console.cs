using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace LivimonTestWPF
{
    //https://stackoverflow.com/questions/3670057/does-console-writeline-block
    public static class Console
    {
        private static BlockingCollection<string> logQueue = new BlockingCollection<string>();

        static Console()
        {
            var thread = new Thread(
              () =>
              {
                  while (true) Debug.WriteLine(logQueue.Take());
              });
            thread.IsBackground = true;
            thread.Start();
        }

        public static bool TIMESTAMPS = true;
        public static void log<T>(T value)
        {
            if(TIMESTAMPS) logQueue.Add("| " + DateTime.Now.ToString("hh:mm:ss.fff") + " | " + value);
            else logQueue.Add("" + value);
        }

        //eventually have all logs write to a file
    }
}
