using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab3Collections
{
    // Абстрактный класс "Геометрическая фигура" с System.IComparable
    abstract class GeometricShape : System.IComparable
    {
        public abstract double CalculateArea();

        public override string ToString()
        {
            return $"{this.GetType().Name}: Площадь = {CalculateArea():F2}";
        }

        // Реализация IComparable для сортировки по площади
        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;

            GeometricShape? other = obj as GeometricShape;
            if (other != null)
                return this.CalculateArea().CompareTo(other.CalculateArea());
            else
                throw new ArgumentException("Объект не является GeometricShape");
        }
    }

    // Класс "Прямоугольник"
    class Rectangle : GeometricShape
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
            return $"Прямоугольник ({Width} x {Height}): Площадь = {CalculateArea():F2}";
        }
    }

    // Класс "Квадрат"
    class Square : Rectangle
    {
        public Square(double side) : base(side, side)
        {
        }

        public override string ToString()
        {
            return $"Квадрат (сторона {Width}): Площадь = {CalculateArea():F2}";
        }
    }

    // Класс "Круг"
    class Circle : GeometricShape
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
            return $"Круг (радиус {Radius}): Площадь = {CalculateArea():F2}";
        }
    }

    // Класс разреженной матрицы
    class SparseMatrix
    {
        private Dictionary<string, double> elements;
        private int rows;
        private int cols;

        public SparseMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            elements = new Dictionary<string, double>();
        }

        // Индексатор для доступа к элементам
        public double this[int x, int y, int z]
        {
            get
            {
                string key = $"{x},{y},{z}";
                return elements.ContainsKey(key) ? elements[key] : 0.0;
            }
            set
            {
                string key = $"{x},{y},{z}";
                if (value != 0.0)
                    elements[key] = value;
                else if (elements.ContainsKey(key))
                    elements.Remove(key);
            }
        }

        public override string ToString()
        {
            if (elements.Count == 0)
                return "Разреженная матрица пуста";

            string result = "Разреженная матрица (ненулевые элементы):\n";
            foreach (var kvp in elements)
            {
                result += $"  [{kvp.Key}] = {kvp.Value:F2}\n";
            }
            return result;
        }
    }

    class SimpleStack<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node? Next { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node? top;
        private int count;

        public SimpleStack()
        {
            top = null;
            count = 0;
        }

        // Добавление в стек
        public void Push(T element)
        {
            Node newNode = new Node(element);
            newNode.Next = top;
            top = newNode;
            count++;
        }

        // Чтение с удалением из стека
        public T Pop()
        {
            if (top == null)
                throw new InvalidOperationException("Стек пуст");

            T data = top.Data;
            top = top.Next;
            count--;
            return data;
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public override string ToString()
        {
            if (IsEmpty)
                return "Стек пуст";

            string result = "Содержимое стека (сверху вниз):\n";
            Node? current = top;
            while (current != null)
            {
                result += $"  {current.Data}\n";
                current = current.Next;
            }
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №3: Коллекции ===\n");

            // 2. Создание объектов
            Rectangle rect1 = new Rectangle(5, 10);
            Rectangle rect2 = new Rectangle(3, 7);
            Square square1 = new Square(6);
            Square square2 = new Square(4);
            Circle circle1 = new Circle(5);
            Circle circle2 = new Circle(3);

            // 3. ArrayList с сортировкой
            Console.WriteLine("--- 3. ArrayList с сортировкой ---");
            ArrayList arrayList = new ArrayList();
            arrayList.Add(rect1);
            arrayList.Add(square1);
            arrayList.Add(circle1);
            arrayList.Add(rect2);
            arrayList.Add(square2);
            arrayList.Add(circle2);

            Console.WriteLine("До сортировки:");
            foreach (GeometricShape shape in arrayList)
                Console.WriteLine($"  {shape}");

            arrayList.Sort();

            Console.WriteLine("\nПосле сортировки по площади:");
            foreach (GeometricShape shape in arrayList)
                Console.WriteLine($"  {shape}");

            // 4. List<GeometricShape>
            Console.WriteLine("\n--- 4. List<GeometricShape> ---");
            List<GeometricShape> shapeList = new List<GeometricShape>();
            shapeList.Add(new Rectangle(8, 4));
            shapeList.Add(new Square(5));
            shapeList.Add(new Circle(4));

            Console.WriteLine("До сортировки:");
            foreach (var shape in shapeList)
                Console.WriteLine($"  {shape}");

            shapeList.Sort();

            Console.WriteLine("\nПосле сортировки по площади:");
            foreach (var shape in shapeList)
                Console.WriteLine($"  {shape}");

            // 6. Разреженная матрица
            Console.WriteLine("\n--- 6. Разреженная матрица (3D) ---");
            SparseMatrix matrix = new SparseMatrix(100, 100);

            // Пример использования для геометрических фигур
            matrix[0, 0, 0] = rect1.CalculateArea();
            matrix[1, 2, 3] = circle1.CalculateArea();
            matrix[5, 10, 15] = square1.CalculateArea();
            matrix[10, 20, 30] = 42.5;

            Console.WriteLine(matrix.ToString());

            // 7-8. SimpleStack на основе геометрических фигур
            Console.WriteLine("\n--- 7-8. SimpleStack<GeometricShape> ---");
            SimpleStack<GeometricShape> stack = new SimpleStack<GeometricShape>();

            Console.WriteLine("Добавляем фигуры в стек:");
            stack.Push(new Rectangle(3, 4));
            Console.WriteLine("  Добавлен прямоугольник 3x4");
            stack.Push(new Square(5));
            Console.WriteLine("  Добавлен квадрат со стороной 5");
            stack.Push(new Circle(2));
            Console.WriteLine("  Добавлен круг с радиусом 2");

            Console.WriteLine($"\nВсего элементов в стеке: {stack.Count}");
            Console.WriteLine(stack.ToString());

            Console.WriteLine("Извлекаем элементы из стека:");
            while (!stack.IsEmpty)
            {
                GeometricShape shape = stack.Pop();
                Console.WriteLine($"  Извлечено: {shape}");
            }

            Console.WriteLine($"\nВсего элементов в стеке: {stack.Count}");

            Console.WriteLine("\n--- Пример работы стека с разными типами ---");
            SimpleStack<GeometricShape> demoStack = new SimpleStack<GeometricShape>();

            demoStack.Push(rect1);
            demoStack.Push(square2);
            demoStack.Push(circle2);

            Console.WriteLine("Демонстрация стека:");
            Console.WriteLine(demoStack.ToString());

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
