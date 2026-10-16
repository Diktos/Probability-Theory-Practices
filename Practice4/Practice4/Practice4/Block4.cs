using System; 

namespace Practice4
{
    public static class Block4
    {
        public static void Run()
        {
            Console.WriteLine(" БЛОК 4 ВИЗНАЧЕННЯ КАТЕГОРІЇ ТОВАРУ ВАРІАНТ 9 ");

            Random rnd = new Random(42); 
            int n = 100000; 

            double pElectronics = 0.30; // Початкова частка товарів категорії Electronics
            double pBooks = 0.70; // Початкова частка товарів категорії Books
            double pPython_Elec = 0.10; // Шанс зустріти слово Python в електроніці
            double pPython_Books = 0.20; // Шанс зустріти слово Python у книгах

            double pPython_theo = pElectronics * pPython_Elec + pBooks * pPython_Books; // Повна ймовірність слова Python (0.17)
            double pBooks_Python_theo = (pBooks * pPython_Books) / pPython_theo; // Теоретичний шанс книги за Байєсом (0.82353)

            int countPython = 0; // Лічильник запитів, де є слово Python
            int countBooksAndPython = 0; // Лічильник запитів зі словом Python, які стосувалися книг

            for (int i = 0; i < n; i++) // Цикл моделювання 100 000 пошукових запитів
            {
                bool isBooks = rnd.NextDouble() < pBooks; // Визначаємо реальну категорію запиту (70% книги, 30% електроніка)
                double pWord = isBooks ? pPython_Books : pPython_Elec; // Обираємо шанс появи слова Python для категорії
                bool hasPython = rnd.NextDouble() < pWord; // Перевіряємо чи ввів користувач слово Python

                if (hasPython) 
                {
                    countPython++; // Збільшуємо загальну кількість запитів зі словом Python
                    if (isBooks) // Якщо при цьому шукали саме книгу — плюсуємо успішну гіпотезу
                    {
                        countBooksAndPython++; // Збільшуємо лічильник книг зі словом Python
                    }
                }
            }

            double wPython = (double)countPython / n; // Статистична частота появи слова Python
            double wBooks_Python = (double)countBooksAndPython / countPython; // Статистична оцінка P(Books|Python)
            double error = Math.Abs(wBooks_Python - pBooks_Python_theo); // Абсолютна похибка симуляції

            Console.WriteLine($"P(Books|Python) теорія: {pBooks_Python_theo:F5}");
            Console.WriteLine($"W(Books|Python) стат:   {wBooks_Python:F5}");
            Console.WriteLine($"Частота слова Python:   {wPython:F5}");
            Console.WriteLine($"Похибка:                {error:F5}");
            Console.WriteLine();
        }
    }
}