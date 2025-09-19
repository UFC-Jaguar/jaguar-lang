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

