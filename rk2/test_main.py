import unittest
from main import (
    computers,
    processors,
    processor_computer,
    query_g1,
    query_g2,
    query_g3,
)


class TestRK2(unittest.TestCase):
    """Тесты для программы РК1 (вариант Г)."""

    def test_query_g1_computers_start_with_a(self):
        """Г1: компьютеры на 'A' и их процессоры."""
        result = query_g1(computers, processors, processor_computer)

        # есть Acer Aspire и ASUS VivoBook
        comp_names = [comp.name for comp, _ in result]
        self.assertIn("Acer Aspire", comp_names)
        self.assertIn("ASUS VivoBook", comp_names)

        # у Acer Aspire два процессора
        acer_procs = next(procs for comp, procs in result
                          if comp.name == "Acer Aspire")
        self.assertEqual(len(acer_procs), 2)

    def test_query_g2_max_frequency_sorted(self):
        """Г2: максимальная частота по компьютерам, сортировка по убыванию."""
        result = query_g2(computers, processors, processor_computer)

        # список не пустой
        self.assertGreater(len(result), 0)

        # первый элемент имеет максимальную частоту 3.6 ГГц
        self.assertEqual(result[0][1], 3.6)

        # у HP Pavilion тоже максимум 3.6 ГГц
        hp_freq = next(freq for name, freq in result
                       if name == "HP Pavilion")
        self.assertEqual(hp_freq, 3.6)

    def test_query_g3_all_links_count_and_first_computer(self):
        """Г3: все связи процессор–компьютер, отсортированы по компьютерам."""
        result = query_g3(computers, processors, processor_computer)

        # количество связей совпадает с количеством элементов в processor_computer
        self.assertEqual(len(result), len(processor_computer))

        # первая группа относится к Acer Aspire
        first_comp_name = result[0][1]
        self.assertEqual(first_comp_name, "Acer Aspire")


if __name__ == "__main__":
    unittest.main()
