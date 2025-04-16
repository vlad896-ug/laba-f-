using Input_Validation;
using Solution_Methods;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Files_and_Collections 
{
    public struct Toy
    {
        private string name;
        private double cost;
        private int ageFrom;
        private int ageTo;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public int AgeFrom
        {
            get { return ageFrom; }
            set { ageFrom = value; }
        }

        public int AgeTo
        {
            get { return ageTo; }
            set { ageTo = value; }
        }
        public Toy(string name, double cost, int ageFrom, int ageTo)
        {
            this.name = name;
            this.cost = cost;
            this.ageFrom = ageFrom;
            this.ageTo = ageTo;
        }
    }
    class Program
    {
        static void Main()
        {
            //1
            string filePath = InputValidation.InputOfTextFileWithValidation("1)Введите путь к файлу: ");
            int quantity = InputValidation.InputNaturalWithValidation("Введите количество чисел для записи в файл: ");
            var randomNumbers = InputValidation.InputRangeWithValidation("Введите нижнюю границу случайных чисел: ", "Введите верхнюю границу случайных чисел: ");
            int min = randomNumbers.min;
            int max = randomNumbers.max;
            SolutionMethods.FileWithRandomData(filePath, quantity, min, max);
            int sum = SolutionMethods.SumOfElementsToIndex(filePath);
            Console.WriteLine($"Сумма элементов, равных их индексу: {sum}");
            //2
            string filePath2 = InputValidation.InputOfTextFileWithValidation("2)Введите путь к файлу: ");
            int rows = InputValidation.InputNaturalWithValidation("Введите количество строк для записи в файл: ");
            int cols = InputValidation.InputNaturalWithValidation("Введите количество чисел в строке: ");
            var randomNumbers2 = InputValidation.InputRangeWithValidation("Введите нижнюю границу случайных чисел: ", "Введите верхнюю границу случайных чисел: ");
            int min2 = randomNumbers2.min;
            int max2 = randomNumbers2.max;
            SolutionMethods.FileWithRandomData2(filePath2, rows, cols, min2, max2);
            int k1 = InputValidation.InputIntegerWithValidation("Введите число k: ");
            long product = SolutionMethods.ComputeProductOfMultiples(filePath2, k1);
            Console.WriteLine($"Произведение элементов, кратных {k1}: {product}");
            //3
            string filePath3_1 = InputValidation.InputOfTextFileWithValidation("3)Введите путь к исходному файлу: ");
            string filePath3_2 = InputValidation.InputOfTextFileWithValidation("Введите путь к файлу для записи результатов: ");
            int rows3 = InputValidation.InputNaturalWithValidation("Введите количество строк для записи в исходный файл: ");
            var wordRange = InputValidation.InputRangeWithValidation( "Введите минимальное количество слов в строке: ", "Введите максимальное количество слов в строке: ");
            int minWords = wordRange.min;
            int maxWords = wordRange.max;
            var charRange = InputValidation.InputRangeWithValidation( "Введите минимальную длину слова: ", "Введите максимальную длину слова: ");
            int minChars = charRange.min;
            int maxChars = charRange.max;
            SolutionMethods.FileWithRandomText(filePath3_1, rows3, minWords, maxWords, minChars, maxChars);
            SolutionMethods.WriteLinesWithoutRussianLetters(filePath3_1, filePath3_2);
            Console.WriteLine($"В файл {filePath3_2} перезаписаны строки без русских букв");
            //4
            string filePath4_1 = InputValidation.InputOfBinaryFileWithValidation("4)Введите путь к исходному файлу: ");
            int quantity4 = InputValidation.InputNaturalWithValidation("Введите количество чисел для записи в файл: ");
            var randomNumbers4 = InputValidation.InputRangeWithValidation("Введите нижнюю границу случайных чисел: ", "Введите верхнюю границу случайных чисел: ");
            int min4 = randomNumbers4.min;
            int max4 = randomNumbers4.max;
            SolutionMethods.BinaryFileWithRandomData(filePath4_1, quantity4, min4, max4);
            SolutionMethods.PrintBinaryFile(filePath4_1);
            string filePath4_2 = InputValidation.InputOfBinaryFileWithValidation("Введите путь к файлу последовательного доступа: ");
            int k2 = InputValidation.InputIntegerWithValidation("Введите число k: ");
            SolutionMethods.CopyToAnotherFile(filePath4_1, filePath4_2, k2);
            SolutionMethods.PrintBinaryFile(filePath4_2);
            //5
            string xmlFilePath = InputValidation.InputOfBinaryFileWithValidation("5)Введите путь к XML-файлу (для сериализации): ");
            string binaryFilePath = InputValidation.InputOfBinaryFileWithValidation("Введите путь к бинарному файлу: ");
            int toyCount = InputValidation.InputNaturalWithValidation("Введите количество игрушек: ");
            SolutionMethods.CreateAndFillFiles(xmlFilePath, binaryFilePath, toyCount);
            double maxCost = SolutionMethods.FindMostExpensiveConstructor(binaryFilePath);
            if (maxCost == -1)
            {
                Console.WriteLine("В файле нет игрушек с названием 'Конструктор'.");
            }
            else
            {
                Console.WriteLine($"Стоимость самого дорогого конструктора: {maxCost} руб.");
            }
            //6
            int quantity6 = InputValidation.InputNaturalWithValidation("6)Введите количество элементов списка: ");
            var list = SolutionMethods.FillList<string>(quantity6); 
            SolutionMethods.DisplayList(list);
            bool hasDuplicates = SolutionMethods.HasDuplicates(list);
            if (hasDuplicates)
            {
                Console.WriteLine("В списке есть хотя бы два одинаковых элемента.");
            }
            else
            {
                Console.WriteLine("В списке нет одинаковых элементов.");
            }
            //7
            int quantity7 = InputValidation.InputNaturalWithValidation("7)Введите количество элементов списка: ");
            var linkedList = SolutionMethods.FillLinkedList<string>(quantity7); 
            Console.WriteLine("Вывод списка в прямом направлении:");
            SolutionMethods.DisplayLinkedListFirst(linkedList);
            Console.Write("Введите элемент, который нужно удалить: ");
            string? elementToRemove = Console.ReadLine();
            if (elementToRemove != null && linkedList.Remove(elementToRemove))
            {
                Console.WriteLine($"Элемент \"{elementToRemove}\" удалён.");
            }
            else
            {
                Console.WriteLine("Элемент не найден, удаление не выполнено.");
            }
            Console.WriteLine("Вывод списка в прямом направлении после удаления:");
            SolutionMethods.DisplayLinkedListFirst(linkedList);
            //8
            int totalTracks = InputValidation.InputNaturalWithValidation("8)Введите количество треков: ");
            HashSet<string> allTracks = SolutionMethods.FillTracks(totalTracks);
            int numberOfMusicLovers = InputValidation.InputNaturalWithValidation("Введите количество меломанов: ");
            List<HashSet<string>> musicLovers = SolutionMethods.GetMusicLoversPreferences(numberOfMusicLovers, allTracks);
            SolutionMethods.AnalyzeMusicPreferences(musicLovers, allTracks);
            //9
            string filePath9 = InputValidation.InputOfTextFileWithValidation("9)Введите путь к файлу: ");
            int rows9 = InputValidation.InputNaturalWithValidation("Введите количество строк для записи в исходный файл: ");
            var wordRange9 = InputValidation.InputRangeWithValidation("Введите минимальное количество слов в строке: ", "Введите максимальное количество слов в строке: ");
            int minWords9 = wordRange9.min;
            int maxWords9 = wordRange9.max;
            var charRange9 = InputValidation.InputRangeWithValidation("Введите минимальную длину слова: ", "Введите максимальную длину слова: ");
            int minChars9 = charRange9.min;
            int maxChars9 = charRange9.max;
            SolutionMethods.FileWithRandomRussianText(filePath9, rows9, minWords9, maxWords9, minChars9, maxChars9);
            string text = File.ReadAllText(filePath9);
            HashSet<char> result = SolutionMethods.FindUniqueVowels(text);
            char[] correctAlphabetOrder = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
            List<char> sortedVowels = new List<char>(result);
            sortedVowels.Sort((x, y) =>
            {
                int ix = Array.IndexOf(correctAlphabetOrder, x);
                int iy = Array.IndexOf(correctAlphabetOrder, y);
                return ix.CompareTo(iy);
            });
            Console.WriteLine("Гласные, входящие не более чем в одно слово (в алфавитном порядке):");
            StringBuilder sb = new StringBuilder(); 
            foreach (char v in sortedVowels)
            {
                sb.Append(v).Append(" "); 
            }
            Console.Write(sb.ToString());
            Console.WriteLine();
            //10
            string filePath10 = InputValidation.InputOfTextFileWithValidation("10)Введите путь к файлу: ");
            string[] lines = File.ReadAllLines(filePath10);
            if (lines.Length == 0)
            {
                Console.WriteLine("Файл пуст.");
                return;
            }

            int numberOfParty;
            if (!int.TryParse(lines[0], out numberOfParty) ||  numberOfParty <= 0 || lines.Length < numberOfParty + 1)
            {
                Console.WriteLine("Некорректный формат данных.");
                return;
            }

            List<string> names = new List<string>();
            List<int> scores = new List<int>();

            SolutionMethods.ProcessInput(lines, numberOfParty, names, scores);
            SolutionMethods.FindBestNonWinner(names, scores);
        }
    }
}
    
