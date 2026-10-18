using System; 

namespace Practice5
{
    public static class Block1
    {
        public static void Run()
        {
            Console.WriteLine(" БЛОК 1 ПРОГРАМНА ПЕРЕВІРКА ЦЕНТРАЛЬНОЇ ГРАНИЧНОЇ ТЕОРЕМИ ");

            Random rnd = new Random(42); 
            int n = 100000; 
            int[] ks = { 1, 2, 5, 12, 30 }; // Кількість незалежних доданків у сумі за завданням

            foreach (int k in ks) // Перебираємо кожне значення кількості доданків k
            {
                double[] zValues = new double[n]; // Масив для збереження сум 
                double sum1 = 0.0; // Накопичувач суми значень для обчислення середнього

                for (int i = 0; i < n; i++) // Генеруємо 100 000 значень випадкової множини Z_k
                {
                    double s = 0.0; // Змінна для накопичення суми k рівномірних чисел
                    for (int j = 0; j < k; j++) // Додаємо k незалежних випадкових величин U(0,1)
                    {
                        s += rnd.NextDouble(); // Додаємо чергове випадкове число від 0 до 1
                    }

                    double z = (s - 0.5 * k) / Math.Sqrt(k / 12.0); // Центруємо та нормуємо суму за формулою ЦГТ
                    zValues[i] = z; // Зберігаємо отримане значення в масив
                    sum1 += z; // Додаємо значення до загальної суми
                }

                double mean = sum1 / n; // Вибіркове математичне середнє арифметичне
                double sum2 = 0.0; // Накопичувач квадратів відхилень для дисперсії
                double sum3 = 0.0; // Накопичувач кубів відхилень для асиметрії
                double sum4 = 0.0; // Накопичувач четвертих степенів для ексцесу

                for (int i = 0; i < n; i++) // Другий прохід по масиву для розрахунку моментів
                {
                    double diff = zValues[i] - mean; // Відхилення конкретного значення від середнього
                    double diff2 = diff * diff; // Квадрат відхилення
                    sum2 += diff2; // Додаємо у суму для дисперсії
                    sum3 += diff2 * diff; // Додаємо куб відхилення для асиметрії
                    sum4 += diff2 * diff2; // Додаємо четвертий степінь для ексцесу
                }

                double variance = sum2 / n; // Вибіркова дисперсія D[Z_k]
                double stdDev = Math.Sqrt(variance); // Середньоквадратичне відхилення sigma[Z_k]
                double skewness = (sum3 / n) / (stdDev * stdDev * stdDev); // Коефіцієнт асиметрії (скошеність)
                double kurtosis = (sum4 / n) / (variance * variance) - 3.0; // Коефіцієнт ексцесу (гостроверхість)

                Console.WriteLine($"k={k,2}: M[Z]={mean,8:F5}, D[Z]={variance:F5}, Skewness={skewness,8:F5}, Kurtosis={kurtosis,8:F5}");
            }
            Console.WriteLine();
        }
    }
}