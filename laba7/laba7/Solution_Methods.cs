using Files_and_Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Solution_Methods
{
    internal class SolutionMethods 
    {
        public static void FileWithRandomData(string filePath, int quantity, int min, int max)
        {
            Random rand = new Random();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < quantity; i++)
                {
                    int value = rand.Next(min, max + 1);
                    writer.WriteLine(value);
                }
            }
        }

        public static int SumOfElementsToIndex(string filePath)
        {
            int sum = 0;
            int index = 0;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                do
                {
                    line = reader.ReadLine();
                    if (line != null && int.TryParse(line, out int value) && value == index)
                    {
                        sum += value;
                    }
                    index++;
                } while (line != null);
            }
            return sum;
        }

        public static void FileWithRandomData2(string filePath, int rows, int cols, int min, int max)
        {
            Random rand = new Random();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++) 
                {
                    StringBuilder sb = new StringBuilder(); 
                    for (int j = 0; j < cols; j++) 
                    {
                        sb.Append(rand.Next(min, max + 1));
                        if (j < cols - 1)
                        {
                            sb.Append(' ');
                        }
                    }
                    writer.WriteLine(sb.ToString()); 
                }
            }
        }

        public static long ComputeProductOfMultiples(string filePath, int k)
        {
            long product = 1;
            bool foundAny = false;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++)
                    {
                        string part = parts[i];
                        if (int.TryParse(part, out int number))
                        {
                            if (number % k == 0)
                            {
                                product *= number;
                                foundAny = true;
                            }
                        }
                    }
                }
            }
            if (!foundAny)
            {
                Console.WriteLine("В файле нет чисел, кратных заданному k.");
            }
            return product;
        }

        public static void FileWithRandomText(string filePath, int rows, int minWords, int maxWords, int minChars, int maxChars)
        {
            Random rand = new Random();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++)
                {
                    int wordsCount = rand.Next(minWords, maxWords + 1); 
                    for (int w = 0; w < wordsCount; w++)
                    {
                        int wordLength = rand.Next(minChars, maxChars + 1); 
                        for (int j = 0; j < wordLength; j++)
                        {
                            char c;
                            if (rand.NextDouble() < 0.5)
                            {
                                c = (char)rand.Next(' ', '~' + 1); 
                            }
                            else
                            {
                                c = (char)rand.Next('А', 'я' + 1);
                            }
                            writer.Write(c);
                        }
                        writer.Write(" "); 
                    }
                    writer.WriteLine();
                }
            }
        }

        public static void WriteLinesWithoutRussianLetters(string inputFilePath, string outputFilePath)
        {
            using (StreamReader reader = new StreamReader(inputFilePath))
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    bool hasRussianLetters = false;
                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];
                        if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || c == 'ё' || c == 'Ё')
                        {
                            hasRussianLetters = true;
                            break;
                        }
                    }
                    if (!hasRussianLetters)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public static void PrintBinaryFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                Console.WriteLine("Содержимое файла:");
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int number = reader.ReadInt32();
                    Console.Write($"{number} ");
                }
                Console.WriteLine();
            }
        }

        public static void BinaryFileWithRandomData(string filePath, int quantity, int min, int max)
        {
            Random rand = new Random();
            using (BinaryWriter file = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
            {
                for (int i = 0; i < quantity; i++)
                {
                    int value = rand.Next(min, max + 1);
                    file.Write(value);
                }
            }
        }

        public static void CopyToAnotherFile(string sourceFilePath, string destFilePath, int k)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(sourceFilePath, FileMode.Open)))
            using (BinaryWriter writer = new BinaryWriter(File.Open(destFilePath, FileMode.Create)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int number = reader.ReadInt32();
                    if (number % k == 0)
                    {
                        writer.Write(number);
                    }
                }
            }
        }

        public static void CreateAndFillFiles(string xmlFilePath, string binaryFilePath, int toyCount)
        {
            string[] possibleNames = { "Конструктор", "Мяч", "Кукла", "Машинка", "Робот" };
            Random rand = new Random();
            Toy[] toys = new Toy[toyCount];
            for (int i = 0; i < toyCount; i++)
            {
                string randomName = possibleNames[rand.Next(possibleNames.Length)];
                double randomCost = rand.Next(100, 1001);
                int ageFrom = rand.Next(1, 10);
                int ageTo = rand.Next(ageFrom + 1, 15);
                toys[i] = new Toy(randomName, randomCost, ageFrom, ageTo);
                toys[i].Name = randomName;
                toys[i].Cost = randomCost;
                toys[i].AgeFrom = ageFrom;
                toys[i].AgeTo = ageTo;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Toy[]));
            using (FileStream fsXml = new FileStream(xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fsXml, toys);
            }
            Console.WriteLine($"\nСгенерировано {toyCount} игрушек, записаны в:\nXML: {xmlFilePath}\nBIN: {binaryFilePath}\n");
        }

        public static Toy[] DeserializeFilesss(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Toy[]));
            using (FileStream fsXml = new FileStream(xmlFilePath, FileMode.Create))
            {
                return (Toy[])serializer.Deserialize(fsXml);
            }
        }



        public static double FindMostExpensiveConstructor(string binaryFilePath)
        {
            double maxConstructorCost = -1;

            using (FileStream fs = new FileStream(binaryFilePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int toyCount = reader.ReadInt32();
                Console.WriteLine("Содержимое бинарного файла (Name, Cost, AgeFrom, AgeTo):");
                for (int i = 0; i < toyCount; i++)
                {
                    string name = reader.ReadString();
                    double cost = reader.ReadDouble();
                    int ageFrom = reader.ReadInt32();
                    int ageTo = reader.ReadInt32();
                    Console.WriteLine($"{name}, {cost} руб., {ageFrom}–{ageTo} лет");
                    if (name == "Конструктор")
                    {
                        if (cost > maxConstructorCost)
                        {
                            maxConstructorCost = cost;
                        }
                    }
                }
            }
            return maxConstructorCost;
        }

        public static List<T> FillList<T>(int quantity)
        {
            List<T> list = new List<T>(quantity);
            Console.WriteLine("Введите элементы списка:");
            for (int i = 0; i < quantity; i++)
            {
                Console.Write($"Элемент {i + 1}: ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    input = "";
                }
                T element = (T)Convert.ChangeType(input, typeof(T));
                list.Add(element);
            }
            return list;
        }

        public static void DisplayList<T>(List<T> list)
        {
            Console.Write("[");

            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i].ToString());
                if (i < list.Count - 1)
                {
                    Console.Write(", ");
                }
            }

            Console.Write("]");
            Console.WriteLine();
        }

        public static bool HasDuplicates<T>(List<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(list[i], list[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static LinkedList<T> FillLinkedList<T>(int quantity)
        {
            LinkedList<T> list = new LinkedList<T>();
            Console.WriteLine("Введите элементы списка:");
            for (int i = 0; i < quantity; i++)
            {
                Console.Write($"Элемент {i + 1}: ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    input = "";
                }
                T element = (T)Convert.ChangeType(input, typeof(T)); 
                list.AddLast(element);
            }
            return list;
        }

        public static void DisplayLinkedListFirst<T>(LinkedList<T> list)
        {
            Console.Write("[");
            LinkedListNode<T>? current = list.First;
            while (current != null)
            {
                Console.Write(current.Value);
                if (current.Next != null)
                {
                    Console.Write(", ");
                }
                current = current.Next;
            }
            Console.WriteLine("]");
        }

        public static HashSet<string> FillTracks(int totalTracks)
        {
            HashSet<string> allTracks = new HashSet<string>();
            Console.WriteLine("Введите названия доступных треков (по одному в строке):");
            for (int i = 0; i < totalTracks; i++)
            {
                while (true)
                {
                    Console.Write($"Трек {i + 1}: ");
                    string? track = Console.ReadLine();
                    if (string.IsNullOrEmpty(track))
                    {
                        track = $"Неизвестный_{i + 1}";
                    }
                    if (allTracks.Contains(track))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка: Трек \"{track}\" уже существует. Введите другое название.");
                        Console.ResetColor();
                    }
                    else
                    {
                        allTracks.Add(track);
                        break;
                    }
                }
            }
            return allTracks;
        }

        public static List<HashSet<string>> GetMusicLoversPreferences(int numberOfMusicLovers, HashSet<string> allTracks)
        {
            List<HashSet<string>> musicLovers = new List<HashSet<string>>();
            for (int i = 0; i < numberOfMusicLovers; i++)
            {
                Console.WriteLine($"\nВведите треки, которые нравятся меломану {i + 1} (через запятую):");
                string? input = Console.ReadLine();
                HashSet<string> userTracks = new HashSet<string>();
                if (!string.IsNullOrEmpty(input))
                {
                    string[] splitted = input.Split(',');
                    for (int j = 0; j < splitted.Length; j++)
                    {
                        string trackName = splitted[j].Trim();
                        if (allTracks.Contains(trackName))
                        {
                            userTracks.Add(trackName);
                        }
                        else
                        {
                            Console.WriteLine($"Предупреждение: трек \"{trackName}\" нет в общем списке и не будет учтён.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ввод пуст, у меломана нет любимых треков или допущен пропуск ввода.");
                }
                musicLovers.Add(userTracks);
            }
            return musicLovers;
        }

        public static void AnalyzeMusicPreferences(List<HashSet<string>> musicLovers, HashSet<string> allTracks)
        {
            // Треки, которые нравятся всем
            HashSet<string> universallyLiked = new HashSet<string>(musicLovers[0]);
            for (int i = 1; i < musicLovers.Count; i++)
            {
                universallyLiked.IntersectWith(musicLovers[i]);
            }
            // Треки, которые нравятся некоторым
            HashSet<string> likedBySomeone = new HashSet<string>();
            foreach (var lover in musicLovers)
            {
                likedBySomeone.UnionWith(lover);
            }
            // Треки, которые не нравятся никому
            HashSet<string> notLiked = new HashSet<string>(allTracks);
            foreach (HashSet<string> musicSet in musicLovers)
            {
                notLiked.ExceptWith(musicSet);
            }
            // Вывод результатов
            Console.WriteLine("\nРезультаты анализа музыкальных предпочтений:");
            Console.WriteLine("\nТреки, которые нравятся всем меломанам:");
            PrintSet(universallyLiked);
            Console.WriteLine("\nТреки, которые нравятся некоторым меломанам:");
            PrintSet(likedBySomeone);
            Console.WriteLine("\nТреки, которые не нравятся никому:");
            PrintSet(notLiked);
        }

        public static void PrintSet(HashSet<string> set)
        {
            if (set.Count == 0)
            {
                Console.WriteLine("Нет треков");
                return;
            }
            foreach (string item in set)
            {
                Console.WriteLine(item);
            }
        }

        public static void FileWithRandomRussianText(string filePath, int rows, int minWords, int maxWords, int minChars, int maxChars)
        {
            Random rand = new Random();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++)
                {
                    int wordsCount = rand.Next(minWords, maxWords + 1); 
                    for (int w = 0; w < wordsCount; w++)
                    {
                        int wordLength = rand.Next(minChars, maxChars + 1); 
                        for (int j = 0; j < wordLength; j++)
                        {
                            char c = (char)rand.Next('А', 'я' + 1);
                            writer.Write(c);
                        }
                        writer.Write(" "); 
                    }
                    writer.WriteLine();
                }
            }
        }

        public static HashSet<char> FindUniqueVowels(string text)
        {
            char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
            HashSet<char> vowelsInMultipleWords = new HashSet<char>();
            HashSet<char> vowelsInCurrentWord = new HashSet<char>();
            HashSet<char> result = new HashSet<char>();
            string[] words = text.ToLower().Split(new char[] { ' ', '\n', '\r', '\t', ',', '.', '!', '?', ':', ';', '\"', '(', ')', '-', '—' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                vowelsInCurrentWord.Clear(); 
                foreach (char c in word)
                {
                    if (new string(vowels).Contains(c))
                    {
                        vowelsInCurrentWord.Add(c);
                    }
                }
                foreach (char v in vowelsInCurrentWord)
                {
                    if (vowelsInMultipleWords.Contains(v))
                    {
                        continue; 
                    }
                    if (result.Contains(v))
                    {
                        vowelsInMultipleWords.Add(v);
                        result.Remove(v); 
                    }
                    else
                    {
                        result.Add(v); 
                    }
                }
            }
            foreach (char v in vowels)
            {
                if (!result.Contains(v) && !vowelsInMultipleWords.Contains(v))
                {
                    result.Add(v); 
                }
            }
            return result;
        }

        public static void ProcessInput(string[] lines, int numberOfParty, List<string> names, List<int> scores)
        {
            for (int i = 1; i <= numberOfParty; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length != 4)
                {
                    continue;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(parts[0]).Append(" ").Append(parts[1]);
                string fullName = sb.ToString();
                int score = int.Parse(parts[3]);
                names.Add(fullName);
                scores.Add(score);
            }
        }

        public static void FindBestNonWinner(List<string> names, List<int> scores)
        {
            int n = scores.Count;
            SortedList<int, List<int>> sortedScores = new SortedList<int, List<int>>();
            for (int i = 0; i < n; i++)
            {
                int score = scores[i];
                if (!sortedScores.ContainsKey(score))
                {
                    sortedScores[score] = new List<int>();
                }
                sortedScores[score].Add(i);
            }
            int maxScore = sortedScores.Keys[sortedScores.Count - 1];
            List<int> topIndexes = sortedScores[maxScore];
            bool isWinner = maxScore > 200 && (topIndexes.Count * 100 <= n * 20);
            int secondMax = (sortedScores.Count > 1) ? sortedScores.Keys[sortedScores.Count - 2] : int.MinValue;
            List<int> resultIndexes = new List<int>();
            if (isWinner)
            {
                resultIndexes = sortedScores.ContainsKey(secondMax) ? sortedScores[secondMax] : new List<int>();
            }
            else
            {
                resultIndexes = topIndexes;
            }
            if (resultIndexes.Count == 1)
            {
                Console.WriteLine(names[resultIndexes[0]]);
            }
            else
            {
                Console.WriteLine(resultIndexes.Count);
            }
        }
    }
}
