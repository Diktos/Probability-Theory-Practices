using System; // Базові математичні операції та робота з консоллю

namespace Practice6
{
    public static class Block2
    {
        //  БЛОК 2 ІНТЕРВАЛЬНЕ ГРУПУВАННЯ ЗА ВАРІАНТОМ 2 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 2 ІНТЕРВАЛЬНЕ ГРУПУВАННЯ ВАРІАНТ 2 ");

            Random rnd = new Random(42); // Генератор випадкових чисел із фіксованим зерном 42
            int n = 200; // Обсяг великої вибірки за умовою
            double mu = 1024.0; // Математичне сподівання використання RAM у МБ за варіантом 2
            double sigma = 128.0; // Стандартне відхилення використання RAM у МБ за варіантом 2

            double[] sample = new double[n]; // Масив для збереження згенерованої вибірки
            for (int i = 0; i < n; i++) // Генеруємо 200 значень за нормальним розподілом
            {
                double u1 = 1.0 - rnd.NextDouble(); // Перше рівномірне число від 0 до 1 без нуля
                double u2 = 1.0 - rnd.NextDouble(); // Друге рівномірне число від 0 до 1 без нуля
                double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // Формула Бокса-Мюллера для N(0,1)
                sample[i] = mu + sigma * z; // Перехід до потрібного нормального розподілу N(1024, 128^2)
            }

            Array.Sort(sample); // Сортуємо вибірку для пошуку мінімуму і максимуму
            double xMin = sample[0]; // Мінімальне значення вибірки
            double xMax = sample[n - 1]; // Максимальне значення вибірки

            int k = 1 + (int)Math.Floor(3.322 * Math.Log10(n)); // Кількість інтервалів за формулою Стерджеса
            double h = (xMax - xMin) / k; // Ширина (крок) одного інтервалу

            Console.WriteLine($"Min={xMin:F2}, Max={xMax:F2}, Інтервалів k={k}, Крок h={h:F4}");

            double sumRaw = 0.0; // Сума елементів незгрупованої вибірки
            for (int i = 0; i < n; i++) // Додаємо всі 200 значень
            {
                sumRaw += sample[i]; // Накопичуємо суму
            }
            double meanRaw = sumRaw / n; // Точне середнє незгрупованої вибірки

            double sumSqRaw = 0.0; // Сума квадратів відхилень незгрупованої вибірки
            for (int i = 0; i < n; i++) // Проходимо по кожному елементу
            {
                double d = sample[i] - meanRaw; // Відхилення елемента від середнього
                sumSqRaw += d * d; // Додаємо квадрат відхилення
            }
            double s2Raw = sumSqRaw / (n - 1); // Виправлена дисперсія незгрупованої вибірки
            double sRaw = Math.Sqrt(s2Raw); // Стандартне відхилення незгрупованої вибірки

            double[] mids = new double[k]; // Масив середин інтервалів
            int[] counts = new int[k]; // Масив абсолютних частот інтервалів

            for (int i = 0; i < k; i++) // Формуємо кожен інтервал і рахуємо скільки чисел туди потрапило
            {
                double left = xMin + i * h; // Ліва межа інтервалу
                double right = xMin + (i + 1) * h; // Права межа інтервалу
                mids[i] = (left + right) / 2.0; // Середина поточного інтервалу

                int cnt = 0; // Лічильник елементів у поточному інтервалі
                for (int j = 0; j < n; j++) // Перевіряємо всі елементи вибірки
                {
                    if (i < k - 1) // Якщо це не останній інтервал — права межа не включно
                    {
                        if (sample[j] >= left && sample[j] < right) // Перевірка потрапляння в [left, right)
                        {
                            cnt++; // Збільшуємо частоту інтервалу
                        }
                    }
                    else // Якщо це останній інтервал — включаємо праву межу щоб не загубити максимум
                    {
                        if (sample[j] >= left && sample[j] <= right + 1e-9) // Перевірка потрапляння в [left, right]
                        {
                            cnt++; // Збільшуємо частоту останнього інтервалу
                        }
                    }
                }
                counts[i] = cnt; // Зберігаємо частоту інтервалу
                double wi = (double)cnt / n; // Відносна частота інтервалу
                double pi = wi / h; // Щільність відносної частоти для гістограми

                Console.WriteLine($"[{left,7:F2}; {right,7:F2}) | mid={mids[i],7:F2} | n={cnt,3} | w={wi:F4} | p={pi:F6}");
            }

            double sumGrp = 0.0; // Сума добутків частот на середини інтервалів
            for (int i = 0; i < k; i++) // Проходимо по всіх інтервалах
            {
                sumGrp += counts[i] * mids[i]; // Додаємо вагу інтервалу
            }
            double meanGrp = sumGrp / n; // Згруповане вибіркове середнє

            double sumSqGrp = 0.0; // Сума квадратів відхилень для згрупованого ряду
            for (int i = 0; i < k; i++) // Рахуємо внесок кожного інтервалу в дисперсію
            {
                double diff = mids[i] - meanGrp; // Відхилення середини інтервалу від згрупованого середнього
                sumSqGrp += counts[i] * diff * diff; // Множимо квадрат відхилення на частоту інтервалу
            }
            double s2Grp = sumSqGrp / (n - 1); // Згрупована виправлена дисперсія

            double errMean = Math.Abs(meanRaw - meanGrp); // Абсолютна похибка середнього через групування
            double errS2 = Math.Abs(s2Raw - s2Grp); // Абсолютна похибка дисперсії через групування

            Console.WriteLine($"Незгруповані оцінки -> x_bar={meanRaw:F4}, S^2={s2Raw:F4}, S={sRaw:F4}");
            Console.WriteLine($"Згруповані оцінки   -> x_bar_grp={meanGrp:F4}, S^2_grp={s2Grp:F4}");
            Console.WriteLine($"Похибка групування  -> d_x={errMean:F4} ({errMean / meanRaw * 100.0:F2}%), d_S^2={errS2:F4} ({errS2 / s2Raw * 100.0:F2}%)");
            Console.WriteLine();
        }
    }
}