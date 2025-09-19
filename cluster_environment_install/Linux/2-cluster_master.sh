#!/bin/bash
user=$USER
mydir=$(pwd)
home=$HOME
host=$(/bin/hostname)

cd $home

########################### MASTER ######################
ssh-keygen -t rsa
cp $home/.ssh/id_rsa.pub $home/.ssh/authorized_keys
chmod 600 $home/.ssh/authorized_keys
tar -czf /opt/MPI/ssh.tar.gz .ssh/
ssh-copy-id $user@$host
#########################################################

cd $mydir

