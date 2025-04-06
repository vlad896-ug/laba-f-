using Number_Triplet;
using Input_Validation;
using Input_Date;

internal class Program
{
    static void Main(string[] args)
    {
        bool isValidDate = false;
        do
        {
        int day = InputValidation.InputIntegerWithValidation("Введите день:");
        int month = InputValidation.InputIntegerWithValidation("Введите месяц:");
        int year = InputValidation.InputIntegerWithValidation("Введите год:");

        Date date = new Date(day, month, year);
        isValidDate = date.IsValid();

        if (isValidDate)
        {
                Console.WriteLine(date);
                Console.WriteLine($"День недели: {date.DayOfWeek()}");
                Console.WriteLine($"Дней с начала года: {date.DaysSinceStartOfYear()}");
                Console.WriteLine($"Минимальная последняя цифра: {date.MinLastDigit()}");
                Console.WriteLine($"Високосный год? {date.IsLeapYear()}");
        }
            else
            {
                Console.WriteLine("Неверная дата.");
            }
        } while (!isValidDate);


        int number1 = InputValidation.InputIntegerWithValidation($"Введите первое целое число: ");
        int number2 = InputValidation.InputIntegerWithValidation($"Введите второе целое число: ");
        int number3 = InputValidation.InputIntegerWithValidation($"Введите третье целое число: ");

        NumberTriplet numbers = new NumberTriplet(number1, number2, number3);

        Console.WriteLine(numbers);
        Console.WriteLine($"Минимальная последняя цифра: {numbers.MinLastDigit()}");

        Console.WriteLine($"Копия: ");
        NumberTriplet copyTriplet = new NumberTriplet(numbers);
        Console.WriteLine(copyTriplet);

        Console.ReadKey();
    }
}

