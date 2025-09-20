#!/bin/bash
num_tasks="$1"

if [ -z "$1" ]; then
	echo "Define the tasks: "
	echo "  ./run.sh num_tasks"
	echo "Ex:"
	echo "  ./run.sh 1"
	echo "  ./run.sh 2"
	echo "  ./run.sh 4"
	echo "  ./run.sh 8"
else
	run_exe=$(ls -laFh bin/Jaguar.exe)

	if [ -z "$run_exe" ]
	then
		echo "'bin/Jaguar.exe' not found!"
	else
		cd bin/
		mpirun -np $num_tasks mono Jaguar.exe
		cd ../
	fi
fi 


