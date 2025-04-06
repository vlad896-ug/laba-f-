using Quadratic_Equation;
using Input_Validation;

class Program
{
    static void Main(string[] args)
    {
        double a = InputValidation.InputDoubleWithValidation("Введите коэффициент a: ");
        double b = InputValidation.InputDoubleWithValidation("Введите коэффициент b: ");
        double c = InputValidation.InputDoubleWithValidation("Введите коэффициент c: ");
        QuadraticEquation equation1 = new QuadraticEquation(a, b, c);
        Console.WriteLine($"equation1: {equation1}");
        double[] roots = equation1.CalculateRoots();
        if (roots.Length == 0)
        {
            Console.WriteLine("Уравнение не имеет действительных корней.");
        }
        else if (roots.Length == 1)
        {
            Console.WriteLine($"Корень уравнения: x = {roots[0]}");
        }
        else
        {
            Console.WriteLine($"Корни уравнения: x1 = {roots[0]}, x2 = {roots[1]}");
        }

        ++equation1;
        Console.WriteLine($"++equation1: {equation1}");

        --equation1;
        Console.WriteLine($"--equation1: {equation1}");

        double discriminant = equation1;
        Console.WriteLine($"Дискриминант equation1: {discriminant}");

        bool hasRoots = (bool)equation1;
        Console.WriteLine($"equation1 имеет корни: {hasRoots}");

        double a1 = InputValidation.InputDoubleWithValidation("Введите коэффициент a для второго уравнения: ");
        double b1 = InputValidation.InputDoubleWithValidation("Введите коэффициент b для второго уравнения: ");
        double c1 = InputValidation.InputDoubleWithValidation("Введите коэффициент c для второго уравнения: ");
        QuadraticEquation equation2 = new QuadraticEquation(a1, b1, c1);
        Console.WriteLine($"equation2: {equation2}");

        Console.WriteLine($"equation1 == equation2: {equation1 == equation2}");
        Console.WriteLine($"equation1 != equation2: {equation1 != equation2}");
        Console.ReadKey();
    }
}