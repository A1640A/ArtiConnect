using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public enum LogLevel
    {
        Information = 1,
        Error = 1,
        Warning = 2
    }

    public static class Logging
    {
        private static readonly HashSet<string> HeaderWhitelist = new HashSet<string> { "Content-Type", "Content-Length", "User-Agent" };

        private const string HttpTrafficTemp = "{App} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        public static async Task WriteLog(LogLevel level, string message, string MethodName, string deviceId = "", Exception ex = null, string AppName = "")
        {
            //Log.Logger = new LoggerConfiguration().WriteTo.Seq("http://seq.sambapos.com/", LogEventLevel.Verbose, 1000, null, null, null, null, 262144L).CreateLogger();
            //double elapsedMilliseconds = GetElapsedMilliseconds(Stopwatch.GetTimestamp(), Stopwatch.GetTimestamp());
            //if (level == LogLevel.Error)
            //{
            //	Log.Error(ex, "{App} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms", AppName, message, MethodName, deviceId, 500, elapsedMilliseconds);
            //	Log.CloseAndFlush();
            //}
            //else
            //{
            //	Log.Information("{App} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms", AppName, message, MethodName, deviceId, 200, elapsedMilliseconds);
            //	Log.CloseAndFlush();
            //}
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (double)((stop - start) * 1000) / (double)Stopwatch.Frequency;
        }
    }
}
