using System;

namespace Logging
{
    public class Logger
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[{0}] [INFO] {1}", Date(), message);
            Console.ResetColor();
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[{0}] [WARN] {1}", Date(), message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[{0}] [ERROR] {1}", Date(), message);
            Console.ResetColor();
        }

        public static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[{0}] [DEBUG] {1}", Date(), message);
            Console.ResetColor();
        }

        public static string Date()
        {
            return DateTime.Now.ToString();
        }
    }
}