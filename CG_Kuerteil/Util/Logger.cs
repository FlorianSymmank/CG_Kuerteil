namespace CG_Kuerteil.Util
{
    public class Logger
    {
        public enum LogLevel
        {
            Info,
            Warn,
            Error,
            Fatal
        }
        public static void Log(string text) => writeLog(LogLevel.Info, text);
        public static void Log(LogLevel logLevel, string text) => writeLog(logLevel, text);
        private static void writeLog(LogLevel logLevel, string text)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine($"Date: {DateTime.Now} | LogLevel: {logLevel} | Message: {text}");
            Console.ResetColor();
        }
    }
}