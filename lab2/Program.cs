using System;

namespace GeometricShapes
{
    // Интерфейс IPrint
    interface IPrint
    {
        void Print();
    }

    // Абстрактный класс "Геометрическая фигура"
    abstract class GeometricShape
    {
        // Виртуальный метод для вычисления площади
        public abstract double CalculateArea();

        // Переопределение ToString()
        public override string ToString()
        {
            return $"Фигура: {this.GetType().Name}, Площадь: {CalculateArea():F2}";
        }
    }

    // Класс "Прямоугольник" наследуется от "Геометрическая фигура"
    class Rectangle : GeometricShape, IPrint
    {
        // Свойства
        public double Width { get; set; }
        public double Height { get; set; }

        // Конструктор по параметрам "ширина" и "высота"
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        // Переопределение метода вычисления площади
        public override double CalculateArea()
        {
            return Width * Height;
        }

        // Переопределение ToString()
        public override string ToString()
        {
            return $"Прямоугольник (Ширина: {Width}, Высота: {Height}), Площадь: {CalculateArea():F2}";
        }

        // Реализация интерфейса IPrint
        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }

    // Класс "Квадрат" наследуется от "Прямоугольник"
    class Square : Rectangle
    {
        // Конструктор по длине стороны
        public Square(double side) : base(side, side)
        {
        }

        // Переопределение ToString()
        public override string ToString()
        {
            return $"Квадрат (Сторона: {Width}), Площадь: {CalculateArea():F2}";
        }
    }

    // Класс "Круг" наследуется от "Геометрическая фигура"
    class Circle : GeometricShape, IPrint
    {
        // Свойство
        public double Radius { get; set; }

        // Конструктор по параметру "радиус"
        public Circle(double radius)
        {
            Radius = radius;
        }

        // Переопределение метода вычисления площади
        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }

        // Переопределение ToString()
        public override string ToString()
        {
            return $"Круг (Радиус: {Radius}), Площадь: {CalculateArea():F2}";
        }

        // Реализация интерфейса IPrint
        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №2: Геометрические фигуры ===\n");

            // Создание объектов
            Rectangle rectangle = new Rectangle(5, 10);
            Square square = new Square(7);
            Circle circle = new Circle(4);

            // Вывод информации через ToString()
            Console.WriteLine("--- Информация о фигурах ---");
            Console.WriteLine(rectangle.ToString());
            Console.WriteLine(square.ToString());
            Console.WriteLine(circle.ToString());

            Console.WriteLine("\n--- Вывод через интерфейс IPrint ---");
            rectangle.Print();
            square.Print();
            circle.Print();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
