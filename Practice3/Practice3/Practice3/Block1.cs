using System; // Базові функції консолі, математики та генератор випадкових чисел

namespace Practice3 
{
    public class Block1 
    {
        public static void Run() 
        {
            Console.WriteLine(" БЛОК 1 СИМУЛЯЦІЯ УМОВНОЇ ЙМОВІРНОСТІ "); 

            Random rand = new Random(42); 
            int[] nValues = { 100, 1000, 10000, 100000, 1000000 }; // Масив кількостей запитів для перевірки закону великих чисел

            double pB = 0.30; // Ймовірність того, що прийшов складний JOIN-запит (подія B)
            double pAgivenB = 0.70; // Ймовірність затримки >200 мс, якщо запит складний P(A|B)
            double pAgivenNotB = 0.10; // Ймовірність затримки >200 мс, якщо запит простий P(A|не B)
            double theoBgivenA = 0.75; // Точне теоретичне значення P(B|A) = 0.21 / 0.28

            foreach (int n in nValues) // Цикл перебору кожної кількості випробувань N зі списку
            {
                int countA = 0; 
                int countB = 0; 
                int countAB = 0; // Лічильник запитів, які одночасно складні і повільні (перетин A і B)

                for (int i = 0; i < n; i++) // Цикл симуляції N окремих запитів до бази даних
                {
                    bool isB = rand.NextDouble() < pB; // Перевіряємо, чи випав складний запит (число від 0 до 1 менше 0.30)
                    bool isA = false; // Чи перевищив час виконання 200 мс?

                    if (isB) // Якщо запит виявився складним — перевіряємо затримку з шансом 0.70
                    {
                        isA = rand.NextDouble() < pAgivenB; // Фіксуємо затримку для складного запиту
                    }
                    else // Запит простий? — перевіряємо затримку з меншим шансом 0.10
                    {
                        isA = rand.NextDouble() < pAgivenNotB; // Фіксуємо затримку для простого запиту
                    }

                    if (isB) 
                    {
                        countB++; 
                    }

                    if (isA) 
                    {
                        countA++; 
                    }

                    if (isA && isB) 
                    {
                        countAB++; 
                    }
                }

                double wA = (double)countA / n; // Статистична частота затримки W(A) = M(A) / N
                double wB = (double)countB / n; // Статистична частота складних запитів W(B) = M(B) / N
                double wAB = (double)countAB / n; // Статистична частота сумісної появи W(A і B) = M(AB) / N
                double wAgivenB = countB > 0 ? (double)countAB / countB : 0.0; // Оцінка умовної ймовірності W(A|B) = M(AB) / M(B)
                double wBgivenA = countA > 0 ? (double)countAB / countA : 0.0; // Оцінка умовної ймовірності W(B|A) = M(AB) / M(A)
                double error = Math.Abs(wBgivenA - theoBgivenA); // Абсолютна похибка між статистикою W(B|A) і теорією 0.75

                Console.WriteLine($"N={n}: W(A)={wA:F5}, W(B)={wB:F5}, W(AB)={wAB:F5}, W(A|B)={wAgivenB:F5}, W(B|A)={wBgivenA:F5}, Похибка={error:F5}"); 
            }

            Console.WriteLine(); 
        }
    }
}