#!/bin/bash
#python3 -m venv venv
#source venv/bin/activate
#pip install threadpoolctl
#pip install numpy
#deactivate

R=30
LOG="1_numpy_log.txt"
rm -rf $LOG
echo "Total Matrix (lin x col) ; Time" >> $LOG

N="8"
echo "############## 2^8 = 256 Matrix - Available max $N Threads ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	python 1_test_A.py $N >> $LOG
done
echo "" >> $LOG

N="16"
echo "############## 2^12 = 4096 Matrix - Available max $N Threads ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	python 1_test_B.py $N >> $LOG
done
echo "" >> $LOG

N="32"
echo "############## 2^16 = 65536 Matrix - Available max $N Threads ##############" >> $LOG
for ((i=1; i<($R+1); i++)); do
	echo "Count run $i : $N"
	python 1_test_C.py $N >> $LOG
done

