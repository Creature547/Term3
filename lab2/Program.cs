using System;

namespace GeometricShapes
{
    interface IPrint
    {
        void Print();
    }

    abstract class GeometricShape
    {
        public abstract double CalculateArea();

        public override string ToString()
        {
            return $"Фигура: {this.GetType().Name}, Площадь: {CalculateArea():F2}";
        }
    }

    // Класс "Прямоугольник" наследуется от "Геометрическая фигура"
    class Rectangle : GeometricShape, IPrint
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override double CalculateArea()
        {
            return Width * Height;
        }

        public override string ToString()
        {
            return $"Прямоугольник (Ширина: {Width}, Высота: {Height}), Площадь: {CalculateArea():F2}";
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }

    class Square : Rectangle
    {
        public Square(double side) : base(side, side)
        {
        }

        public override string ToString()
        {
            return $"Квадрат (Сторона: {Width}), Площадь: {CalculateArea():F2}";
        }
    }

    // Класс "Круг" наследуется от "Геометрическая фигура"
    class Circle : GeometricShape, IPrint
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }

        public override string ToString()
        {
            return $"Круг (Радиус: {Radius}), Площадь: {CalculateArea():F2}";
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №2: Геометрические фигуры ===\n");
            Rectangle rectangle = new Rectangle(5, 10);
            Square square = new Square(7);
            Circle circle = new Circle(4);

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
