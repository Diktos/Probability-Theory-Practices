using System; // Математичні функції та консольний вивід

namespace Practice7
{
    public static class Block3
    {
        //  БЛОК 3 ДОВІРЧИЙ ІНТЕРВАЛ ДЛЯ ЙМОВІРНОСТІ В СХЕМІ БЕРНУЛЛІ 
        public static void Run()
        {
            Console.WriteLine(" БЛОК 3 ДОВІРЧИЙ ІНТЕРВАЛ ДЛЯ ЧАСТКИ ПОМИЛОК БЕРНУЛЛІ ");

            int n = 500; // Загальна кількість запитів за умовою
            int m = 35; // Кількість зафіксованих помилок за умовою
            double pHat = (double)m / n; // Точкова оцінка частки помилок 0.07
            double z = 1.959964; // Квантиль стандартного нормального розподілу для надії 0.95

            double waldHalf = z * Math.Sqrt(pHat * (1.0 - pHat) / n); // Півширина наближеного інтервалу Волда
            double waldLow = pHat - waldHalf; // Нижня межа інтервалу Волда
            double waldHigh = pHat + waldHalf; // Верхня межа інтервалу Волда

            double z2 = z * z; // Квадрат квантиля z
            double denom = 1.0 + z2 / n; // Знаменник у формулі Вілсона
            double wilsonCenter = (pHat + z2 / (2.0 * n)) / denom; // Зміщений центр інтервалу Вілсона
            double wilsonHalf = (z * Math.Sqrt(pHat * (1.0 - pHat) / n + z2 / (4.0 * n * n))) / denom; // Півширина інтервалу Вілсона
            double wilsonLow = wilsonCenter - wilsonHalf; // Нижня межа інтервалу Вілсона
            double wilsonHigh = wilsonCenter + wilsonHalf; // Верхня межа інтервалу Вілсона

            Console.WriteLine($"Точкова оцінка p_hat = {pHat:F4}");
            Console.WriteLine($"Інтервал Волда (95%)   -> [{waldLow:F5}; {waldHigh:F5}], ширина={(waldHigh - waldLow):F5}");
            Console.WriteLine($"Інтервал Вілсона (95%) -> [{wilsonLow:F5}; {wilsonHigh:F5}], ширина={(wilsonHigh - wilsonLow):F5}");

            int[] nGrid = { 50, 100, 250, 500, 1000, 2000, 3500, 5000 }; // Сітка обсягів вибірки для графіка ширини
            Console.WriteLine("Залежність ширини інтервалу Delta від обсягу вибірки n");
            for (int i = 0; i < nGrid.Length; i++) // Проходимо по всіх значеннях n від 50 до 5000
            {
                int curN = nGrid[i]; // Поточний обсяг вибірки
                double delta = 2.0 * z * Math.Sqrt(pHat * (1.0 - pHat) / curN); // Ширина інтервалу для поточного n
                Console.WriteLine($"  n={curN,4} -> Delta={delta:F5}");
            }

            double eps = 0.01; // Задана гранична похибка 1 відсоток
            double nMinExact = (z2 * pHat * (1.0 - pHat)) / (eps * eps); // Точна формула мінімального обсягу вибірки
            int nMin = (int)Math.Ceiling(nMinExact); // Округлюємо вгору до цілого числа випробувань

            Console.WriteLine($"Мінімальний обсяг вибірки для похибки <= 0.01 становить n >= {nMin} (точне {nMinExact:F2})");
            Console.WriteLine();
        }
    }
}