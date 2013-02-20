using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POEApi.Infrastructure
{
    public static class Logger
    {
        private const string OUTPUT = "DebugInfo.log";
        public static void Log(Exception e)
        {
            Log(e.ToString());
        }
        public static void Log(string s)
        {
            File.AppendAllText(OUTPUT, s);
        }
    }
}
