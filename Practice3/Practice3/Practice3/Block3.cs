using System; // Базові функції консолі, математики та генератор випадкових чисел

namespace Practice3 
{
    public class Block3 
    {
        public static void Run() 
        {
            Console.WriteLine(" БЛОК 3 НАДІЙНІСТЬ СЕРІЙНО-ПАРАЛЕЛЬНОЇ СИСТЕМИ "); 

            Random rand = new Random(42); 
            int[] nValues = { 100, 1000, 10000, 100000, 1000000 }; 

            double pLB = 0.98; // Надійність вхідного балансувальника LB (p1)
            double pWeb1 = 0.90; // Надійність першого веб-сервера Web1 (p2)
            double pWeb2 = 0.90; // Надійність другого веб-сервера Web2 (p3)
            double pDB1 = 0.95; // Надійність першої бази даних DB1 (p4)
            double pDB2 = 0.95; // Надійність другої бази даних DB2 (p5)

            double theoSys = pLB * (1.0 - (1.0 - pWeb1) * (1.0 - pWeb2)) * (1.0 - (1.0 - pDB1) * (1.0 - pDB2)); // Точна теоретична надійність системи = 0.9677745

            foreach (int n in nValues) // Цикл перебору кожної кількості випробувань N
            {
                int sysOkCount = 0; // Лічильник успішних спрацювань усієї веб-системи

                for (int i = 0; i < n; i++) // Цикл проведення N незалежних випробувань системи
                {
                    bool lbOk = rand.NextDouble() < pLB; // Перевіряємо, чи працює балансувальник LB
                    bool web1Ok = rand.NextDouble() < pWeb1; // Перевіряємо, чи працює перший сервер Web1
                    bool web2Ok = rand.NextDouble() < pWeb2; // Перевіряємо, чи працює другий сервер Web2
                    bool db1Ok = rand.NextDouble() < pDB1; // Перевіряємо, чи працює перша база DB1
                    bool db2Ok = rand.NextDouble() < pDB2; // Перевіряємо, чи працює друга база DB2

                    bool webSubsystemOk = web1Ok || web2Ok; // Паралельна ланка веб-серверів працює, якщо живий хоча б один
                    bool dbSubsystemOk = db1Ok || db2Ok; // Паралельна ланка баз даних працює, якщо жива хоча б одна

                    if (lbOk && webSubsystemOk && dbSubsystemOk) // Послідовна схема: система працює, якщо справні всі три ланки
                    {
                        sysOkCount++; // Фіксуємо успішну роботу всієї системи
                    }
                }

                double wSys = (double)sysOkCount / n; // Статистична оцінка надійності системи W_sys = M / N
                double error = Math.Abs(wSys - theoSys); // Абсолютна похибка між симуляцією та теоретичною формулою

                Console.WriteLine($"N={n}: W_sys={wSys:F5}, Теорія={theoSys:F5}, Похибка={error:F5}"); // Виводимо результат для поточного N
            }

            Console.WriteLine(); 
        }
    }
}