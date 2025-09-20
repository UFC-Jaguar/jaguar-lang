#!/bin/bash
user=$USER
mydir=$(pwd)
home=$HOME
host=$(/bin/hostname)

cd $home

########################### WORKERS ######################
cp /opt/MPI/ssh.tar.gz $home/
tar -xzf ssh.tar.gz
ssh-copy-id $user@$host
##########################################################

cd $mydir

echo "################################## Atenção ###############################"
echo "Para completar e testar o ssh sem senha, em cada node master e worker, digitar:"
echo "ssh n0"
echo "ssh n1"
echo "(....)"

echo "responda 'yes' às perguntas!!"
