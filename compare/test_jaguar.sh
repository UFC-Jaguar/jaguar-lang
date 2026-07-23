#!/bin/bash
cd jaguar_bin/

N=4
if [ $1 ]; then
	N="$1"
fi

echo "Process count: $N"
mpirun -np $N mono Jaguar.exe matrix_par.c


cd ../
