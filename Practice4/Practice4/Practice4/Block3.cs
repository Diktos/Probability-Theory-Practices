using System; 

namespace Practice4
{
    public static class Block3
    {
        public static void Run()
        {
            Console.WriteLine(" БЛОК 3 НАЇВНИЙ БАЙЄСІВСЬКИЙ КЛАСИФІКАТОР СПАМУ ");

            Random rnd = new Random(42); 
            int n = 10000; 

            double pSpam = 0.30; // Початковий шанс що лист є спамом
            double pHam = 0.70; // Початковий шанс що лист є нормальним (Ham)

            double pW1_Spam = 0.80; // Ймовірність слова free у спамі
            double pW2_Spam = 0.60; // Ймовірність слова click у спамі
            double pW1_Ham = 0.05; // Ймовірність слова free у нормальному листі
            double pW2_Ham = 0.10; // Ймовірність слова click у нормальному листі

            int spamDetected = 0; // Істинно позитивні (спам правильно названо спамом)
            int falseAlarms = 0; // Хибно позитивні (нормальний лист помилково кинуто в спам)
            int cleanPassed = 0; // Істинно негативні (нормальний лист правильно пропущено)
            int spamMissed = 0; // Хибно негативні (спам пропущено у вхідні)

            for (int i = 0; i < n; i++) // Проганяємо 10 000 тестових листів
            {
                bool actualSpam = rnd.NextDouble() < pSpam; // Генеруємо реальний клас листа (спам чи нормальний)

                int x1 = rnd.NextDouble() < (actualSpam ? pW1_Spam : pW1_Ham) ? 1 : 0; // Ознака 1: чи є слово free (1 або 0)
                int x2 = rnd.NextDouble() < (actualSpam ? pW2_Spam : pW2_Ham) ? 1 : 0; // Ознака 2: чи є слово click (1 або 0)

                double likeW1_Spam = x1 == 1 ? pW1_Spam : (1.0 - pW1_Spam); // Множник для першого слова за гіпотези спаму
                double likeW2_Spam = x2 == 1 ? pW2_Spam : (1.0 - pW2_Spam); // Множник для другого слова за гіпотези спаму
                double scoreSpam = pSpam * likeW1_Spam * likeW2_Spam; // Апостеріорна вага класу Spam

                double likeW1_Ham = x1 == 1 ? pW1_Ham : (1.0 - pW1_Ham); // Множник для першого слова за гіпотези Ham
                double likeW2_Ham = x2 == 1 ? pW2_Ham : (1.0 - pW2_Ham); // Множник для другого слова за гіпотези Ham
                double scoreHam = pHam * likeW1_Ham * likeW2_Ham; // Апостеріорна вага класу Ham(наскільки цей лист схожий на нормальний за формулою Байєса)

                bool predictedSpam = scoreSpam > scoreHam; // Якщо вага спаму більша — класифікатор блокує лист

                if (actualSpam && predictedSpam) spamDetected++; // Спам вгадано правильно
                else if (!actualSpam && predictedSpam) falseAlarms++; // Хибна тривога на нормальному листі
                else if (!actualSpam && !predictedSpam) cleanPassed++; // Нормальний лист пропущено правильно
                else spamMissed++; // Спам не помічено
            }

            double accuracy = (double)(spamDetected + cleanPassed) / n; // Частка усіх правильних рішень класифікатора
            double precision = (double)spamDetected / (spamDetected + falseAlarms); // Точність: скільки реального спаму серед заблокованих
            double recall = (double)spamDetected / (spamDetected + spamMissed); //Яку частку всього спаму вдалося зловити
            double f1 = 2.0 * precision * recall / (precision + recall); // Середнє гармонійне точності та повноти

            Console.WriteLine($"Матриця помилок: TP={spamDetected}, FP={falseAlarms}, TN={cleanPassed}, FN={spamMissed}");
            Console.WriteLine($"Метрики якості:  Accuracy={accuracy:F5}, Precision={precision:F5}, Recall={recall:F5}, F1={f1:F5}");
            Console.WriteLine();
        }
    }
}