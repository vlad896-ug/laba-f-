using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DataBase_Helper;
using Class_Journal;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== Журнал класса ===");
            Console.WriteLine("1. Чтение базы данных из бинарного файла");
            Console.WriteLine("2. Просмотр базы данных");
            Console.WriteLine("3. Удалить запись по фамилии");
            Console.WriteLine("4. Добавить новую запись");
            Console.WriteLine("5. Запрос 1: Оценка > 4.5");
            Console.WriteLine("6. Запрос 2: Ученики, сдающие более двух предметов");
            Console.WriteLine("7. Запрос 3: Средний возраст");
            Console.WriteLine("8. Запрос 4: Количество должников"); 
            Console.WriteLine("9. Сгенерировать случайные данные");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
            string? input = Console.ReadLine(); 
            switch (input) 
            {
                case null:
                    Console.WriteLine("Ввод не может быть пустым!");
                    break;
                case "1":
                    DataBaseHelper.LoadFromBin();
                    break;
                case "2":
                    DataBaseHelper.ViewAll();
                    break;
                case "3":
                    DataBaseHelper.DeleteByLastName();
                    break;
                case "4":
                    DataBaseHelper.AddRecord();
                    break;
                case "5":
                    DataBaseHelper.Query1_List();
                    break;
                case "6":
                    DataBaseHelper.Query2_List(); 
                    break;
                case "7":
                    DataBaseHelper.Query3_Value();
                    break;
                case "8":
                    DataBaseHelper.Query4_Value(); 
                    break;
                case "9":
                    DataBaseHelper.GenerateRandomData();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
    }
}