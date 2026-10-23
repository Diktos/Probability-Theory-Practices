using System; // Математичні функції та консольний вивід

namespace Practice7
{
    public static class Block1
    {
        //  БЛОК 1 ПОРІВНЯННЯ МЕТОДІВ ТОЧКОВОГО ОЦІНЮВАННЯ MOM ТА MLE 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 1 ПОРІВНЯННЯ МЕТОДІВ MOM ТА MLE ");

            Random rnd = new Random(42); // Генератор випадкових чисел із фіксованим зерном 42
            double lambdaTrue = 0.04; // Справжній параметр показникового розподілу (істинне середнє 25 мс)
            int mRepeats = 1000; // Кількість повторних вибірок для кожного обсягу n
            int[] nValues = { 5, 10, 20, 50, 100, 500, 1000, 5000, 10000 }; // Масив обсягів вибірки за умовою

            for (int idx = 0; idx < nValues.Length; idx++) // Проходимо по кожному обсягу вибірки n
            {
                int n = nValues[idx]; // Поточний розмір вибірки
                double sumLambdaHat = 0.0; // Сума точкових оцінок лямбда за 1000 прогонів
                double sumSqError = 0.0; // Сума квадратів похибок для розрахунку MSE

                for (int sim = 0; sim < mRepeats; sim++) // Генеруємо M повторних вибірок
                {
                    double sumX = 0.0; // Сума елементів однієї вибірки
                    for (int i = 0; i < n; i++) // Генеруємо n випадкових чисел з Exp(0.04)
                    {
                        double u = 1.0 - rnd.NextDouble(); // Рівномірне випадкове число в інтервалі (0, 1]
                        double x = -Math.Log(u) / lambdaTrue; // Формула оберненої функції для показникового закону
                        sumX += x; // Додаємо згенерований час між збоями до суми
                    }

                    double xBar = sumX / n; // Вибіркове середнє поточної вибірки
                    double lambdaHat = 1.0 / xBar; // Оцінка параметра за методом моментів та максимальної правдоподібності
                    sumLambdaHat += lambdaHat; // Накопичуємо оцінку для обчислення математичного сподівання

                    double err = lambdaHat - lambdaTrue; // Відхилення оцінки від істинного значення 0.04
                    sumSqError += err * err; // Додаємо квадрат похибки
                }

                double meanLambdaHat = sumLambdaHat / mRepeats; // Середнє значення оцінки за 1000 експериментів
                double bias = meanLambdaHat - lambdaTrue; // Зміщення оцінки Bias
                double mse = sumSqError / mRepeats; // Середньоквадратична похибка MSE

                Console.WriteLine($"n={n,5} | M[lambda_hat]={meanLambdaHat:F6} | Bias={bias:+0.000000;-0.000000} | MSE={mse:F8}");
            }
            Console.WriteLine();
        }
    }
}