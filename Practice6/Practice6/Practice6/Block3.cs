using System; // Математичні функції та консольний вивід

namespace Practice6
{
    public static class Block3
    {
        //  БЛОК 3 ДЕТЕКЦІЯ ТА ФІЛЬТРАЦІЯ АНОМАЛІЙ OUTLIERS 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 3 ДЕТЕКЦІЯ ТА ФІЛЬТРАЦІЯ АНОМАЛІЙ ");

            Random rnd = new Random(42); // Фіксуємо зерно 42 для відтворення вибірки з Блоку 2
            int nBase = 200; // Обсяг базової вибірки
            double mu = 1024.0; // Середнє використання RAM у МБ
            double sigma = 128.0; // Стандартне відхилення у МБ

            double[] baseSample = new double[nBase]; // Масив для базової вибірки без викидів
            double sumBase = 0.0; // Сума елементів базової вибірки
            for (int i = 0; i < nBase; i++) // Генеруємо 200 базових замірів
            {
                double u1 = 1.0 - rnd.NextDouble(); // Перше випадкове число (0, 1]
                double u2 = 1.0 - rnd.NextDouble(); // Друге випадкове число (0, 1]
                double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // Перетворення Бокса-Мюллера
                baseSample[i] = mu + sigma * z; // Записуємо нормальну величину
                sumBase += baseSample[i]; // Додаємо до суми
            }

            Array.Sort(baseSample); // Сортуємо базову вибірку
            double meanBase = sumBase / nBase; // Середнє базової вибірки
            double sqBase = 0.0; // Сума квадратів відхилень базової вибірки
            for (int i = 0; i < nBase; i++) // Рахуємо розкид базової вибірки
            {
                double d = baseSample[i] - meanBase; // Відхилення від середнього
                sqBase += d * d; // Накопичуємо квадрат відхилення
            }
            double sBase = Math.Sqrt(sqBase / (nBase - 1)); // Стандартне відхилення базової вибірки
            double medBase = GetPercentile(baseSample, 0.50); // Медіана базової вибірки

            double threshold = meanBase + 5.0 * sBase; // Нижня межа для генерації аномальних викидів x_bar + 5S
            int nDirty = nBase + 10; // Новий обсяг вибірки разом із 10 викидами
            double[] dirtySample = new double[nDirty]; // Масив для спотвореної вибірки
            Array.Copy(baseSample, dirtySample, nBase); // Копіюємо 200 нормальних значень

            for (int i = 0; i < 10; i++) // Додаємо 10 екстремальних збоїв пам'яті понад поріг
            {
                dirtySample[nBase + i] = threshold + 50.0 + rnd.NextDouble() * 300.0; // Генеруємо аномальне значення
            }

            Array.Sort(dirtySample); // Сортуємо спотворену вибірку за зростанням
            double sumDirty = 0.0; // Сума елементів спотвореної вибірки
            for (int i = 0; i < nDirty; i++) // Сумуємо всі 210 значень
            {
                sumDirty += dirtySample[i]; // Додаємо елемент
            }
            double meanDirty = sumDirty / nDirty; // Середнє спотвореної вибірки

            double sqDirty = 0.0; // Сума квадратів відхилень спотвореної вибірки
            for (int i = 0; i < nDirty; i++) // Проходимо по всіх 210 значеннях
            {
                double d = dirtySample[i] - meanDirty; // Відхилення від спотвореного середнього
                sqDirty += d * d; // Додаємо квадрат відхилення
            }
            double sDirty = Math.Sqrt(sqDirty / (nDirty - 1)); // Стандартне відхилення спотвореної вибірки
            double medDirty = GetPercentile(dirtySample, 0.50); // Медіана спотвореної вибірки
            double q1 = GetPercentile(dirtySample, 0.25); // Перший квартиль (25-й перцентиль)
            double q3 = GetPercentile(dirtySample, 0.75); // Третій квартиль (75-й перцентиль)
            double iqr = q3 - q1; // Міжквартильний розмах IQR

