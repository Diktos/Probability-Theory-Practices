using System; // Базові функції консолі, математики та генератор випадкових чисел

namespace Practice3 
{
    public class Block4 
    {
        public static void Run() 
        {
            Console.WriteLine(" БЛОК 4 FAILOVER У КЛАСТЕРІ БД ВАРІАНТ 4 "); 

            Random rand = new Random(42); 
            int n = 100000; // Кількість сесій для моделювання 

            double pMain = 0.95; // Надійність основної бази даних (p1)
            double pSwitch = 0.90; // Ймовірність успішного спрацювання перемикача Failover switch (ps)
            double pBackup = 0.90; // Надійність резервної бази даних (p2)

            double theoSuccess = pMain + (1.0 - pMain) * pSwitch * pBackup; // Теоретична ймовірність успіху = 0.95 + 0.05*0.90*0.90 = 0.9905
            int successCount = 0; // Лічильник сесій, які завершилися успішною роботою БД

            for (int i = 0; i < n; i++) // Цикл симуляції 100 000 робочих сесій кластера
            {
                bool mainOk = rand.NextDouble() < pMain; // Перевіряємо, чи витримала навантаження основна БД

                if (mainOk) // Якщо основна БД працює справно — сесія одразу успішна
                {
                    successCount++; 
                }
                else // Інакше основна БД впала — запускаємо аварійне перемикання(резервне)
                {
                    bool switchOk = rand.NextDouble() < pSwitch; // Перевіряємо, чи спрацював автоматичний перемикач
                    bool backupOk = rand.NextDouble() < pBackup; // Перевіряємо, чи справна резервна база даних

                    if (switchOk && backupOk) // Якщо і перемикач спрацював, і резервна БД жива — сесію врятовано
                    {
                        successCount++; 
                    }
                }
            }

            double wSuccess = (double)successCount / n; // Статистична частота успішної роботи системи
            double wFail = 1.0 - wSuccess; // Статистична частота повної відмови кластера
            double error = Math.Abs(wSuccess - theoSuccess); // Абсолютна похибка між статистикою та теорією

            Console.WriteLine($"Успіх (теорія): {theoSuccess:F5}"); // Виводимо теоретичну надійність кластера
            Console.WriteLine($"Успіх (стат):   {wSuccess:F5}"); // Виводимо статистичну частоту успіху
            Console.WriteLine($"Відмова (стат): {wFail:F5}"); // Виводимо частку повних відмов кластера
            Console.WriteLine($"Похибка:        {error:F5}"); // Виводимо абсолютну похибку моделювання
        }
    }
}