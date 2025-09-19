#!/bin/bash
## sudo apt install cifs-utils
userid="$UID"
groupid="$GROUPS"

mnt="/opt/MPI"
mkdir -p $mnt

echo -n ">> //Host/folder: "
read host

echo -n ">> User: "
read user

echo -n ">> Pass: "
read pass

#echo "Host: $host, User: $user, Pass: $pass"
COM="sudo mount -t cifs -o uid=$userid,gid=$groupid,username=$user,password=$pass $host $mnt"
echo "      $COM"

echo -n ">> To mount, type 's' and 'Enter': "
read yes

if [ "$yes" = "s" ]; then
	echo "Montando...."
	$COM

fi

# sudo mount -t cifs -o uid=$UID,gid=$GROUPS,username=cnz,password=$SENHA //vostro/MPI /opt/MPI
# sudo mount -t cifs -o uid=$UID,gid=$GROUPS,username=cnz,password=$SENHA //vostro/MPI /opt/MPI ; l /opt/ ; sudo umount /opt/MPI



