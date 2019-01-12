using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixelmon.Utils
{
    public static class ConsoleHelper
    {

        public static void WriteCenteredLine(string text)
        {
            // Recursive
            if (text.Contains('\n'))
            {
                foreach (string line in text.Split('\n'))
                    ConsoleHelper.WriteCenteredLine(line);
            }

            // Centers
            else
            {
                Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
                Console.WriteLine(text);
            }
        }

        public static int Read() => Console.Read();
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        public static void NewLine() => Console.WriteLine();
        public static void SetTile(string title) => Console.Title = title;
        public static void Pause(string message = null)
        {
            if (message != null)
                Colorful.Console.WriteLine(message, Color.DarkGray);
            Console.ReadKey(true);
            ConsoleHelper.ClearLastLine();
        }

        public static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

    }
}
