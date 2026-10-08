using System; // дає консольний вивід, базові типи та математичні функції

namespace Practice2
{
    //  БЛОК 2  
    // класична ймовірність, комбінаторика, несумісні події (Варіант 4)
    public static class Block2_Combinatorics
    {
        public static void Run(Random rnd)
        {
            const int nSim = 100000; // обсяг вибірки для забезпечення похибки менше 0.001

            // теоретичні розрахунки за формулами класичної комбінаторики
            double pA = 9.0 / 495.0; // частка: вибір 1 некритичної задачі з 9 поділений на C(12,4)
            double pB = 16.0 / 55.0; // частка: 4^3 способів розкидати критичні задачі поділені на C(12,3)
            double pAB = 0.0;        // перетин подій неможливий через їх несумісність
            double pAUB = pA + pB;   // теорема додавання ймовірностей для несумісних подій
            double pNotA = 1.0 - pA; // віднімання ймовірності події А від одиниці

            int countA = 0; // лічильник випадків, коли всі 3 критичні задачі у першого воркера
            int countB = 0; // лічильник випадків, коли кожен воркер має рівно по 1 критичній задачі
            int countAB = 0; // лічильник одночасного виконання обох подій
            int countAUB = 0; // лічильник настання хоча б однієї з двох подій
            int countNotA = 0; // лічильник ненастання першої події

            int[] tasks = new int[12]; // масив індексів задач (0, 1, 2 - критичні, решта - звичайні)

            // цикл ста тисяч незалежних розподілів
            for (int sim = 0; sim < nSim; sim++)
            {
                // заповнення масиву початковими індексами
                for (int i = 0; i < 12; i++) tasks[i] = i;

                // алгоритм тасування Фішера-Єйтса: рівноможливий вибір однієї з 12! перестановок
                for (int i = 11; i > 0; i--)
                {
                    int j = rnd.Next(i + 1); // випадковий індекс від 0 до i включно
                    int temp = tasks[i]; // тимчасове збереження значення для обміну
                    tasks[i] = tasks[j];
                    tasks[j] = temp;
                }

                int critW1 = 0; // критичні задачі, що дісталися воркеру 1 (індекси 0..3)
                int critW2 = 0; // критичні задачі, що дісталися воркеру 2 (індекси 4..7)
                int critW3 = 0; // критичні задачі, що дісталися воркеру 3 (індекси 8..11)

                // підрахунок критичних задач у першій четвірці
                for (int i = 0; i < 4; i++) if (tasks[i] < 3) critW1++;
                // підрахунок критичних задач у другій четвірці
                for (int i = 4; i < 8; i++) if (tasks[i] < 3) critW2++;
                // підрахунок критичних задач у третій четвірці
                for (int i = 8; i < 12; i++) if (tasks[i] < 3) critW3++;

                bool isA = (critW1 == 3); // перевірка виконання умови події А
                bool isB = (critW1 == 1 && critW2 == 1 && critW3 == 1); // перевірка виконання умови події В

                // накопичення статистичних лічильників
                if (isA) countA++;
                if (isB) countB++;
                if (isA && isB) countAB++;
                if (isA || isB) countAUB++;
                if (!isA) countNotA++;
            }

            // розрахунок практичних частот
            double wA = (double)countA / nSim; // частота події А
            double wB = (double)countB / nSim; // частота події В
            double wAB = (double)countAB / nSim; // частота перетину
            double wAUB = (double)countAUB / nSim; // частота об'єднання
            double wNotA = (double)countNotA / nSim; // частота протилежної події

            Console.WriteLine("БЛОК 2. Балансування задач між воркерами (Варіант 4)");
            Console.WriteLine($"{"Подія",15} | {"Теорія P",12} | {"Статистика W",14} | {"Похибка Δ",12}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"P(A)",15} | {pA,12:F5} | {wA,14:F5} | {Math.Abs(wA - pA),12:F5}");
            Console.WriteLine($"{"P(B)",15} | {pB,12:F5} | {wB,14:F5} | {Math.Abs(wB - pB),12:F5}");
            Console.WriteLine($"{"P(A ∩ B)",15} | {pAB,12:F5} | {wAB,14:F5} | {Math.Abs(wAB - pAB),12:F5}");
            Console.WriteLine($"{"P(A ∪ B)",15} | {pAUB,12:F5} | {wAUB,14:F5} | {Math.Abs(wAUB - pAUB),12:F5}");
            Console.WriteLine($"{"P(Ā)",15} | {pNotA,12:F5} | {wNotA,14:F5} | {Math.Abs(wNotA - pNotA),12:F5}");
            Console.WriteLine($"Перевірка P(Ā) = 1 - P(A): 1 - {wA:F5} = {1 - wA:F5}, W(Ā) = {wNotA:F5}\n");
        }
    }
}