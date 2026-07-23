#!/bin/bash
N=4
if [ $1 ]; then
	N="$1"
	if [ $2 ]; then
		echo "Process count: $N"
		mpirun -np $N mono Jaguar.exe $2
	fi
else
	echo "Example to 32 process over 'matrix_par.c' source code:"
	echo "     ./test_jaguar.sh 32 matrix_par.c"
fi



