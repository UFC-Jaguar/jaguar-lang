#!/bin/bash


################ Instalacoes Básicas ###############################
sudo apt install git vim -y
sudo apt install net-tools iputils-ping cifs-utils traceroute -y
sudo apt install openssh-server -y

sudo apt install rar unrar p7zip-full p7zip-rar -y
sudo apt-get install build-essential cmake cmake-data autoconf automake pkg-config libtool libzip-dev libxml2-dev libglvnd-dev -y
sudo apt install nasm make gcc g++ gfortran -y
####################################################################

sudo chown $USER:$USER /opt
mkdir -p /opt/MPI

existe=$(ls /etc/hosts.bkp)
if ! [ "$existe" == "/etc/hosts.bkp" ]; then
	sudo cp /etc/hosts /etc/hosts.bkp
fi
sudo cp 0-hosts /etc/hosts

echo "" ; echo "" ; echo "" ; echo ""
echo "****************************************** ATENÇÃO *******************************************"
echo "##############################################################################################"
echo "################# Voce deve montar o compartilhamento samba em /opt/MPI ######################"
echo "##############################################################################################"
echo "seja, 'n0' o nome do master, onde há o samba e a pasta MPI compartilhada, faça:"
echo "   Antes de executar ./2-cluster_worker.sh, faça:"
echo "   >> ./samba_mount.sh # e digite:"
echo "   >> //n0/MPI"
echo "   >> $USER"
echo "   >> SuaSenha"

echo "OU digite:"
echo "  sudo mount -t cifs -o uid=$UID,gid=$UID,username=$USER,password=SuaSenha //n0/MPI /opt/MPI"
echo "*** OBS **:"
echo "   Após isso, sua pasta compartilhada com o master será: /opt/MPI"
echo "   Digitar 'df' para verificar se está montada. Caso sim, será listada."

echo "Para testar ssh (entrar sem senha), em cada node master e worker, digitar:"
echo "ssh n0"
echo "ssh n1"
echo "(....)"
