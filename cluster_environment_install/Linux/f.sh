#!/bin/bash
res=$(ls /etc/hosts.bkp)
echo ": $res"

if [ "$res" == "/etc/hosts.bkp" ]; then
	echo "achou"
else
	echo "nao achou"
fi

