using System; // Математичні операції та вивід результатів у консоль

namespace Practice7
{
    public static class Block2
    {
        //  БЛОК 2 ПОБУДОВА ДОВІРЧИХ ІНТЕРВАЛІВ ЗА ВАРІАНТОМ 2 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 2 ДОВІРЧІ ІНТЕРВАЛИ ВАРІАНТ 2 RAM СЕРВЕРА БД ");

            Random rnd = new Random(42); // Фіксований генератор випадкових чисел із зерном 42
            int n = 80; // Обсяг вибірки за варіантом 2
            double meanGiven = 2048.0; // Задане середнє споживання RAM у МБ за варіантом 2
            double sGiven = 210.5; // Задане стандартне відхилення у МБ за варіантом 2

            double[] sample = new double[n]; // Масив для синтетичної вибірки
            double sum = 0.0; // Сума елементів синтетичної вибірки
            for (int i = 0; i < n; i++) // Генеруємо 80 значень за нормальним розподілом N(2048, 210.5^2)
            {
                double u1 = 1.0 - rnd.NextDouble(); // Перше випадкове число (0, 1]
                double u2 = 1.0 - rnd.NextDouble(); // Друге випадкове число (0, 1]
                double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // Перетворення Бокса-Мюллера
                sample[i] = meanGiven + sGiven * z; // Записуємо згенерований обсяг RAM
                sum += sample[i]; // Додаємо до загальної суми
            }

            double xBarSyn = sum / n; // Точкова оцінка середнього за згенерованою вибіркою
            double sumSq = 0.0; // Сума квадратів відхилень від середнього
            for (int i = 0; i < n; i++) // Проходимо по всіх 80 елементах
            {
                double d = sample[i] - xBarSyn; // Відхилення елемента від середнього
                sumSq += d * d; // Додаємо квадрат відхилення
            }
            double s2Syn = sumSq / (n - 1); // Виправлена дисперсія синтетичної вибірки
            double sSyn = Math.Sqrt(s2Syn); // Виправлене стандартне відхилення синтетичної вибірки

            Console.WriteLine($"Синтетична вибірка (n={n}) -> x_bar={xBarSyn:F2}, S^2={s2Syn:F2}, S={sSyn:F2}");

            double t95 = 1.99045; // Квантиль розподілу Стьюдента для надії 0.95 і df=79
            double t99 = 2.63950; // Квантиль розподілу Стьюдента для надії 0.99 і df=79
            double chi95Low = 56.3089; // Нижній квантиль хі-квадрат (0.025, df=79)
            double chi95High = 105.4728; // Верхній квантиль хі-квадрат (0.975, df=79)
            double chi99Low = 50.3761; // Нижній квантиль хі-квадрат (0.005, df=79)
            double chi99High = 115.1166; // Верхній квантиль хі-квадрат (0.995, df=79)

            PrintIntervals("Оцінки за синтетичною вибіркою", n, xBarSyn, sSyn, t95, t99, chi95Low, chi95High, chi99Low, chi99High);
            PrintIntervals("Оцінки за паспортними даними варіанта 2", n, meanGiven, sGiven, t95, t99, chi95Low, chi95High, chi99Low, chi99High);
            Console.WriteLine();
        }

        // Допоміжний метод для обчислення і виводу довірчих інтервалів
        private static void PrintIntervals(string title, int n, double xBar, double s, double t95, double t99, double c95L, double c95H, double c99L, double c99H)
        {
            Console.WriteLine(title);
            double s2 = s * s; // Виправлена дисперсія
            double se = s / Math.Sqrt(n); // Стандартна похибка середнього

            double mu95L = xBar - t95 * se; // Нижня межа 95% інтервалу для середнього
            double mu95H = xBar + t95 * se; // Верхня межа 95% інтервалу для середнього
            double mu99L = xBar - t99 * se; // Нижня межа 99% інтервалу для середнього
            double mu99H = xBar + t99 * se; // Верхня межа 99% інтервалу для середнього

            double var95L = (n - 1) * s2 / c95H; // Нижня межа 95% інтервалу для дисперсії
            double var95H = (n - 1) * s2 / c95L; // Верхня межа 95% інтервалу для дисперсії
            double var99L = (n - 1) * s2 / c99H; // Нижня межа 99% інтервалу для дисперсії
            double var99H = (n - 1) * s2 / c99L; // Верхня межа 99% інтервалу для дисперсії

            Console.WriteLine($"  ДІ для mu (95%) -> [{mu95L:F2}; {mu95H:F2}], ширина={(mu95H - mu95L):F2}");
            Console.WriteLine($"  ДІ для mu (99%) -> [{mu99L:F2}; {mu99H:F2}], ширина={(mu99H - mu99L):F2}");
            Console.WriteLine($"  ДІ для sigma^2 (95%) -> [{var95L:F2}; {var95H:F2}], для sigma -> [{Math.Sqrt(var95L):F2}; {Math.Sqrt(var95H):F2}]");
            Console.WriteLine($"  ДІ для sigma^2 (99%) -> [{var99L:F2}; {var99H:F2}], для sigma -> [{Math.Sqrt(var99L):F2}; {Math.Sqrt(var99H):F2}]");
        }
    }
}