using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Lab4FileProcessor
{
    class Program
    {
        private static List<string> wordsList = new List<string>();
        private static Stopwatch stopwatch = new Stopwatch();
        private static string currentFileName = "";

        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №4: Работа с файлами ===\n");

            while (true)
            {
                ShowMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        OpenFile();
                        break;
                    case "2":
                        SearchWord();
                        break;
                    case "3":
                        ShowAllWords();
                        break;
                    case "4":
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
            Console.WriteLine("1. Открыть файл");
            Console.WriteLine("2. Найти слово");
            Console.WriteLine("3. Показать все слова");
            Console.WriteLine("4. Выход");
            Console.Write("\nВыберите действие: ");
        }

        static void OpenFile()
        {
            Console.WriteLine("\n--- Открытие файла ---");
            Console.Write("Введите путь к файлу (или нажмите Enter для примера test.txt): ");
            string? filePath = Console.ReadLine();

            // Если путь не указан, используем test.txt в текущей директории
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = "test.txt";

                // Создаем пример файла, если его нет
                if (!File.Exists(filePath))
                {
                    CreateSampleFile(filePath);
                    Console.WriteLine($"Создан тестовый файл: {Path.GetFullPath(filePath)}");
                }
            }

            // Проверка, что файл существует
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"ОШИБКА: Файл не найден: {filePath}");
                return;
            }

            // Проверка расширения .txt
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension != ".txt")
            {
                Console.WriteLine($"ПРЕДУПРЕЖДЕНИЕ: Файл имеет расширение '{extension}', рекомендуется .txt");
                Console.Write("Продолжить? (y/n): ");
                string? confirm = Console.ReadLine();
                if (confirm?.ToLower() != "y")
                    return;
            }

            try
            {
                // Начало отсчета времени загрузки
                stopwatch.Restart();

                // Чтение файла с использованием File.ReadAllText()
                string fileContent = File.ReadAllText(filePath);

                // Разделение на слова с использованием Split()
                char[] separators = new char[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?', '"', '\'', '(', ')', '[', ']', '{', '}' };
                string[] words = fileContent.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // Сохранение в список без дубликатов
                wordsList = words
                    .Select(w => w.Trim())
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .Distinct()
                    .ToList();

                // Остановка таймера
                stopwatch.Stop();

                currentFileName = Path.GetFileName(filePath);

                // Вывод результатов
                Console.WriteLine($"\n✓ Файл успешно загружен: {currentFileName}");
                Console.WriteLine($"✓ Всего уникальных слов: {wordsList.Count}");
                Console.WriteLine($"✓ Время загрузки и сохранения: {stopwatch.ElapsedMilliseconds} мс");
                Console.WriteLine($"  (Время загрузки: {stopwatch.Elapsed.TotalMilliseconds:F3} мс)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА при чтении файла: {ex.Message}");
            }
        }

        static void SearchWord()
        {
            Console.WriteLine("\n--- Поиск слова ---");

            if (wordsList.Count == 0)
            {
                Console.WriteLine("ОШИБКА: Сначала загрузите файл (выберите пункт 1)");
                return;
            }

            Console.Write("Введите слово для поиска: ");
            string? searchWord = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchWord))
            {
                Console.WriteLine("ОШИБКА: Введите непустое слово для поиска");
                return;
            }

            // Начало отсчета времени поиска
            stopwatch.Restart();

            // Поиск слова как подстроки (регистронезависимый)
            List<string> foundWords = wordsList
                .Where(w => w.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            // Остановка таймера
            stopwatch.Stop();

            // Вывод результатов
            Console.WriteLine($"\n✓ Поиск завершен");
            Console.WriteLine($"✓ Найдено слов: {foundWords.Count}");
            Console.WriteLine($"✓ Время поиска: {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"  (Время поиска: {stopwatch.Elapsed.TotalMilliseconds:F3} мс)");

            if (foundWords.Count > 0)
            {
                Console.WriteLine("\nНайденные слова:");
                int count = 0;
                foreach (string word in foundWords)
                {
                    Console.WriteLine($"  {++count}. {word}");
                    if (count >= 50)
                    {
                        Console.WriteLine($"  ... и еще {foundWords.Count - 50} слов");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine($"\n✗ Слово '{searchWord}' не найдено в списке");
            }
        }

        static void ShowAllWords()
        {
            Console.WriteLine("\n--- Все слова из списка ---");

            if (wordsList.Count == 0)
            {
                Console.WriteLine("Список пуст. Сначала загрузите файл (выберите пункт 1)");
                return;
            }

            Console.WriteLine($"Файл: {currentFileName}");
            Console.WriteLine($"Всего уникальных слов: {wordsList.Count}\n");

            Console.Write("Показать все слова? (y/n): ");
            string? confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                int count = 0;
                foreach (string word in wordsList)
                {
                    Console.WriteLine($"  {++count}. {word}");
                }
            }
            else
            {
                Console.WriteLine("Показываем первые 20 слов:");
                int count = 0;
                foreach (string word in wordsList.Take(20))
                {
                    Console.WriteLine($"  {++count}. {word}");
                }
                if (wordsList.Count > 20)
                {
                    Console.WriteLine($"  ... и еще {wordsList.Count - 20} слов");
                }
            }
        }

        static void CreateSampleFile(string filePath)
        {
            string sampleText = @"Это тестовый файл для лабораторной работы номер четыре.
Программа должна читать файл и обрабатывать слова.
В этом файле есть повторяющиеся слова: файл, слова, работа.
Класс File используется для чтения содержимого файла.
Метод Split разделяет текст на отдельные слова.
Список List сохраняет слова без дубликатов.
Класс Stopwatch измеряет время загрузки и поиска.
Поиск выполняется с помощью метода Contains класса string.";

            File.WriteAllText(filePath, sampleText);
        }
    }
}
