// <copyright file="ConsoleExtensions.cs" company="tagItron GmbH">
// Copyright (c) tagItron GmbH. All rights reserved.
// </copyright>

namespace AOC2024
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// TODO: Add class summary.
    /// </summary>
    public static class ConsoleExtensions
    {
        public static void WriteLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (color != ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (color != ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
            }
            else
            {
                Console.ResetColor();
            }

            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteLine(char message, ConsoleColor color)
        {
            WriteLine($"{message}", color);
        }

        public static void Write(char message, ConsoleColor color)
        {
            Write($"{message}", color);
        }

        public static void WriteLine(int message, ConsoleColor color)
        {
            WriteLine($"{message}", color);
        }

        public static void Write(int message, ConsoleColor color)
        {
            Write($"{message}", color);
        }
    }
}
