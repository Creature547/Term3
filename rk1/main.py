# Класс "Компьютер"
class Computer:
    def __init__(self, id, name):
        self.id = id
        self.name = name

# Класс "Микропроцессор"
class Processor:
    def __init__(self, id, name, frequency):
        self.id = id
        self.name = name
        self.frequency = frequency  # Частота в ГГц

# Класс для связи многие-ко-многим
class ProcessorComputer:
    def __init__(self, processor_id, computer_id):
        self.processor_id = processor_id
        self.computer_id = computer_id

# Создание тестовых данных
computers = [
    Computer(1, "Acer Aspire"),
    Computer(2, "Apple MacBook"),
    Computer(3, "ASUS VivoBook"),
    Computer(4, "Dell Inspiron"),
    Computer(5, "HP Pavilion")
]

processors = [
    Processor(1, "Intel Core i5", 2.4),
    Processor(2, "AMD Ryzen 7", 3.6),
    Processor(3, "Intel Core i7", 3.2),
    Processor(4, "Apple M1", 3.0),
    Processor(5, "AMD Ryzen 5", 2.8),
    Processor(6, "Intel Core i3", 2.0)
]

# Связь многие-ко-многим (связывает компьютеры и процессоры)
processor_computer = [
    ProcessorComputer(1, 1),  # Intel Core i5 в Acer Aspire
    ProcessorComputer(3, 1),  # Intel Core i7 в Acer Aspire
    ProcessorComputer(2, 2),  # AMD Ryzen 7 в Apple MacBook
    ProcessorComputer(4, 2),  # Apple M1 в Apple MacBook
    ProcessorComputer(5, 3),  # AMD Ryzen 5 в ASUS VivoBook
    ProcessorComputer(6, 4),  # Intel Core i3 в Dell Inspiron
    ProcessorComputer(1, 5),  # Intel Core i5 в HP Pavilion
    ProcessorComputer(2, 5)   # AMD Ryzen 7 в HP Pavilion
]

# ============================================================================
# Запрос Г1: Список всех компьютеров, у которых название начинается с буквы «А»,
# и список работающих в них микропроцессоров
# ============================================================================
print("=" * 80)
print("Запрос Г1: Компьютеры на букву 'А' и их микропроцессоры")
print("=" * 80)

computers_with_a = [c for c in computers if c.name.startswith('A')]

if computers_with_a:
    for comp in computers_with_a:
        print(f"\nКомпьютер: {comp.name} (ID: {comp.id})")
        # Находим процессоры через многие-ко-многим связь
        proc_ids = [pc.processor_id for pc in processor_computer if pc.computer_id == comp.id]
        procs = [p for p in processors if p.id in proc_ids]

        if procs:
            for proc in procs:
                print(f"  └─ Процессор: {proc.name}, Частота: {proc.frequency} ГГц")
        else:
            print("  └─ Процессоры не найдены")
else:
    print("Компьютеры, начинающиеся с буквы 'А', не найдены")

# ============================================================================
# Запрос Г2: Список компьютеров с максимальной частотой микропроцессоров
# в каждом компьютере, отсортированный по максимальной частоте (убывание)
# ============================================================================
print("\n" + "=" * 80)
print("Запрос Г2: Компьютеры с максимальной частотой процессоров (отсортировано)")
print("=" * 80)

# Для каждого компьютера находим максимальную частоту процессора
computer_max_freq = []

for comp in computers:
    # Находим процессоры, связанные с этим компьютером
    proc_ids = [pc.processor_id for pc in processor_computer if pc.computer_id == comp.id]
    procs = [p for p in processors if p.id in proc_ids]

    if procs:
        max_freq = max(p.frequency for p in procs)
        computer_max_freq.append((comp.name, max_freq))

# Сортируем по убыванию максимальной частоты
computer_max_freq_sorted = sorted(computer_max_freq, key=lambda x: x[1], reverse=True)

print("\nКомпьютеры, отсортированные по максимальной частоте процессора:")
for comp_name, max_freq in computer_max_freq_sorted:
    print(f"Компьютер: {comp_name:20} Макс. частота: {max_freq} ГГц")

# ============================================================================
# Запрос Г3: Список всех связанных микропроцессоров и компьютеров,
# отсортированный по компьютерам, сортировка по микропроцессорам произвольная
# ============================================================================
print("\n" + "=" * 80)
print("Запрос Г3: Все связи микропроцессоров и компьютеров (отсортировано по компьютерам)")
print("=" * 80)

# Создаем список всех связей (компьютер, процессор, частота)
result_g3 = []
for pc in processor_computer:
    proc = next((p for p in processors if p.id == pc.processor_id), None)
    comp = next((c for c in computers if c.id == pc.computer_id), None)
    if proc and comp:
        result_g3.append((comp.id, comp.name, proc.name, proc.frequency))

# Сортируем по ID компьютера (чтобы было по компьютерам)
result_g3_sorted = sorted(result_g3, key=lambda x: x[0])

print("\nВсе связи процессоров и компьютеров:")
current_comp_id = None
for comp_id, comp_name, proc_name, freq in result_g3_sorted:
    if comp_id != current_comp_id:
        print(f"\nКомпьютер: {comp_name}")
        current_comp_id = comp_id
    print(f"  └─ Процессор: {proc_name:20} Частота: {freq} ГГц")

print("\n" + "=" * 80)
print("Программа завершена успешно")
print("=" * 80)
