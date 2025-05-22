using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работает Pascal-компилятор");
        string inputPath = "D:\\Влад\\пгниу\\языки программирования\\компилятор\\pascal_tests\\error5.txt";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Файл {inputPath} не найден.");
            return;
        }
        List<string> inputLines = new List<string>(File.ReadAllLines(inputPath));
        InputOutput io = new InputOutput(inputLines);
        if (inputLines.Count == 0 || !inputLines[0].StartsWith("program", StringComparison.OrdinalIgnoreCase))
        {
            io.Error(3, new TextPosition(1, 1));
        }
        bool lineHasUnclosedParen = false;
        int parenLine = 0;
        int openCount = 0;
        int closeCount = 0;
        char prevChar = '\0';

        while (io.CurrentChar != '\0')
        {
            char ch = io.CurrentChar;

            if (ch == '$')
            {
                io.Error(10, io.CurrentPosition);
            }

            if (prevChar == '.' && ch == '.')
            {
                io.Error(7, io.CurrentPosition);
            }
            prevChar = ch;

            if (ch == '(')
            {
                openCount++;
                parenLine = io.CurrentPosition.LineNumber;
                lineHasUnclosedParen = true;
            }

            if (ch == ')')
            {
                closeCount++;
                lineHasUnclosedParen = false; 
            }

            if (io.CurrentPosition.CharNumber == io.CurrentLineLength)
            {
                string line = io.CurrentLine;
                if (line.Contains("if", StringComparison.OrdinalIgnoreCase) &&
                    !line.Contains("then", StringComparison.OrdinalIgnoreCase))
                {
                    int pos = line.Length + 1;
                    int lineNumber = io.CurrentPosition.LineNumber;
                    io.Error(52, new TextPosition(lineNumber, pos));
                }
                if (lineHasUnclosedParen && openCount > closeCount)
                {
                    io.Error(4, new TextPosition(parenLine, io.CurrentLineLength + 1));
                    lineHasUnclosedParen = false;
                }
                openCount = 0;
                closeCount = 0;
            }
            io.NextChar();
        }

        Console.WriteLine("\nКомпиляция завершена.\n");
    }
}
