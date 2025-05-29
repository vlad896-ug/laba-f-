using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работает Pascal-компилятор");
        string inputPath = "D:\\Влад\\пгниу\\языки программирования\\компилятор\\pascal_tests\\test4.txt";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Файл {inputPath} не найден.");
            return;
        }
        List<string> inputLines = new List<string>(File.ReadAllLines(inputPath));
        InputOutput io = new InputOutput(inputLines);
        LexicalAnalyzer lexer = new LexicalAnalyzer(io);
        lexer.Analyze();
        Console.WriteLine($"\nКомпиляция завершена: ошибок — {io.TotalErrors}!");
    }
}





