#!/bin/bash
R="30"
LOG="2_jaguar_log.txt"
rm -rf $LOG
echo "Type (Parallel or Serial); Process_count ; Total Matrix (lin x col) ; Time" >> $LOG
#N=4
#if [ $1 ]; then
#	N="$1"
#	if [ $2 ]; then
#		echo "Process count: $N"
#		for ((i=1; i<($R+1); i++)); do
#			echo "Count run $i : $N"
#			mpirun -np $N mono Jaguar.exe $2 >> $LOG
#		done
#	fi
#else
#	echo "Example to 32 process over 'matrix_par.ru' source code:"
#	echo "     ./test_jaguar.sh 32 matrix_par.ru"
#fi

N="8"
echo "############## 2^8 = 256 Matrix - Available max $N Process ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	mpirun -np $N mono Jaguar.exe 2_test_A.ru >> $LOG
done
N="16"
echo "" >> $LOG
echo "############## 2^12 = 4096 Matrix - Available max $N Process ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	mpirun -np $N mono Jaguar.exe 2_test_B.ru >> $LOG
done

N="32"
echo "" >> $LOG
echo "############## 2^16 = 65536 Matrix - Available max $N Process ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	mpirun -np $N mono Jaguar.exe 2_test_C.ru >> $LOG
done

