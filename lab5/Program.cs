using System;
using System.Collections.Generic;
using System.Linq;

namespace LevenshteinLibrary
{
    /// <summary>
    /// Библиотека для вычисления расстояния Левенштейна
    /// с использованием алгоритма Вагнера-Фишера
    /// </summary>
    public class LevenshteinCalculator
    {
        /// <summary>
        /// Простой вариант алгоритма без оптимизации
        /// </summary>
        public static int Calculate(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target) ? 0 : target.Length;

            if (string.IsNullOrEmpty(target))
                return source.Length;

            int sourceLength = source.Length;
            int targetLength = target.Length;

            // Создание матрицы расстояний
            int[,] matrix = new int[sourceLength + 1, targetLength + 1];

            // Инициализация первой строки и первого столбца
            for (int i = 0; i <= sourceLength; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                matrix[0, j] = j;

            // Заполнение матрицы
            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= targetLength; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(
                            matrix[i - 1, j] + 1,      // удаление
                            matrix[i, j - 1] + 1),     // вставка
                        matrix[i - 1, j - 1] + cost);  // замена
                }
            }

            return matrix[sourceLength, targetLength];
        }

        /// <summary>
        /// Вычисление расстояния с учетом перестановок соседних символов (Дамерау-Левенштейн)
        /// </summary>
        public static int CalculateWithTransposition(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target) ? 0 : target.Length;

            if (string.IsNullOrEmpty(target))
                return source.Length;

            int sourceLength = source.Length;
            int targetLength = target.Length;

            int[,] matrix = new int[sourceLength + 1, targetLength + 1];

            for (int i = 0; i <= sourceLength; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= targetLength; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(
                            matrix[i - 1, j] + 1,
                            matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);

                    // Проверка перестановки соседних символов
                    if (i > 1 && j > 1 &&
                        source[i - 1] == target[j - 2] &&
                        source[i - 2] == target[j - 1])
                    {
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                    }
                }
            }

            return matrix[sourceLength, targetLength];
        }

        /// <summary>
        /// Проверка, является ли расстояние допустимым (не превышает максимум)
        /// </summary>
        public static bool IsWithinMaxDistance(string source, string target, int maxDistance)
        {
            int distance = Calculate(source, target);
            return distance <= maxDistance;
        }

        /// <summary>
        /// Получение матрицы расстояний для визуализации
        /// </summary>
        public static int[,] GetDistanceMatrix(string source, string target)
        {
            if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target))
                return new int[1, 1] { { 0 } };

            int sourceLength = source?.Length ?? 0;
            int targetLength = target?.Length ?? 0;

            int[,] matrix = new int[sourceLength + 1, targetLength + 1];

            for (int i = 0; i <= sourceLength; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                matrix[0, j] = j;

            if (sourceLength > 0 && targetLength > 0 && source != null && target != null)
            {
                for (int i = 1; i <= sourceLength; i++)
                {
                    for (int j = 1; j <= targetLength; j++)
                    {
                        int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                        matrix[i, j] = Math.Min(
                            Math.Min(
                                matrix[i - 1, j] + 1,
                                matrix[i, j - 1] + 1),
                            matrix[i - 1, j - 1] + cost);
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Вывод матрицы в строковом виде для отладки
        /// </summary>
        public static string MatrixToString(string source, string target)
        {
            int[,] matrix = GetDistanceMatrix(source, target);
            int sourceLength = source?.Length ?? 0;
            int targetLength = target?.Length ?? 0;

            string result = "Матрица расстояний Левенштейна:\n\n";
            result += "    ";

            if (target != null)
            {
                for (int j = 0; j < targetLength; j++)
                    result += $"{target[j],3}";
            }

            result += "\n";

            for (int i = 0; i <= sourceLength; i++)
            {
                if (i == 0)
                    result += "  ";
                else if (source != null)
                    result += $"{source[i - 1],2}";

                for (int j = 0; j <= targetLength; j++)
                {
                    result += $"{matrix[i, j],3}";
                }
                result += "\n";
            }

            return result;
        }
    }

    // Программа-пример для демонстрации работы библиотеки
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №5: Расстояние Левенштейна ===\n");

            while (true)
            {
                ShowMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SimpleCalculation();
                        break;
                    case "2":
                        WithMaxDistance();
                        break;
                    case "3":
                        ShowMatrix();
                        break;
                    case "4":
                        BatchTest();
                        break;
                    case "5":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.\n");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Простое вычисление расстояния");
            Console.WriteLine("2. Проверка с максимальным порогом");
            Console.WriteLine("3. Показать матрицу расстояний");
            Console.WriteLine("4. Пакетное тестирование");
            Console.WriteLine("5. Выход");
            Console.Write("\nВыберите действие: ");
        }

        static void SimpleCalculation()
        {
            Console.WriteLine("\n--- Простое вычисление расстояния ---");
            Console.Write("Введите первую строку: ");
            string? str1 = Console.ReadLine();
            Console.Write("Введите вторую строку: ");
            string? str2 = Console.ReadLine();

            if (str1 == null || str2 == null)
            {
                Console.WriteLine("Ошибка: некорректный ввод");
                return;
            }

            int distance = LevenshteinCalculator.Calculate(str1, str2);
            int distanceWithTransposition = LevenshteinCalculator.CalculateWithTransposition(str1, str2);

            Console.WriteLine($"\n✓ Расстояние Левенштейна: {distance}");
            Console.WriteLine($"✓ Расстояние Дамерау-Левенштейна (с перестановками): {distanceWithTransposition}");
        }

        static void WithMaxDistance()
        {
            Console.WriteLine("\n--- Проверка с максимальным порогом ---");
            Console.Write("Введите первую строку: ");
            string? str1 = Console.ReadLine();
            Console.Write("Введите вторую строку: ");
            string? str2 = Console.ReadLine();
            Console.Write("Введите максимальное допустимое расстояние: ");

            if (str1 == null || str2 == null)
            {
                Console.WriteLine("Ошибка: некорректный ввод");
                return;
            }

            if (int.TryParse(Console.ReadLine(), out int maxDistance))
            {
                int distance = LevenshteinCalculator.Calculate(str1, str2);
                bool isValid = LevenshteinCalculator.IsWithinMaxDistance(str1, str2, maxDistance);

                Console.WriteLine($"\n✓ Расстояние Левенштейна: {distance}");
                Console.WriteLine($"✓ Максимальное расстояние: {maxDistance}");

                if (isValid)
                    Console.WriteLine($"✓ Строки совпадают (расстояние {distance} ≤ {maxDistance})");
                else
                    Console.WriteLine($"✗ Строки НЕ совпадают (расстояние {distance} > {maxDistance})");
            }
            else
            {
                Console.WriteLine("Ошибка: введите корректное число");
            }
        }

        static void ShowMatrix()
        {
            Console.WriteLine("\n--- Матрица расстояний ---");
            Console.Write("Введите первую строку: ");
            string? str1 = Console.ReadLine();
            Console.Write("Введите вторую строку: ");
            string? str2 = Console.ReadLine();

            if (str1 == null || str2 == null)
            {
                Console.WriteLine("Ошибка: некорректный ввод");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(LevenshteinCalculator.MatrixToString(str1, str2));

            int distance = LevenshteinCalculator.Calculate(str1, str2);
            Console.WriteLine($"✓ Итоговое расстояние Левенштейна: {distance}");
        }

        static void BatchTest()
        {
            Console.WriteLine("\n--- Пакетное тестирование ---\n");

            var testCases = new List<(string, string, int)>
            {
                ("kitten", "sitting", 3),
                ("saturday", "sunday", 3),
                ("book", "back", 2),
                ("", "abc", 3),
                ("abc", "", 3),
                ("abc", "abc", 0),
                ("fast", "cats", 3),
                ("ant", "aunt", 1)
            };

            int passed = 0;
            int failed = 0;

            foreach (var (str1, str2, expected) in testCases)
            {
                int distance = LevenshteinCalculator.Calculate(str1, str2);
                bool success = (distance == expected);

                if (success)
                {
                    Console.WriteLine($"✓ PASS: \"{str1}\" -> \"{str2}\" = {distance} (ожидалось {expected})");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"✗ FAIL: \"{str1}\" -> \"{str2}\" = {distance} (ожидалось {expected})");
                    failed++;
                }
            }

            Console.WriteLine($"\n--- Результаты ---");
            Console.WriteLine($"Пройдено: {passed}/{testCases.Count}");
            Console.WriteLine($"Провалено: {failed}/{testCases.Count}");
        }
    }
}
