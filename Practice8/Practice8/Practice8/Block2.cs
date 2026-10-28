using System; // Математичні операції та вивід у консоль

namespace Practice8
{
    public static class Block2
    {
        //  БЛОК 2 AB ТЕСТУВАННЯ ТА ПАРНА РЕГРЕСІЯ ВАРІАНТ 2 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 2 AB ТЕСТ QUICKSORT VS MERGESORT ТА ПАРНА РЕГРЕСІЯ ВАРІАНТ 2 ");

            int n1 = 60; // Обсяг вибірки для алгоритму A (QuickSort)
            double xBar1 = 12.4; // Середній час виконання QuickSort у мс
            double s1 = 2.1; // Стандартне відхилення QuickSort у мс

            int n2 = 60; // Обсяг вибірки для алгоритму B (MergeSort)
            double xBar2 = 14.1; // Середній час виконання MergeSort у мс
            double s2 = 1.8; // Стандартне відхилення MergeSort у мс

            int df = n1 + n2 - 2; // Кількість ступенів свободи двовибіркового t-тесту
            double sp2 = ((n1 - 1) * s1 * s1 + (n2 - 1) * s2 * s2) / df; // Об'єднана зважена дисперсія двох вибірок
            double sp = Math.Sqrt(sp2); // Об'єднане стандартне відхилення
            double tObs = (xBar1 - xBar2) / Math.Sqrt(sp2 * (1.0 / n1 + 1.0 / n2)); // Спостережуване значення t-критерію Стьюдента
            double tCrit = 1.9803; // Критичне значення t-критерію для рівня 0.05 та df=118
            double pValue = 0.000006; // Двостороннє значення p-value
            double cohenD = (xBar1 - xBar2) / sp; // Розмір ефекту за Коеном

            Console.WriteLine($"A/B тест -> Sp^2={sp2:F4}, Sp={sp:F4}, t_спост={tObs:F4}, df={df}, t_крит={tCrit:F4}, p-value={pValue:F6}, d Коена={cohenD:F4}");

            if (Math.Abs(tObs) > tCrit) // Якщо модуль t більший за критичну межу — різниця статистично значуща
            {
                Console.WriteLine("Висновок A/B тесту -> Відхиляємо H0 (QuickSort працює статистично значуще швидше за MergeSort).");
            }
            else // Інакше різниця випадкова
            {
                Console.WriteLine("Висновок A/B тесту -> Немає підстав відхиляти H0.");
            }

            Random rnd = new Random(42); // Фіксований генератор випадкових чисел із зерном 42
            int nReg = 60; // Кількість замірів для побудови регресії
            double[] xSize = new double[nReg]; // Масив розмірів вхідного масиву X у тис. елементів
            double[] yTime = new double[nReg]; // Масив часу сортування Y у мс
            double sumX = 0.0; // Сума значень X
            double sumY = 0.0; // Сума значень Y

            for (int i = 0; i < nReg; i++) // Генеруємо 60 пар значень (розмір масиву та час виконання)
            {
                xSize[i] = 10.0 + rnd.NextDouble() * 90.0; // Розмір масиву рівномірно від 10 до 100 тис. елементів
                double u1 = 1.0 - rnd.NextDouble(); // Перше випадкове число (0, 1]
                double u2 = 1.0 - rnd.NextDouble(); // Друге випадкове число (0, 1]
                double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // Нормальний шум N(0, 1)
                yTime[i] = 2.5 + 0.18 * xSize[i] + 1.2 * z; // Лінійна залежність часу від розміру з випадковим шумом
                sumX += xSize[i]; // Додаємо X до суми
                sumY += yTime[i]; // Додаємо Y до суми
            }

            double xMean = sumX / nReg; // Середній розмір масиву
            double yMean = sumY / nReg; // Середній час виконання
            double covXY = 0.0; // Сума добутків відхилень X та Y
            double ssX = 0.0; // Сума квадратів відхилень X
            double ssY = 0.0; // Сума квадратів відхилень Y

            for (int i = 0; i < nReg; i++) // Обчислюємо суми для кореляції Пірсона та МНК
            {
                double dx = xSize[i] - xMean; // Відхилення X від середнього
                double dy = yTime[i] - yMean; // Відхилення Y від середнього
                covXY += dx * dy; // Накопичуємо коваріаційну суму
                ssX += dx * dx; // Накопичуємо квадрат відхилення X
                ssY += dy * dy; // Накопичуємо квадрат відхилення Y
            }

            double r = covXY / Math.Sqrt(ssX * ssY); // Лінійний коефіцієнт кореляції Пірсона
            double beta1 = covXY / ssX; // Коефіцієнт нахилу регресійної прямої
            double beta0 = yMean - beta1 * xMean; // Вільний член регресійної прямої
            double r2 = r * r; // Коефіцієнт детермінації R^2

            Console.WriteLine($"Парна регресія (n={nReg}) -> x_bar={xMean:F2}, y_bar={yMean:F2}, r={r:F4}, R^2={r2:F4}");
            Console.WriteLine($"Рівняння регресії -> Y_hat = {beta0:F4} + {beta1:F4} * X");
            Console.WriteLine();
        }
    }
}