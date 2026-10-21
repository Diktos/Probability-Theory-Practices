using System; // Базові математичні функції та вивід у консоль

namespace Practice6
{
    public static class Block1
    {
        //  БЛОК 1 ОБРОБКА НЕЗГРУПОВАНОЇ ВИБІРКИ ТА ECDF 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 1 ОБРОБКА НЕЗГРУПОВАНОЇ ВИБІРКИ ТА ECDF ");

            double[] xRaw = {
                23.4, 18.2, 25.1, 21.0, 18.2, 29.5, 23.4, 21.0, 27.8, 18.2,
                23.4, 25.1, 31.0, 21.0, 23.4, 19.8, 27.8, 23.4, 21.0, 25.1
            }; // Початковий масив затримок виконання функції в мс

            int n = xRaw.Length; // Обсяг вибірки (кількість замірів)
            double[] sorted = new double[n]; // Масив для ранжованого ряду
            Array.Copy(xRaw, sorted, n); // Копіюємо вихідні дані щоб не псувати оригінал
            Array.Sort(sorted); // Сортуємо елементи за зростанням

            Console.WriteLine("Ранжований ряд " + string.Join("; ", sorted));

            double[] uniqueVals = new double[n]; // Масив для збереження унікальних значень
            int[] counts = new int[n]; // Масив абсолютних частот для кожного значення
            int uniqueCount = 0; // Лічильник кількості унікальних елементів

            for (int i = 0; i < n; i++) // Проходимо по всіх елементах відсортованого ряду
            {
                if (uniqueCount == 0 || sorted[i] != uniqueVals[uniqueCount - 1]) // Якщо зустріли нове унікальне число — записуємо його в таблицю
                {
                    uniqueVals[uniqueCount] = sorted[i]; // Фіксуємо нове значення xi
                    counts[uniqueCount] = 1; // Встановлюємо початкову частоту 1
                    uniqueCount++; // Збільшуємо кількість унікальних значень
                }
                else // Якщо число повторюється — збільшуємо його частоту
                {
                    counts[uniqueCount - 1]++; // Додаємо одиницю до частоти поточного елемента
                }
            }

            int cumulative = 0; // Накопичена частота для розрахунку ECDF
            int maxFreq = 0; // Максимальна частота для пошуку моди
            double mode = sorted[0]; // Значення моди (найчастіший елемент)

            Console.WriteLine("Статистичний розподіл та ECDF");
            for (int i = 0; i < uniqueCount; i++) // Виводимо таблицю частот та емпіричну функцію
            {
                double wi = (double)counts[i] / n; // Відносна частота (частка від загальної кількості)
                double fStarBefore = (double)cumulative / n; // Значення F*(x) для елементів строго менших за поточний
                cumulative += counts[i]; // Додаємо поточну частоту до накопиченої суми
                double fStarAfter = (double)cumulative / n; // Значення F*(x) після стрибка в точці xi

                if (counts[i] > maxFreq) // Якщо поточна частота більша за попередній максимум — оновлюємо моду
                {
                    maxFreq = counts[i]; // Запам'ятовуємо нову найбільшу частоту
                    mode = uniqueVals[i]; // Запам'ятовуємо значення моди
                }

                Console.WriteLine($"x={uniqueVals[i],5:F1} | n={counts[i],2} | w={wi:F4} | F*(x)={fStarBefore:F4} -> {fStarAfter:F4}");
            }

            double sum = 0.0; // Сума всіх елементів для обчислення середнього
            for (int i = 0; i < n; i++) // Додаємо всі заміри вибірки
            {
                sum += sorted[i]; // Накопичуємо загальну суму
            }
            double mean = sum / n; // Вибіркове середнє арифметичне

            double sumSqDiff = 0.0; // Сума квадратів відхилень від середнього
            for (int i = 0; i < n; i++) // Рахуємо відхилення для кожного елемента
            {
                double diff = sorted[i] - mean; // Різниця між елементом і середнім
                sumSqDiff += diff * diff; // Додаємо квадрат різниці
            }

            double s2Biased = sumSqDiff / n; // Вибіркова зміщена дисперсія
            double s2Unbiased = sumSqDiff / (n - 1); // Виправлена незміщена дисперсія
            double sUnbiased = Math.Sqrt(s2Unbiased); // Виправлене середнє квадратичне відхилення

            double median; // Змінна для медіани (середини ряду)
            if (n % 2 == 0) // Якщо кількість елементів парна — беремо півсуму двох центральних
            {
                median = (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0; // Середнє двох центральних чисел
            }
            else // Якщо кількість непарна — беремо центральний елемент
            {
                median = sorted[n / 2]; // Центральне число ряду
            }

            double range = sorted[n - 1] - sorted[0]; // Розмах вибірки (різниця між максимумом і мінімумом)
            double cv = (sUnbiased / mean) * 100.0; // Коефіцієнт варіації у відсотках

            Console.WriteLine($"x_bar={mean:F4}, s^2={s2Biased:F4}, S^2={s2Unbiased:F4}, S={sUnbiased:F4}");
            Console.WriteLine($"Медіана={median:F4}, Мода={mode:F1}, Розмах R={range:F4}, Коеф. варіації V={cv:F2}%");
            Console.WriteLine();
        }
    }
}