using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToolbox.Classes {
    public class Log {

        public static Log.Level level = Level.debug;

        public enum Level {
            error, info, warn, debug
        }

        private static ConsoleColor colorForLogLevel(Level level) {
            switch (level) {
                case Level.info: return ConsoleColor.White;
                case Level.warn: return ConsoleColor.Yellow;
                case Level.error: return ConsoleColor.Red;
                case Level.debug: return ConsoleColor.Cyan;
                default: return ConsoleColor.White;
            }
        }

        private static String nameForLevel(Level level) {
            switch (level) {
                case Level.info: return "information";
                case Level.warn: return "warning";
                case Level.error: return "error";
                case Level.debug: return "debug";
                default: return "";
            }
        }

        public static void debug(String message) {
            write(Level.debug, message);
        }

        public static void debugLine(String message) {
            writeLine(Level.debug, message);
        }

        public static void info(String message) {
            write(Level.info, message);
        }

        public static void infoLine(String message) {
            writeLine(Level.info, message);
        }

        public static void error(String message) {
            write(Level.error, message);
        }

        public static void errorLine(String message) {
            writeLine(Level.error, message);
        }

        public static void warn(String message) {
            write(Level.warn, message);
        }

        public static void warnLine(String message) {
            writeLine(Level.warn, message);
        }

        private static void write(Level level, String message) {
            if (level <= Log.level) {
                Console.ForegroundColor = colorForLogLevel(level);
                Console.Write(String.Format("[{0}] {1}", nameForLevel(level).ToUpper(), message));
                Console.ResetColor();
            }
        }

        private static void writeLine(Level level, String message) {
            if (level <= Log.level) {
                Console.ForegroundColor = colorForLogLevel(level);
                Console.WriteLine(String.Format("[{0}] {1}", nameForLevel(level).ToUpper(), message));
                Console.ResetColor();
            }
        }
    }
}
