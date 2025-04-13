using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Input_Validation 
{
    internal class InputValidation
    {
        public static int InputIntegerWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пустой ввод. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (input == "0")
                {
                    return 0;
                }
                if (int.TryParse(input, out int inputNumber) && !input.StartsWith('0') && !(input.StartsWith("-0")))
                {
                    return inputNumber;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный формат числа. Повторите ввод.");
                Console.ResetColor();
            }
        }

        public static double InputDoubleWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пустой ввод. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (input == "0" || input == "0,0" ) 
                {
                    return 0.0;
                }
                if (double.TryParse(input, out double inputNumber))
                {
                    if (input.StartsWith("0") && !input.StartsWith("0,")) 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Недопустимый ввод. Число не может начинаться с нуля, если оно не равно нулю (за исключением 0 и 0,x).");
                        Console.ResetColor();
                        continue;
                    }
                    if ((input.StartsWith("-0") && !input.StartsWith("-0,")) || input == "-0")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Недопустимый ввод. Число не может начинаться с нуля, если оно не равно нулю (за исключением -0,x).");
                        Console.ResetColor();
                        continue;
                    }
                    return inputNumber;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный формат числа. Повторите ввод.");
                Console.ResetColor();
            }
        }

        public static int InputNaturalWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пустой ввод. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (int.TryParse(input, out int inputNumber) && inputNumber > 0 && !input.StartsWith('0'))
                {
                    return inputNumber;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный формат натурального числа. Повторите ввод.");
                Console.ResetColor();
            }
        }

        public static string InputOfTextFileWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Путь к файлу не может быть пустым. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (Path.GetExtension(input).ToLower() != ".txt")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Файл должен иметь расширение .txt. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                return input;
            }
        }

        public static string InputOfBinaryFileWithValidation(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Путь к файлу не может быть пустым. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                if (Path.GetExtension(input).ToLower() != ".bin")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Файл должен иметь расширение .bin. Повторите ввод.");
                    Console.ResetColor();
                    continue;
                }
                return input;
            }
        }

        public static (int min, int max) InputRangeWithValidation(string promptMin, string promptMax)
        {
            int min, max;
            while (true)
            {
                min = InputNaturalWithValidation(promptMin);
                max = InputNaturalWithValidation(promptMax);
                if (max >= min)
                {
                    return (min, max);
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Максимальное значение должно быть больше или равно минимальному. Повторите ввод.");
                Console.ResetColor();
            }
        }
    }
}
