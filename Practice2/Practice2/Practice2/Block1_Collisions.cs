using System; // дає консольний вивід, базові типи та математичні функції
using System.Collections.Generic; // дає колекцію HashSet для швидкого пошуку дублікатів

namespace Practice2
{
    //  БЛОК 1  
    public static class Block1_Collisions
    {
        public static void Run(Random rnd)
        {
            const int n = 365; // ємність простору (аналог днів у році)
            const int m = 10000; // число повторень досліду для закону великих чисел
            int[] kValues = { 5, 10, 15, 20, 23, 30, 40, 50, 60, 70, 80 }; // розміри вибірок для аналізу перегину ймовірності

            Console.WriteLine("БЛОК 1. Дослідження колізій та ЗВЧ (N = 365, M = 10000)");
            Console.WriteLine($"{"k",4} | {"P(колізія) теор",16} | {"W(колізія) стат",16} | {"Похибка Δ",12}");
            Console.WriteLine(new string('-', 55));

            // перебір кожного розміру вибірки зі списку
            foreach (int k in kValues)
            {
                double pNoCollision = 1.0; // база добутку для події без збігів

                // множимо частки вільних комірок для кожного наступного кроку
                for (int i = 0; i < k; i++)
                {
                    pNoCollision *= (double)(n - i) / n; // теорема множення ймовірностей незалежних кроків
                }

                double pCollision = 1.0 - pNoCollision; // теорема протилежної події

                int collisionCount = 0; // лічильник випадків, коли знайшли хоча б один дублікат

                // серія з десяти тисяч симуляцій
                for (int sim = 0; sim < m; sim++)
                {
                    HashSet<int> seen = new HashSet<int>(); // контейнер перевірених унікальних значень
                    bool hasCollision = false; // маркер фіксації збігу

                    // генерація випадкових значень для поточної групи
                    for (int i = 0; i < k; i++)
                    {
                        int day = rnd.Next(0, n); // рівномірне випадкове число від 0 до n-1

                        // якщо число вже випадало раніше — фіксуємо колізію і достроково зупиняємо цикл
                        if (!seen.Add(day))
                        {
                            hasCollision = true;
                            break;
                        }
                    }

                    // фіксуємо факт колізії для поточної серії
                    if (hasCollision) collisionCount++;
                }

                double wCollision = (double)collisionCount / m; // статистична частота події за формулою Бернуллі
                double delta = Math.Abs(wCollision - pCollision); // модуль абсолютної похибки між практикою і теорією

                Console.WriteLine($"{k,4} | {pCollision,16:F5} | {wCollision,16:F5} | {delta,12:F5}");
            }
            Console.WriteLine();
        }
    }
}