using System; 

namespace Practice4
{
    public static class Block2
    {
        public static void Run()
        {
            Console.WriteLine(" БЛОК 2 ДЕТЕКЦІЯ БОТІВ ЗА КОРОТКИМИ ВІДГУКАМИ ВАРІАНТ 5 ");

            Random rnd = new Random(42); 
            int n = 100000; 

            double pH1 = 0.85; // Початкова частка реальних користувачів
            double pH2 = 0.15; // Початкова частка ботів
            double pA_H1 = 0.10; // Ймовірність короткого відгуку від живої людини
            double pA_H2 = 0.80; // Ймовірність короткого відгуку від бота

            double pA_theo = pH1 * pA_H1 + pH2 * pA_H2; // Повна ймовірність короткого відгуку (0.205)
            double pH2_A_theo = (pH2 * pA_H2) / pA_theo; // Теоретичний шанс бота за формулою Байєса (0.58537)

            int countShort = 0; // Лічильник усіх коротких відгуків
            int countBotAndShort = 0; // Лічильник коротких відгуків, залишених саме ботами

            for (int i = 0; i < n; i++) // Цикл перевірки 100 000 згенерованих відгуків
            {
                bool isBot = rnd.NextDouble() < pH2; // Якщо число менше 0.15 — відгук пише бот, інакше людина
                double pShort = isBot ? pA_H2 : pA_H1; // Обираємо шанс короткого тексту (0.80 для бота, 0.10 для людини)
                bool isShort = rnd.NextDouble() < pShort; // Перевіряємо чи вийшов відгук коротким

                if (isShort) // Якщо відгук виявився коротким — рахуємо його
                {
                    countShort++; // Збільшуємо кількість коротких відгуків
                    if (isBot) // Якщо цей короткий відгук написав бот — фіксуємо одиничку
                    {
                        countBotAndShort++; // Збільшуємо лічильник ботів серед коротких відгуків
                    }
                }
            }

            double wA = (double)countShort / n; // Статистична частка коротких відгуків
            double wH2_A = (double)countBotAndShort / countShort; // Статистична частка ботів серед коротких відгуків
            double errA = Math.Abs(wA - pA_theo); // Абсолютна похибка для повної ймовірності
            double errH2 = Math.Abs(wH2_A - pH2_A_theo); // Абсолютна похибка для формули Байєса

            Console.WriteLine($"P(A) коротка довжина: стат={wA:F5}, теор={pA_theo:F5}, похибка={errA:F5}");
            Console.WriteLine($"P(H2|A) автор бот:    стат={wH2_A:F5}, теор={pH2_A_theo:F5}, похибка={errH2:F5}");
            Console.WriteLine();
        }
    }
}