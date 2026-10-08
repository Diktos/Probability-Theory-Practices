using System; // дає консольний вивід, базові типи та математичні функції
using System.Collections.Generic; // дає колекцію HashSet для швидкого пошуку дублікатів

namespace Practice2
{
    //  БЛОК 4  
    // колізії сесійних ідентифікаторів у просторі 2^16 (Варіант 4)
    public static class Block4_SessionId
    {
        public static void Run(Random rnd)
        {
            Console.WriteLine("БЛОК 4. Моделювання колізій 16-бітних Session ID (Варіант 4)");

            const int nSpace = 65536; // ємність простору 16-бітних значень: 2^16
            const int k = 200; // кількість сесій, згенерованих в одному пакеті
            const int m = 10000; // кількість тестових пакетів для статистики

            // апроксимаційна формула Тейлора: P = 1 - exp(-k^2 / (2N))
            double pApprox = 1.0 - Math.Exp(-(double)(k * k) / (2.0 * nSpace));

            double pNoCol = 1.0; // база добутку ймовірності відсутності колізій
            for (int i = 0; i < k; i++)
            {
                // точний ланцюговий добуток вільних комірок для кожного наступного значення
                pNoCol *= (double)(nSpace - i) / nSpace;
            }
            double pExact = 1.0 - pNoCol; // перехід до протилежної події (хоча б одна колізія)

            int collisionCount = 0; // лічильник тестових сесій, де знайдено дублікат ключа

            // серія з десяти тисяч тестів
            for (int sim = 0; sim < m; sim++)
            {
                HashSet<int> seen = new HashSet<int>(); // контейнер для перевірки збігів сесійних ключів
                bool hasCol = false; // маркер появи колізії

                // генерація двохсот сесій
                for (int i = 0; i < k; i++)
                {
                    int id = rnd.Next(0, nSpace); // випадковий 16-бітний ключ
                    // якщо ключ уже зустрічався — фіксуємо колізію і виходимо з перевірки поточної сесії
                    if (!seen.Add(id))
                    {
                        hasCol = true;
                        break;
                    }
                }

                if (hasCol) collisionCount++;
            }

            // статистична ймовірність: відношення пакетів з колізіями до загальної кількості тестів
            double wStat = (double)collisionCount / m;

            Console.WriteLine($"Потужність простору: N = {nSpace}, k = {k}, M = {m}");
            Console.WriteLine($"Теоретична ймовірність (апроксимація): P ≈ {pApprox:F5}");
            Console.WriteLine($"Теоретична ймовірність (точна):        P = {pExact:F5}");
            Console.WriteLine($"Статистична частота W(колізія):        W = {wStat:F5}");
            Console.WriteLine($"Похибка відносно апроксимації:         Δ = {Math.Abs(wStat - pApprox):F5}");
            Console.WriteLine($"Похибка відносно точної:               Δ = {Math.Abs(wStat - pExact):F5}\n");
        }
    }
}