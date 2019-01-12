using System.Drawing;

namespace Pixelmon.Utils
{
    public class LogLevel
    {
        public Color Color { get; set; }
        public string Level { get; set; }

        public static LogLevel Debug
        {
            get { return new LogLevel() { Color = Color.Gray, Level = "Debug" };  }
        }

        public static LogLevel Ask
        {
            get { return new LogLevel() { Color = Color.LightSkyBlue, Level = "Question" }; }
        }

        public static LogLevel Info
        {
            get { return new LogLevel() { Color = Color.LightBlue, Level = "Info" }; }
        }

        public static LogLevel Warning
        {
            get { return new LogLevel() { Color = Color.DarkOrange, Level = "Attention" }; }
        }

        public static LogLevel Danger
        {
            get { return new LogLevel() { Color = Color.DarkRed, Level = "Erreur" }; }
        }
    }
}
