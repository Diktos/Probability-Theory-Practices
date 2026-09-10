import matplotlib.pyplot as plt
import random
import math

# Перед запуском потрібно в терміналі візуалки pip install matplotlib

# Графік 1. Залежність N випробувань від статистичної частоти W(A) (Блок 1: Закон великих чисел)
N_values = [10, 100, 1000, 10000, 100000, 1000000]
W_values = [0.50000, 0.59000, 0.50500, 0.49470, 0.50292, 0.50008]

plt.figure(figsize=(8, 5))
plt.plot(N_values, W_values, marker='o', label='Статистична частота W(A)')
plt.axhline(y=0.5, color='r', linestyle='--', label='Теоретична ймовірність (0.5)')
plt.xscale('log') 
plt.xlabel('Кількість випробувань (N)')
plt.ylabel('Частота')
plt.title('Блок 1: Закон великих чисел')
plt.legend()
plt.grid(True)
plt.savefig('block1_graph.png')
plt.close()

# Графік 2 (Блок 3: Збіжність Пі) 
N_mc = 10000
pi_estimates = []
M_pi = 0
steps = range(100, N_mc + 1, 100)

for i in range(1, N_mc + 1):
    if random.uniform(-1, 1)**2 + random.uniform(-1, 1)**2 <= 1.0:
        M_pi += 1
    if i % 100 == 0:
        pi_estimates.append(4.0 * M_pi / i)

plt.figure(figsize=(8, 5))
plt.plot(list(steps), pi_estimates, label='Оцінка Пі (Монте-Карло)')
plt.axhline(y=math.pi, color='r', linestyle='--', label='Точне значення Пі')
plt.xlabel('Кількість випробувань (N)')
plt.ylabel('Значення Пі')
plt.title('Блок 3: Метод Монте-Карло (Оцінка числа Пі)')
plt.legend()
plt.grid(True)
plt.savefig('block3_graph.png')
plt.close()

print("Готово! Файли block1_graph.png та block3_graph.png з'явилися у твоїй папці.")