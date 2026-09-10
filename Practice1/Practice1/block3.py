# Блок 3. Оцінка числа Пі (Монте-Карло)

import random
import math

M_pi = sum(1 for _ in range(N_block2) if random.uniform(-1, 1)**2 + random.uniform(-1, 1)**2 <= 1.0)
pi_est = 4.0 * M_pi / N_block2 

print(f"\nПі (Монте-Карло): {pi_est:.5f}")
print(f"Похибка: {abs(pi_est - math.pi):.5f}")
