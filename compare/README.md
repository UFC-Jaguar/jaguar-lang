# This folder include a miscellaneous of instructions, target to run **MPI** and **MONO** on Linux.
### Enviromment MPI and MONO on Linux:
1. Require the compiled [mono_mpi_environment.tar.gz](https://drive.google.com/file/d/1juuCPMtXjgo2edPJn9GzRNpVGQQRMaVW/view?usp=sharing). This file include compliled Mono, MPI.NET and MPICH2. This eliminates a laborious task compared to compiling source code ([mpich2-1.5rc3](https://www.mpich.org/static/downloads/1.5rc3/mpich2-1.5rc3.tar.gz),  [mono-6.12.0.90](https://github.com/mono/mono/archive/refs/tags/mono-6.12.0.90.tar.gz)).
2. With mono_mpi_environment.tar.gz, make:
     - 2.1 Dependencies: You need install: libxml2-dev
     - 2.1.1 Ex: Ubuntu or Debian:
     *      sudo apt update
     *      sudo apt install git libxml2-dev -y
3. Use a user with write permissions for /opt, or use **sudo** commands. Copy the downloaded file mono_mpi_environment.tar.gz to /opt folder:
     *      sudo cp mono_mpi_environment.tar.gz /opt/
     *      cd /opt/
     * Optional (without sudo) - change permission on /opt:
     *      sudo chown $USER:$GROUPS /opt
4. "Unzip" the file:
     *      sudo tar -xzf mono_mpi_environment.tar.gz
5. Open the folder to install:
     *      cd mono_mpi_environment/
     *      ./sudo_install.sh # When used sudo permission
     *      ./root_install.sh # When used root permission
     - 5.1 Read the enviromment variables of mono and mpi:
     *      source to_end.bashrc
     - 5.2 **Or close the terminal and open again**
     - 5.3 Test your enviromment:
     *      mpiexec -n 4 /bin/hostname
6. Create the workspace:
     *      mkdir -p /opt/MPI
     *      sudo chown $USER:$GROUPS /opt/MPI
     *      cd /opt/MPI
7. Clone the git Project:
     *      git clone https://github.com/UFC-Jaguar/jaguar-lang
     *      cd jaguar-lang/compare/
8. Compile the sources:
     *      chmod +x *.sh
     *      ./compilar.sh
9. **Informations to Run by scripts** (Jaguar and Numpy):
     - 9.1 Read script: "cat ./test_jaguar.sh" or "cat ./test_python.sh";
     - 9.2 Detail A, variables: R="30" (replications), LOG="2_jaguar_log.txt" (file result), and N (parallel process);
     - 9.3 Detail B, Loads Numpy and Jaguar are on the sources: (1_test_A.py, 1_test_B.py, 1_test_C.py) and (2_test_A.ru, 2_test_A.ru, 2_test_A.ru). On the sources, the variable "load" set a number of matrixs to multily;
10. **Tests warning**, before run Jaguar:
     - I recommend exclude or comment the block inside from 'test_jaguar.sh', referring to 32 N process. Off course, if you have a machine with 32 or more cores, ignore the exclusion over script block from 'test_jaguar.sh'.
12. **The script runners** to Jaguar and Numpy:
     *      ./test_jaguar.sh;
     *      ./test_python.sh.
----
