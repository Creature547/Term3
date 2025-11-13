using System;
using System.Reflection;
using System.Linq;

namespace Lab6DelegatesReflection
{
    // ===== ЧАСТЬ 1: ДЕЛЕГАТЫ =====

    // Делегат с несколькими параметрами разных типов
    public delegate double MathOperation(int a, double b, string operation);

    // Делегат для демонстрации
    public delegate void MessageDelegate(string message);

    class Calculator
    {
        // Метод для делегата MathOperation
        public static double Calculate(int a, double b, string operation)
        {
            switch (operation.ToLower())
            {
                case "add":
                    return a + b;
                case "multiply":
                    return a * b;
                case "divide":
                    return b != 0 ? a / b : 0;
                default:
                    return 0;
            }
        }

        // Метод для вывода сообщения
        public static void PrintMessage(string msg)
        {
            Console.WriteLine($"Сообщение: {msg}");
        }

        // Метод с параметром-делегатом
        public static void ProcessWithDelegate(MathOperation operation, int x, double y, string op)
        {
            Console.WriteLine($"\n--- Вызов через делегат как параметр ---");
            double result = operation(x, y, op);
            Console.WriteLine($"Результат операции '{op}' для ({x}, {y}): {result}");
        }
    }

    // ===== ЧАСТЬ 2: РЕФЛЕКСИЯ =====

    // Атрибут для пометки свойств
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public DisplayAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    // Класс для демонстрации рефлексии
    public class Person
    {
        [Display("Имя пользователя")]
        public string Name { get; set; }

        [Display("Возраст")]
        public int Age { get; set; }

        public string Email { get; set; }

        public Person()
        {
            Name = "Неизвестно";
            Age = 0;
            Email = "not_set@example.com";
        }

        public Person(string name, int age, string email)
        {
            Name = name;
            Age = age;
            Email = email;
        }

        public void Introduce()
        {
            Console.WriteLine($"Привет! Меня зовут {Name}, мне {Age} лет.");
        }

        public string GetInfo()
        {
            return $"Имя: {Name}, Возраст: {Age}, Email: {Email}";
        }

        private void SecretMethod()
        {
            Console.WriteLine("Это приватный метод!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №6: Делегаты и Рефлексия ===\n");

            while (true)
            {
                ShowMenu();
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DemoDelegates();
                        break;
                    case "2":
                        DemoFuncAction();
                        break;
                    case "3":
                        DemoReflection();
                        break;
                    case "4":
                        DemoAttributes();
                        break;
                    case "5":
                        DemoReflectionInvoke();
                        break;
                    case "6":
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
            Console.WriteLine("1. Демонстрация пользовательских делегатов");
            Console.WriteLine("2. Демонстрация Func<> и Action<>");
            Console.WriteLine("3. Рефлексия: информация о классе");
            Console.WriteLine("4. Рефлексия: работа с атрибутами");
            Console.WriteLine("5. Рефлексия: вызов методов динамически");
            Console.WriteLine("6. Выход");
            Console.Write("\nВыберите действие: ");
        }

        // ЧАСТЬ 1: Демонстрация делегатов
        static void DemoDelegates()
        {
            Console.WriteLine("\n=== ЧАСТЬ 1: Пользовательские делегаты ===\n");

            // Создание экземпляра делегата
            MathOperation mathOp = Calculator.Calculate;

            Console.WriteLine("--- Прямой вызов делегата ---");
            double result1 = mathOp(10, 5.5, "add");
            Console.WriteLine($"10 + 5.5 = {result1}");

            double result2 = mathOp(10, 5.5, "multiply");
            Console.WriteLine($"10 * 5.5 = {result2}");

            // Передача делегата как параметра
            Calculator.ProcessWithDelegate(mathOp, 20, 4.0, "divide");

            // Делегат с лямбда-выражением
            Console.WriteLine("\n--- Делегат с лямбда-выражением ---");
            MessageDelegate msgDel = (msg) => Console.WriteLine($"[Lambda] {msg}");
            msgDel("Привет из лямбда-выражения!");

            // Многоадресный делегат (multicast)
            Console.WriteLine("\n--- Многоадресный делегат ---");
            MessageDelegate multiDel = Calculator.PrintMessage;
            multiDel += (msg) => Console.WriteLine($"[Дополнительно] {msg}");
            multiDel += (msg) => Console.WriteLine($"[Еще раз] {msg}");

            multiDel("Тестовое сообщение");
        }

        // ЧАСТЬ 1: Демонстрация Func<> и Action<>
        static void DemoFuncAction()
        {
            Console.WriteLine("\n=== ЧАСТЬ 1: Func<> и Action<> ===\n");

            // Action<> - делегат без возвращаемого значения
            Console.WriteLine("--- Action<> делегат ---");
            Action<string> printAction = (message) => Console.WriteLine($"Action: {message}");
            printAction("Привет от Action!");

            Action<int, int> sumAction = (a, b) => Console.WriteLine($"Сумма: {a + b}");
            sumAction(5, 10);

            // Func<> - делегат с возвращаемым значением
            Console.WriteLine("\n--- Func<> делегат ---");
            Func<int, int, int> addFunc = (x, y) => x + y;
            int sum = addFunc(7, 3);
            Console.WriteLine($"7 + 3 = {sum}");

            Func<string, int> lengthFunc = (str) => str.Length;
            int length = lengthFunc("Hello, World!");
            Console.WriteLine($"Длина строки 'Hello, World!': {length}");

            // Комбинирование Func<>
            Func<int, int> square = x => x * x;
            Func<int, int> addTen = x => x + 10;

            int value = 5;
            int result = addTen(square(value)); // (5^2) + 10 = 35
            Console.WriteLine($"Результат комбинации функций для 5: {result}");
        }

        // ЧАСТЬ 2: Рефлексия - информация о классе
        static void DemoReflection()
        {
            Console.WriteLine("\n=== ЧАСТЬ 2: Рефлексия - Информация о классе ===\n");

            Type personType = typeof(Person);

            // Информация о классе
            Console.WriteLine($"Полное имя класса: {personType.FullName}");
            Console.WriteLine($"Пространство имен: {personType.Namespace}");
            Console.WriteLine($"Имя класса: {personType.Name}");
            Console.WriteLine($"Является ли классом: {personType.IsClass}");

            // Информация о конструкторах
            Console.WriteLine("\n--- Конструкторы ---");
            ConstructorInfo[] constructors = personType.GetConstructors();
            foreach (var ctor in constructors)
            {
                var parameters = ctor.GetParameters();
                string paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  Constructor({paramList})");
            }

            // Информация о свойствах
            Console.WriteLine("\n--- Свойства ---");
            PropertyInfo[] properties = personType.GetProperties();
            foreach (var prop in properties)
            {
                Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name}");
                Console.WriteLine($"    Можно читать: {prop.CanRead}, Можно писать: {prop.CanWrite}");
            }

            // Информация о методах
            Console.WriteLine("\n--- Методы ---");
            MethodInfo[] methods = personType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                string paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({paramList})");
            }
        }

