class Computer:
    def __init__(self, id, name):
        self.id = id
        self.name = name


class Processor:
    def __init__(self, id, name, frequency):
        self.id = id
        self.name = name
        self.frequency = frequency  # ГГц


class ProcessorComputer:
    def __init__(self, processor_id, computer_id):
        self.processor_id = processor_id
        self.computer_id = computer_id


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

processor_computer = [
    ProcessorComputer(1, 1),
    ProcessorComputer(3, 1),
    ProcessorComputer(2, 2),
    ProcessorComputer(4, 2),
    ProcessorComputer(5, 3),
    ProcessorComputer(6, 4),
    ProcessorComputer(1, 5),
    ProcessorComputer(2, 5)
]


def query_g1(computers_list, processors_list, links):
    """Компьютеры на 'A' и их процессоры."""
    result = []
    for comp in computers_list:
        if comp.name.startswith("A"):
            proc_ids = [pc.processor_id for pc in links if pc.computer_id == comp.id]
            procs = [p for p in processors_list if p.id in proc_ids]
            result.append((comp, procs))
    return result


def query_g2(computers_list, processors_list, links):
    """Максимальная частота по каждому компьютеру, отсортировано по убыванию частоты."""
    computer_max_freq = []
    for comp in computers_list:
        proc_ids = [pc.processor_id for pc in links if pc.computer_id == comp.id]
        procs = [p for p in processors_list if p.id in proc_ids]
        if procs:
            max_freq = max(p.frequency for p in procs)
            computer_max_freq.append((comp.name, max_freq))
    return sorted(computer_max_freq, key=lambda x: x[1], reverse=True)


def query_g3(computers_list, processors_list, links):
    """Все связи (компьютер, процессор, частота), отсортировано по компьютерам."""
    result = []
    for pc in links:
        proc = next((p for p in processors_list if p.id == pc.processor_id), None)
        comp = next((c for c in computers_list if c.id == pc.computer_id), None)
        if proc and comp:
            result.append((comp.id, comp.name, proc.name, proc.frequency))
    return sorted(result, key=lambda x: x[0])


def main():
    # ===== Г1 =====
    print("=" * 80)
    print("Запрос Г1: Компьютеры на букву 'А' и их микропроцессоры")
    print("=" * 80)

    g1_result = query_g1(computers, processors, processor_computer)
    if g1_result:
        for comp, procs in g1_result:
            print(f"\nКомпьютер: {comp.name} (ID: {comp.id})")
            if procs:
                for proc in procs:
                    print(f"  └─ Процессор: {proc.name}, Частота: {proc.frequency} ГГц")
            else:
                print("  └─ Процессоры не найдены")
    else:
        print("Компьютеры, начинающиеся с буквы 'А', не найдены")

    # ===== Г2 =====
    print("\n" + "=" * 80)
    print("Запрос Г2: Компьютеры с максимальной частотой процессоров (отсортировано)")
    print("=" * 80)

    g2_result = query_g2(computers, processors, processor_computer)
    print("\nКомпьютеры, отсортированные по максимальной частоте процессора:")
    for comp_name, max_freq in g2_result:
        print(f"Компьютер: {comp_name:20} Макс. частота: {max_freq} ГГц")

    # ===== Г3 =====
    print("\n" + "=" * 80)
    print("Запрос Г3: Все связи микропроцессоров и компьютеров (отсортировано по компьютерам)")
    print("=" * 80)

    g3_result = query_g3(computers, processors, processor_computer)
    print("\nВсе связи процессоров и компьютеров:")
    current_comp_id = None
    for comp_id, comp_name, proc_name, freq in g3_result:
        if comp_id != current_comp_id:
            print(f"\nКомпьютер: {comp_name}")
            current_comp_id = comp_id
        print(f"  └─ Процессор: {proc_name:20} Частота: {freq} ГГц")

    print("\n" + "=" * 80)
    print("Программа завершена успешно")
    print("=" * 80)



if __name__ == "__main__":
    main()
