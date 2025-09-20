#!/bin/bash
num_version="$1"
tasks="$2"
# Remove all dots
num_no_dots="${num_version//./}"
version="net$num_no_dots"
compilou="false"

if [ -z "$1" ]; then
	echo "Define the version as: "
	echo "  ./run_compile_linux.sh dot_net_version"
	echo "Ex:"
	echo "  ./run_compile_linux.sh 4.5"
	echo "  ./run_compile_linux.sh 4.8"
else if [ "$version" = "net45" ]; then
	./compile.sh $num_no_dots
	compilou="4.5"
else if [ "$version" = "net48" ]; then
	./compile.sh $num_no_dots
	compilou="4.8"
else
	echo "######### $version not suported ##############"
fi fi fi


if [ "$compilou" = "false" ]; then
	echo "Not possible compile the code!!!"
else
	if [ -z "$tasks" ]; then
		./run.sh 4
	else
		./run.sh $tasks
	fi 
fi