            double lowFence = q1 - 1.5 * iqr; // Нижня допустима межа за правилом Boxplot
            double highFence = q3 + 1.5 * iqr; // Верхня допустима межа за правилом Boxplot

            double[] tempClean = new double[nDirty]; // Тимчасовий масив для очищених даних
            int nClean = 0; // Лічильник нормальних елементів після фільтрації
            int outliersCount = 0; // Лічильник виявлених аномалій

            for (int i = 0; i < nDirty; i++) // Перевіряємо кожен елемент на вихід за межі Boxplot
            {
                if (dirtySample[i] >= lowFence && dirtySample[i] <= highFence) // Якщо значення в межах норми — залишаємо
                {
                    tempClean[nClean] = dirtySample[i]; // Записуємо в очищений масив
                    nClean++; // Збільшуємо кількість чистих елементів
                }
                else // Якщо значення вийшло за паркан Boxplot — фіксуємо аномалію
                {
                    outliersCount++; // Рахуємо знайдений викид
                }
            }

            double[] cleanSample = new double[nClean]; // Фінальний масив очищеної вибірки
            Array.Copy(tempClean, cleanSample, nClean); // Копіюємо лише чисті значення

            double sumClean = 0.0; // Сума очищеної вибірки
            for (int i = 0; i < nClean; i++) // Додаємо всі очищені елементи
            {
                sumClean += cleanSample[i]; // Накопичуємо суму
            }
            double meanClean = sumClean / nClean; // Очищене вибіркове середнє

            double sqClean = 0.0; // Сума квадратів відхилень очищеної вибірки
            for (int i = 0; i < nClean; i++) // Рахуємо розкид після очищення
            {
                double d = cleanSample[i] - meanClean; // Відхилення від очищеного середнього
                sqClean += d * d; // Додаємо квадрат відхилення
            }
            double sClean = Math.Sqrt(sqClean / (nClean - 1)); // Очищене стандартне відхилення
            double medClean = GetPercentile(cleanSample, 0.50); // Очищена медіана

            double meanShiftPct = Math.Abs(meanDirty - meanBase) / meanBase * 100.0; // Відносне зміщення середнього через викиди
            double medShiftPct = Math.Abs(medDirty - medBase) / medBase * 100.0; // Відносне зміщення медіани через викиди

            Console.WriteLine($"Поріг аномалій x_bar + 5S = {threshold:F2} МБ");
            Console.WriteLine($"Спотворена вибірка (n={nDirty}) -> x_bar={meanDirty:F2}, S={sDirty:F2}, Медіана={medDirty:F2}, IQR={iqr:F2}");
            Console.WriteLine($"Квартилі та межі Boxplot -> Q1={q1:F2}, Q3={q3:F2}, Межі=[{lowFence:F2}; {highFence:F2}], Виявлено викидів={outliersCount}");
            Console.WriteLine($"Очищена вибірка (n={nClean})    -> x_bar_clean={meanClean:F2}, S_clean={sClean:F2}, Медіана_clean={medClean:F2}");
            Console.WriteLine($"Зміщення від викидів -> Середнє змінилось на {meanShiftPct:F2}%, Медіана змінилась лише на {medShiftPct:F2}%");
            Console.WriteLine();
        }

        // Допоміжний метод лінійної інтерполяції перцентиля для відсортованого масиву
        private static double GetPercentile(double[] sorted, double p)
        {
            double pos = p * (sorted.Length - 1); // Точна позиція індексу в масиві
            int lo = (int)Math.Floor(pos); // Нижній цілий індекс
            int hi = (int)Math.Ceiling(pos); // Верхній цілий індекс
            double frac = pos - lo; // Дробова частина між сусідніми елементами
            return sorted[lo] + frac * (sorted[hi] - sorted[lo]); // Проміжне значення за пропорцією
        }
    }
}