using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работает Pascal-компилятор");
        string inputPath = "D:\\Влад\\пгниу\\языки программирования\\компилятор\\pascal_tests\\test114.txt";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Файл {inputPath} не найден.");
            return;
        }
        List<string> inputLines = new List<string>(File.ReadAllLines(inputPath));
        InputOutput io = new InputOutput(inputLines);
        LexicalAnalyzer lexer = new LexicalAnalyzer(io);
        lexer.Analyze();
        Console.WriteLine($"\nРаботает синтаксический анализ... ");
        Console.WriteLine($"\nПеречень синтаксических ошибок: ");

        var tokens = LexicalAnalyzer.symbolCodes;

        var analyzer = new SyntaxAnalyzer(tokens);
        analyzer.Analyze();
        if (SyntaxAnalyzer.ErrorCount == 0)
        {
            Console.WriteLine("Cинтаксических ошибок нет! ");
        }


        Console.WriteLine($"\nКомпиляция завершена: ошибок — {io.TotalErrors + SyntaxAnalyzer.ErrorCount}!");
    }
}