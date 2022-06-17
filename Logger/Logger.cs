//-----------------------------------------------------------------------------
// <copyright file="Logger.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для логирования данных.
// в формат PDF
// </summary>
//-----------------------------------------------------------------------------

using System;
using System.IO;

using static System.Console;

namespace Usl
{
    /// <summary>
    /// Вариант записи логов.
    /// </summary>
    public enum WriteTo
    {
        File,
        Console
    }

    /// <summary>
    /// Уровни логирования.
    /// </summary>
    public enum LogLevel
    {
        Trace   = 0,
        Info    = 1,
        Warn    = 2,
        Error   = 3,
        Fatal   = 4,
        Off     = 5
    }

    /// <summary>
    /// Класс логирования.
    /// </summary>
    public class Logger
    {
        private static WriteTo _writeTo = WriteTo.Console;
        private static StreamWriter _file;

        /// <summary>
        /// Текущий уровень логирования.
        /// </summary>
        public static LogLevel Level { get; set; } = LogLevel.Trace;

        /// <summary>
        /// Перенаправить вывод логов в файл.
        /// </summary>
        /// <param name="name">Имя файла</param>
        public static void WriteToFile(string name)
        {
            _writeTo = WriteTo.File;
            _file = new StreamWriter(name, false);
        }

        /// <summary>
        /// Закрыть и сохранить файл.
        /// </summary>
        /// <remarks>
        /// Вызывать в конце программы. После вызова нельзя выводить логи.
        /// </remarks>
        public static void SaveFile()
        {
            _file?.Close();
        }

        /// <summary>
        /// Логирование сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Trace(string message)
        {
            if (Level > LogLevel.Trace)
                return;

            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.Green;
                Write("[TRACE] ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[TRACE] ");
            _file.WriteLine(message);
        }

        /// <summary>
        /// Логирование сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        public static void Trace(string message, params object[] args)
            => Trace(string.Format(message, args));

        /// <summary>
        /// Логирование отладочной информации.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <remarks>
        /// Выводит сообщение только в режиме отладки.
        /// </remarks>
        public static void Debug(string message)
        {
#if DEBUG
            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.Magenta;
                Write("[DEBUG] ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[DEBUG] ");
            _file.WriteLine(message);
#endif
        }

        /// <summary>
        /// Логирование отладочной информации.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        /// <remarks>
        /// Выводит сообщение только в режиме отладки.
        /// </remarks>
        public static void Debug(string message, params object[] args)
            => Debug(string.Format(message, args));

        /// <summary>
        /// Логирование информационного сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Info(string message)
        {
            if (Level > LogLevel.Info)
                return;

            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.Blue;
                Write("[INFO]  ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[INFO]  ");
            _file.WriteLine(message);
        }

        /// <summary>
        /// Логирование информационного сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        public static void Info(string message, params object[] args)
            => Info(string.Format(message, args));

        /// <summary>
        /// Логирование предупреждающего сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Warn(string message)
        {
            if (Level > LogLevel.Warn)
                return;

            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.DarkYellow;
                Write("[WARN]  ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[WARN]  ");
            _file.WriteLine(message);
        }

        /// <summary>
        /// Логирование предупреждающего сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        public static void Warn(string message, params object[] args)
            => Warn(string.Format(message, args));

        /// <summary>
        /// Логирование сообщения об ошибке.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Error(string message)
        {
            if (Level > LogLevel.Error)
                return;

            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.Red;
                Write("[ERROR] ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[ERROR] ");
            _file.WriteLine(message);
        }

        /// <summary>
        /// Логирование сообщения об ошибке.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        public static void Error(string message, params object[] args)
            => Fatal(string.Format(message, args));

        /// <summary>
        /// Логирование сообщения о критической ошибке.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Fatal(string message)
        {
            if (Level > LogLevel.Fatal)
                return;

            if (_writeTo == WriteTo.Console) {
                ForegroundColor = ConsoleColor.DarkRed;
                Write("[FATAL] ");
                ResetColor();
                WriteLine(message);
                return;
            }
            _file.Write("[FATAL] ");
            _file.WriteLine(message);
        }

        /// <summary>
        /// Логирование сообщения о критической ошибке.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Аргументы</param>
        public static void Fatal(string message, params object[] args)
            => Fatal(string.Format(message, args));
    }
}