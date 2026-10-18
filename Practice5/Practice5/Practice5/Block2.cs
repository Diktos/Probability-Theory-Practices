using System; 

namespace Practice5
{
    public static class Block2
    {
        public static void Run()
        {
            Console.WriteLine(" БЛОК 2 MTBF СЕРВЕРА ПОКАЗНИКОВИЙ ЗАКОН ВАРІАНТ 2 ");

            Random rnd = new Random(42); 
            int n = 100000; 
            double lambda = 0.05; // Інтенсивність відмов сервера (0.05 відмов за годину)

            double meanTheo = 1.0 / lambda; // Теоретичне математичне сподівання(арифметичне) M[X] = 20 годин
            double varTheo = 1.0 / (lambda * lambda); // Теоретична дисперсія D[X] = 400 год^2
            double stdTheo = Math.Sqrt(varTheo); // Теоретичне середньоквадратичне відхилення = 20 годин
            double pGt30Theo = Math.Exp(-lambda * 30.0); // Теоретична ймовірність P(X > 30) = e^(-1.5)

            double[] samples = new double[n]; // Масив для збереження часу роботи сервера в кожному тесті
            double sum1 = 0.0; // Сума всіх значень для розрахунку вибіркового середнього
            int countGt30 = 0; // Лічильник випадків, коли сервер пропрацював більше 30 годин

            for (int i = 0; i < n; i++) // Цикл моделювання 100 000 запусків сервера
            {
                double u = rnd.NextDouble(); // Базове рівномірне число від 0 до 1
                double x = -Math.Log(1.0 - u) / lambda; // Формула оберненої функції для показникового розподілу
                samples[i] = x; // Записуємо час до відмови у масив
                sum1 += x; // Додаємо час до загальної суми

                if (x > 30.0) // Якщо сервер протримався довше 30 годин — фіксуємо успіх
                {
                    countGt30++; // Збільшуємо лічильник роботи понад 30 годин
                }
            }

            double meanStat = sum1 / n; // Вибіркове середнє M[X]
            double sum2 = 0.0; // Накопичувач квадратів відхилень для вибіркової дисперсії

            for (int i = 0; i < n; i++) // Цикл обчислення розкиду значень навколо середнього
            {
                double diff = samples[i] - meanStat; // Різниця між поточним часом і середнім
                sum2 += diff * diff; // Додаємо квадрат різниці
            }

            double varStat = sum2 / n; // Вибіркова дисперсія D[X]
            double stdStat = Math.Sqrt(varStat); // Вибіркове середньоквадратичне відхилення sigma[X]
            double pGt30Stat = (double)countGt30 / n; // Статистична частота події X > 30

            Console.WriteLine($"M[X]:     стат={meanStat:F5}, теор={meanTheo:F5}, похибка={Math.Abs(meanStat - meanTheo):F5}"); // Виводимо статистичне та теоретичне значення математичного сподівання
            Console.WriteLine($"D[X]:     стат={varStat:F5}, теор={varTheo:F5}, похибка={Math.Abs(varStat - varTheo):F5}"); // Виводимо статистичне та теоретичне значення дисперсії
            Console.WriteLine($"sigma[X]: стат={stdStat:F5}, теор={stdTheo:F5}, похибка={Math.Abs(stdStat - stdTheo):F5}"); // Виводимо статистичне та теоретичне значення середньоквадратичного відхилення
            Console.WriteLine($"P(X>30):  стат={pGt30Stat:F5}, теор={pGt30Theo:F5}, похибка={Math.Abs(pGt30Stat - pGt30Theo):F5}"); // Виводимо статистичну та теоретичну ймовірність події що сервер пропрацює понад 30 годин
            Console.WriteLine();
        }
    }
}