
import random
import math

# Блок 1. Закон великих чисел
p_exact = 0.5 
N_values = [10, 100, 1000, 10000, 100000, 1000000]

for N in N_values:
    M = sum(1 for _ in range(N) if random.random() < p_exact)
    W = M / N 
    print(f"N={N}: W(A)={W:.5f}, Похибка={abs(W - p_exact):.5f}")

