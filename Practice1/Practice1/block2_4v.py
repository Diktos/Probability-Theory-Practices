# Блок 2. Варіант 4. Задача про зустріч (Геометрична ймовірність)

import random
import math

N_block2 = 100000 
T = 60.0 
tau = 10.0 

meeting_count = sum(1 for _ in range(N_block2) if abs(random.uniform(0, T) - random.uniform(0, T)) <= tau)

W_meeting = meeting_count / N_block2 
P_meeting_exact = 1.0 - ((T - tau) / T) ** 2 

print(f"\nЗустріч (теорія): {P_meeting_exact:.5f}")
print(f"Зустріч (стат): {W_meeting:.5f}")
print(f"Відсутність зустрічі: {1.0 - W_meeting:.5f}")
print(f"Похибка: {abs(W_meeting - P_meeting_exact):.5f}")