        // ЧАСТЬ 2: Рефлексия - работа с атрибутами
        static void DemoAttributes()
        {
            Console.WriteLine("\n=== ЧАСТЬ 2: Рефлексия - Атрибуты ===\n");

            Type personType = typeof(Person);

            Console.WriteLine("--- Свойства с атрибутом Display ---");
            PropertyInfo[] properties = personType.GetProperties();

            foreach (var prop in properties)
            {
                // Получение атрибута
                var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();

                if (displayAttr != null)
                {
                    Console.WriteLine($"  Свойство: {prop.Name}");
                    Console.WriteLine($"  Отображаемое имя: {displayAttr.DisplayName}");
                    Console.WriteLine($"  Тип: {prop.PropertyType.Name}");
                }
                else
                {
                    Console.WriteLine($"  Свойство: {prop.Name} (без атрибута Display)");
                }
                Console.WriteLine();
            }
        }

        // ЧАСТЬ 2: Рефлексия - вызов методов
        static void DemoReflectionInvoke()
        {
            Console.WriteLine("\n=== ЧАСТЬ 2: Рефлексия - Динамический вызов методов ===\n");

            // Создание объекта через рефлексию
            Type personType = typeof(Person);

            Console.WriteLine("--- Создание объекта через рефлексию ---");
            object? personObj = Activator.CreateInstance(personType, "Аня", 20, "anya@example.com");

            if (personObj == null)
            {
                Console.WriteLine("Ошибка создания объекта");
                return;
            }

            Console.WriteLine("Объект создан успешно!");

            // Получение и установка значений свойств
            Console.WriteLine("\n--- Работа со свойствами через рефлексию ---");
            PropertyInfo? nameProp = personType.GetProperty("Name");
            if (nameProp != null)
            {
                string? currentName = nameProp.GetValue(personObj) as string;
                Console.WriteLine($"Текущее имя: {currentName}");

                nameProp.SetValue(personObj, "Анна");
                string? newName = nameProp.GetValue(personObj) as string;
                Console.WriteLine($"Новое имя: {newName}");
            }

            // Вызов метода через рефлексию
            Console.WriteLine("\n--- Вызов методов через рефлексию ---");
            MethodInfo? introduceMethod = personType.GetMethod("Introduce");
            if (introduceMethod != null)
            {
                Console.WriteLine("Вызов метода Introduce():");
                introduceMethod.Invoke(personObj, null);
            }

            MethodInfo? getInfoMethod = personType.GetMethod("GetInfo");
            if (getInfoMethod != null)
            {
                Console.WriteLine("\nВызов метода GetInfo():");
                object? result = getInfoMethod.Invoke(personObj, null);
                Console.WriteLine(result);
            }

            // Вызов приватного метода
            Console.WriteLine("\n--- Вызов приватного метода ---");
            MethodInfo? secretMethod = personType.GetMethod("SecretMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            if (secretMethod != null)
            {
                Console.WriteLine("Вызов приватного метода SecretMethod():");
                secretMethod.Invoke(personObj, null);
            }
        }
    }
}
