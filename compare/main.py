import numpy as np
import time
#load = 65536
load = 4000
ini = 0
fim = load - 1
lin = 100
col = 100
matrixs = []
for i in range(ini, fim):
  matrixs += [[[0 for _ in range(col)] for _ in range(lin)]]

m = [[0 for _ in range(col)] for _ in range(lin)]

matrixs = np.array(matrixs)
m = np.array(m)

#for i in range(len(m)):
#  for j in range(len(m[0])):
#    if i<=j: m[i][j] = 1

#for e in matrixs:
#  for i in range(len(e)):
#    for j in range(len(e[0])):
#      if i<=j: e[i][j] = 1
start_time = time.perf_counter()
for e in matrixs:
  m = m @ e
end_time = time.perf_counter()
print(f"Execution time: {end_time - start_time:.6f} seconds")
print(f"Total de matrizes multiplicadas: {load}\nDimensao: {lin} x {col}")
#print(m)
