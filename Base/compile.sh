#!/bin/bash
num_version="$1"
# Remove all dots
num_no_dots="${num_version//./}"
version="net$num_no_dots"

if [ -z "$1" ]; then
	echo "Define the version as: "
	echo "  ./compile.sh dot_net_version"
	echo "Ex:"
	echo "  ./compile.sh 4.5"
	echo "  ./compile.sh 4.8"
else if [ "$version" = "net45" ]; then
	echo "######### 4.5 ##############"
	./lib_set_linux.sh
	xbuild Jaguar.net45.sln
	./lib_set_win.sh
else if [ "$version" = "net48" ]; then
	echo "######### 4.8 ##############"
	./lib_set_linux.sh
	xbuild Jaguar.sln
	./lib_set_win.sh
else
	echo "######### $version not suported ##############"
fi fi fi

