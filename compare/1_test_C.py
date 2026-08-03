import sys
import numpy as np
import time
from threadpoolctl import threadpool_limits

size = 1
load = 65536
# Check if arguments were passed
if len(sys.argv) > 1:
    script_name = sys.argv[0]
    size = int(sys.argv[1])

    with threadpool_limits(limits=size, user_api='blas'):
        start_time = time.perf_counter()
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

        for e in matrixs:
            m = m @ e
        end_time = time.perf_counter()
        print(f"{load} ({lin}x{col}) ; {end_time - start_time:.6f} seconds")

