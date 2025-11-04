using System;
using System.Linq.Expressions;
class Program
{
    static void Main()
    {
        Console.WriteLine("Решение биквадратных уравнений");

        double A1;
        while (true)
        {
            Console.Write("Введите коэффициент А: ");
            var A = Console.ReadLine();
            if (double.TryParse(A, out A1))
            {
                break;
            }
            Console.WriteLine("Ошибка: вы ввели значение неверного формата");
        }

        double B1;
        while (true)
        {
            Console.Write("Введите коэффициент B: ");
            var B = Console.ReadLine();
            if (double.TryParse(B, out B1))
            {
                break;
            }
            Console.WriteLine("Ошибка: вы ввели значение неверного формата");
        }


        double C1;
        while (true)
        {
            Console.Write("Введите коэффициент C: ");
            var C = Console.ReadLine();
            if (double.TryParse(C, out C1))
            {
                break;
            }
            Console.WriteLine("Ошибка: вы ввели значение неверного формата");
        }

        double D = B1 * B1 - 4 * A1 * C1;
        // Console.WriteLine(D);
        if (D < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Дискриминант отрицательный. Решений нет");
            Console.ResetColor();
        }
        else if (D > 0)
        {
            bool flag = false;
            double t1 = (-B1 + Math.Sqrt(D)) / (2 * A1);
            double t2 = (-B1 - Math.Sqrt(D)) / (2 * A1);

            if (t1 >= 0)
            {
                flag = true;
                double x1 = Math.Sqrt(t1);
                double x2 = -Math.Sqrt(t1);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(x1);
                Console.WriteLine(x2);
                Console.ResetColor();
            }
            if (t2 >= 0)
            {
                flag = true;
                double x3 = Math.Sqrt(t2);
                double x4 = -Math.Sqrt(t2);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(x3);
                Console.WriteLine(x4);
                Console.ResetColor();
            }
            if (flag == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Решений не существует");
                Console.ResetColor();
            }
        }
        else
        {
            double t0 = -B1 / (2 * A1);
            if (t0 < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Решений нет");
                Console.ResetColor();
            }
            else
            {
                double x1 = Math.Sqrt(t0);
                double x2 = -Math.Sqrt(t0);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(x1);
                Console.WriteLine(x2);
                Console.ResetColor();
            }
        }

    }
}
