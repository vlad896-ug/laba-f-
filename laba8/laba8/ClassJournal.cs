using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Journal
{
    internal class ClassJournal
    {
        private string lastName;
        private int age;
        private List<string> examSubjects;
        private double averageGrade;
        private bool areAnyDebts;

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия не может быть null, пустой строкой или состоять только из пробелов.", nameof(LastName));
                }
                lastName = value;
            }
        }

        public int Age
        {
            get { return age; }
            set
            {
                if (value < 10 || value > 18)
                {
                    throw new ArgumentOutOfRangeException(nameof(Age), value, "Возраст должен быть положительным числом.");
                }
                age = value;
            }
        }

        public List<string> ExamSubjects
        {
            get { return examSubjects; }
            set
            {
                if (value.Any(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentException("Список предметов не может содержать null или пустые записи.", nameof(ExamSubjects));
                }
                examSubjects = value;
            }
        }

        public double AverageGrade
        {
            get { return averageGrade; }
            set
            {
                if (value < 2.0 || value > 5.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(AverageGrade), value, "Средняя оценка должна быть между 2.0 и 5.0.");
                }
                averageGrade = value;
            }
        }

        public bool AreAnyDebts
        {
            get { return areAnyDebts; }
            set { areAnyDebts = value; }
        }

        public ClassJournal() 
        {
            LastName = "Неизвестно"; 
            Age = 10;
            ExamSubjects = new List<string> { "Без предметов" };
            AverageGrade = 2.0; 
            AreAnyDebts = false; 
        }

        public ClassJournal(string lastName, int age, List<string> examSubjects, double averageGrade, bool areAnyDebts)
        {
            LastName = lastName;
            Age = age;
            ExamSubjects = examSubjects;
            AverageGrade = averageGrade;
            AreAnyDebts = areAnyDebts;
        }

        public override string ToString()
        {
            return $"{LastName,-15} | Age: {Age,-2} | ExamSubjects: {string.Join(", ", ExamSubjects)} | AvgGrade: {AverageGrade:F2} | Debts: {AreAnyDebts}";
        }
    }
}
