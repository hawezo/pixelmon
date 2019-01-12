using Colorful;
using System;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace Pixelmon.Utils
{
    public static class Logger
    {

        public static string LogFile { get; private set; }
        private static StreamWriter LogWriter { get; set; }

        /// <summary>
        /// Logs an entry into the console.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void Log(LogLevel level, string message)
        {
            Logger.LogToFile(level, message);

            if (level.Level == LogLevel.Debug.Level)
                return;

            string text = "{0} {1}{2}{3} {4} ";
            Formatter[] formats = new Formatter[]
            {
                new Formatter(DateTime.Now.ToShortTimeString(), Color.DarkGray),
                new Formatter("[", Color.DarkGray),
                new Formatter(level.Level, level.Color),
                new Formatter("]", Color.DarkGray),
                new Formatter(message, Color.WhiteSmoke),
            };
            Console.WriteLineFormatted(text, Color.WhiteSmoke, formats);
        }

        /// <summary>
        /// Logs an information message into the console.
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            Logger.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs to file.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private static void LogToFile(LogLevel level, string message)
        {
            if (Logger.LogFile == null)
                Logger.LogFile = $"log-{DateTime.Now.ToString("HHmm-MMddyy")}.log";

            if (Logger.LogWriter == null)
                Logger.LogWriter = new StreamWriter(Logger.LogFile);

            try
            {
                Logger.LogWriter.WriteLine(
                    $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} [{level.Level.ToString()}] {message}");
                Logger.LogWriter.Flush();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Ends logging.
        /// </summary>
        public static void EndLogging()
        {
            if (Logger.LogWriter != null)
            {
                Logger.LogWriter.Close();
                Logger.LogWriter.Dispose();
            }
        }

    }
}
