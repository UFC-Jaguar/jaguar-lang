#!/bin/bash

sudo chown $USER:$USER /opt
mkdir -p /opt/MPI

sudo apt install git vim -y
sudo apt install net-tools iputils-ping cifs-utils traceroute -y
sudo apt install samba -y
sudo apt install openssh-server -y

sudo apt install rar unrar p7zip-full p7zip-rar -y
sudo apt-get install build-essential cmake cmake-data autoconf automake pkg-config libtool libzip-dev libxml2-dev libglvnd-dev -y
sudo apt install nasm make gcc g++ gfortran -y

sudo service smbd stop
sudo mv /etc/samba/smb.conf /etc/samba/smb.conf-org
sudo cp smb.conf /etc/samba/
sudo service smbd start


existe=$(ls /etc/hosts.bkp)
if ! [ "$existe" == "/etc/hosts.bkp" ]; then
	sudo cp /etc/hosts /etc/hosts.bkp
fi
sudo cp 0-hosts /etc/hosts

echo "****************************************** ATENÇÃO *******************************************"
echo "##############################################################################################"
echo "################# Voce deve cadastrar uma senha para o usuario samba #########################"
echo "################################# sudo smbpasswd -a $USER ####################################"
sudo smbpasswd -a $USER
echo "##############################################################################################"

