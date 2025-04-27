using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Quadratic_Equation;
using Input_Validation;

namespace QuadraticEquationWPF
{
    public partial class MainWindow : Window
    {
        private QuadraticEquation? equation1;
        private QuadraticEquation? equation2;

        public MainWindow()
        {
            InitializeComponent();
            equation1 = null;
            equation2 = null;
        }

        private void CreateEquation1Button_Click(object sender, RoutedEventArgs e)
        {
            string aText = A1TextBox.Text;
            string bText = B1TextBox.Text;
            string cText = C1TextBox.Text;
            string errorMessage;

            double? a = InputValidation.InputDoubleWithValidation(aText, out errorMessage);
            if (a == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            double? b = InputValidation.InputDoubleWithValidation(bText, out errorMessage);
            if (b == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            double? c = InputValidation.InputDoubleWithValidation(cText, out errorMessage);
            if (c == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            try
            {
                equation1 = new QuadraticEquation((double)a, (double)b, (double)c);
                Equation1Display.Text = $"equation1: {equation1}";
                UpdateEquation1Properties();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка аргумента");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }


        private void CreateEquation2Button_Click(object sender, RoutedEventArgs e)
        {
            string aText = A2TextBox.Text;
            string bText = B2TextBox.Text;
            string cText = C2TextBox.Text;
            string errorMessage;

            double? a = InputValidation.InputDoubleWithValidation(aText, out errorMessage);
            if (a == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            double? b = InputValidation.InputDoubleWithValidation(bText, out errorMessage);
            if (b == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            double? c = InputValidation.InputDoubleWithValidation(cText, out errorMessage);
            if (c == null)
            {
                MessageBox.Show(errorMessage, "Ошибка ввода");
                return;
            }

            try
            {
                equation2 = new QuadraticEquation((double)a, (double)b, (double)c);
                Equation2Display.Text = $"equation2: {equation2}";
                UpdateEquation2Properties();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка аргумента");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }



        private void CalculateRoots1Button_Click(object sender, RoutedEventArgs e)
        {
            if (equation1 == null)
            {
                MessageBox.Show("Сначала создайте первое уравнение.", "Предупреждение");
                return;
            }

            try
            {
                double[] roots = equation1.CalculateRoots();
                if (roots.Length == 0)
                {
                    Roots1Display.Text = "Уравнение не имеет действительных корней.";
                }
                else if (roots.Length == 1)
                {
                    Roots1Display.Text = $"Корень уравнения: x = {roots[0]}";
                }
                else
                {
                    Roots1Display.Text = $"Корни уравнения: x1 = {roots[0]}, x2 = {roots[1]}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при вычислении корней: {ex.Message}", "Ошибка");
            }
        }

        private void CalculateRoots2Button_Click(object sender, RoutedEventArgs e)
        {
            if (equation2 == null)
            {
                MessageBox.Show("Сначала создайте второе уравнение.", "Предупреждение");
                return;
            }

            try
            {
                double[] roots = equation2.CalculateRoots();
                if (roots.Length == 0)
                {
                    Roots2Display.Text = "Уравнение не имеет действительных корней.";
                }
                else if (roots.Length == 1)
                {
                    Roots2Display.Text = $"Корень уравнения: x = {roots[0]}";
                }
                else
                {
                    Roots2Display.Text = $"Корни уравнения: x1 = {roots[0]}, x2 = {roots[1]}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при вычислении корней: {ex.Message}", "Ошибка");
            }
        }

        private void Increment1Button_Click(object sender, RoutedEventArgs e)
        {
            if (equation1 == null)
            {
                MessageBox.Show("Сначала создайте первое уравнение.", "Предупреждение");
                return;
            }

            try
            {
                equation1++;
                Equation1Display.Text = $"++equation1: {equation1}";
                UpdateEquation1Properties();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при инкременте: {ex.Message}", "Ошибка");
            }
        }

        private void Decrement1Button_Click(object sender, RoutedEventArgs e)
        {
            if (equation1 == null)
            {
                MessageBox.Show("Сначала создайте первое уравнение.", "Предупреждение");
                return;
            }

            try
            {
                equation1--;
                Equation1Display.Text = $"--equation1: {equation1}";
                UpdateEquation1Properties();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при декременте: {ex.Message}", "Ошибка");
            }
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            if (equation1 == null || equation2 == null)
            {
                MessageBox.Show("Сначала создайте оба уравнения.", "Предупреждение");
                return;
            }

            try
            {
                bool isEqual = equation1 == equation2;
                ComparisonResultDisplay.Text = $"equation1 == equation2: {isEqual}";
                NotEqualResultDisplay.Text = $"equation1 != equation2: {equation1 != equation2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сравнении: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateEquation1Properties()
        {
            if (equation1 != null)
            {
                try
                {
                    double discriminant = equation1;
                    Discriminant1Display.Text = $"Дискриминант equation1: {discriminant}";

                    bool hasRoots = (bool)equation1;
                    HasRoots1Display.Text = $"equation1 имеет корни: {hasRoots}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при получении свойств уравнения 1: {ex.Message}", "Ошибка");
                }
            }
            else
            {
                Discriminant1Display.Text = "Дискриминант equation1: N/A";
                HasRoots1Display.Text = "equation1 имеет корни: N/A";
            }
        }

        private void UpdateEquation2Properties()
        {
            if (equation2 != null)
            {
                try
                {
                    double discriminant = equation2;
                    Discriminant2Display.Text = $"Дискриминант equation2: {discriminant}";

                    bool hasRoots = (bool)equation2;
                    HasRoots2Display.Text = $"equation2 имеет корни: {hasRoots}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при получении свойств уравнения 2: {ex.Message}", "Ошибка");
                }
            }
            else
            {
                Discriminant2Display.Text = "Дискриминант equation2: N/A";
                HasRoots2Display.Text = "equation2 имеет корни: N/A";
            }
        }

    }
}

