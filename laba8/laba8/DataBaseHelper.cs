using Class_Journal;
using Input_Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Helper 
{
    internal class DataBaseHelper
    {
        private static string filePath = "students.bin";
        private static List<ClassJournal> records = new List<ClassJournal>();
        private static Random rand = new Random();

        public static void GenerateRandomData()
        {
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов", "Васильев", "Попов", "Соколов", "Лебедев", "Козлов" };
            string[] subjects = { "Математика", "Физика", "Химия", "Литература", "История", "Биология", "География", "Английский язык" };

            records = lastNames.Select(name => new ClassJournal(
                name,
                rand.Next(10, 18),
                subjects.OrderBy(x => rand.Next()).Take(rand.Next(1, 5)).ToList(),
                Math.Round(rand.NextDouble() * 3 + 2, 2),
                rand.Next(0, 2) == 1
            )).ToList();

            SaveToBin();
            Console.WriteLine("Случайные данные сгенерированы и сохранены.");
        }

        public static void SaveToBin()
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(records.Count);
                    foreach (ClassJournal r in records)
                    {
                        writer.Write(r.LastName);
                        writer.Write(r.Age);
                        writer.Write(r.ExamSubjects.Count);
                        foreach (string s in r.ExamSubjects)
                            writer.Write(s);
                        writer.Write(r.AverageGrade);
                        writer.Write(r.AreAnyDebts);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        public static void LoadFromBin()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден. Генерация новых данных...");
                GenerateRandomData();
                return;
            }
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int count = reader.ReadInt32();
                    records = new List<ClassJournal>();
                    for (int i = 0; i < count; i++)
                    {
                        string lastName = reader.ReadString();
                        int age = reader.ReadInt32();
                        int subjectCount = reader.ReadInt32();
                        List<string> subjects = new List<string>();
                        for (int j = 0; j < subjectCount; j++)
                        {
                            subjects.Add(reader.ReadString());
                        }
                        double averageGrade = reader.ReadDouble();
                        bool areAnyDebts = reader.ReadBoolean();
                        records.Add(new ClassJournal(lastName, age, subjects, averageGrade, areAnyDebts));
                    }
                }

                Console.WriteLine("База данных загружена из BIN-файла.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
            }
        }

        public static void ViewAll()
        {
            if (records.Count == 0)
            {
                Console.WriteLine("База данных пуста.");
                return;
            }
            foreach (ClassJournal r in records)
            {
                Console.WriteLine(r);
            }
        }

        public static void DeleteByLastName()
        {
            Console.Write("Введите фамилию для удаления: ");
            string? name = Console.ReadLine();
            List<ClassJournal> toRemove = (from journalRecord in records
                                           where journalRecord.LastName.Equals(name, StringComparison.OrdinalIgnoreCase)
                                           select journalRecord).ToList();
            foreach (ClassJournal recordToRemove in toRemove)
            {
                records.Remove(recordToRemove);
            }
            SaveToBin();
            if (toRemove.Count > 0)
            {
                Console.WriteLine($"{toRemove.Count} запись(ей) удалено.");
            }
            else
            {
                Console.WriteLine("Совпадений не найдено.");
            }
        }

        public static void AddRecord()
        {
            string? lastName;
            int age;
            List<string> examSubjects = new List<string>();
            double averageGrade;
            bool areAnyDebts;
            Console.Write("Фамилия: ");
            while (true)
            {
                lastName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(lastName))
                    break;
                Console.Write("Фамилия не может быть пустой или состоять из пробелов. Попробуйте снова: ");
            }

            age = InputValidation.InputNaturalWithValidation("Возраст (от 10 до 18): "); 
            while (age < 10 || age > 18)
            {
                Console.Write("Возраст должен быть числом от 10 до 18. Попробуйте снова: ");
                age = InputValidation.InputNaturalWithValidation("Возраст (от 10 до 18): "); 
            }

            string[] subjects = { "Математика", "Физика", "Химия", "Литература", "История", "Биология", "География", "Английский язык" };
            Console.WriteLine($"Доступные предметы: {string.Join(", ", subjects)}");
            while (true)
            {
                Console.Write("Введите предмет (или пустую строку для завершения): ");
                string? subject = Console.ReadLine();
                if (string.IsNullOrEmpty(subject))
                    break;
                if (subjects.Contains(subject))
                {
                    examSubjects.Add(subject);
                }
                else
                {
                    Console.WriteLine("Предмет не найден в списке доступных. Попробуйте снова.");
                }
            }

            averageGrade = InputValidation.InputDoubleWithValidation("Средний балл (от 2.0 до 5.0): ");
            while (averageGrade < 2.0 || averageGrade > 5.0)
            {
                Console.Write("Средний балл должен быть числом от 2.0 до 5.0. Попробуйте снова: ");
                averageGrade = InputValidation.InputDoubleWithValidation("Средний балл (от 2.0 до 5.0): ");
            }

            Console.Write("Есть ли задолженности (true/false): ");
            while (true)
            {
                string input = Console.ReadLine().ToLower();
                if (input == "true" || input == "false")
                {
                    areAnyDebts = input == "true";
                    break;
                }
                Console.Write("Введите true или false. Попробуйте снова: ");
            }
            ClassJournal newRecord = new ClassJournal(lastName, age, examSubjects, averageGrade, areAnyDebts);
            records.Add(newRecord);
            SaveToBin();
            Console.WriteLine("Запись добавлена.");
        }

        public static void Query1_List()
        {
            List<ClassJournal> result = (from student in records
                                         where student.AverageGrade > 4.5
                                         select student).ToList();
            Console.WriteLine("Ученики с оценкой выше 4.5:");
            result.ForEach(Console.WriteLine);
        }

        public static void Query2_List()
        {
            List<ClassJournal> result = (from student in records
                                         where student.ExamSubjects.Count > 2
                                         select student).ToList();
            Console.WriteLine("Ученики, сдающие более 2 предметов:");
            result.ForEach(Console.WriteLine);
        }

        public static void Query3_Value()
        {
            double averageAge = (from student in records 
                                 select student.Age).Average();
            Console.WriteLine($"Средний возраст: {averageAge:F2}");
        }

        public static void Query4_Value()
        {
            int debtorsCount = (from student in records
                                where student.AreAnyDebts == false 
                                select student).Count();
            Console.WriteLine($"Количество должников в классе: {debtorsCount}");
        }
    }
}
