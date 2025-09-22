----

# This folder include a miscellaneous of examples and resources, target to run **MPI** and **MONO** on Linux.
### Enviromment MPI and MONO on Linux:
1. Require the compiled mono_mpi_environment.tar.gz (in the group), or download/compile a tested version of [mpich2-1.5rc3](https://www.mpich.org/static/downloads/1.5rc3/mpich2-1.5rc3.tar.gz) and [mono-6.12.0.90](https://github.com/mono/mono/archive/refs/tags/mono-6.12.0.90.tar.gz).
2. Case download/compile, make this instruction 2. Caso no, go to next instruction (3).
     * For compatibilty with this tutorial, set compiled folder like "/opt/mono-6.12.0.90" and "/opt/mpich2-1.5rc3";
     * Include on ~/.bashrc (end of file) the follow content:
<pre><code>######################## Mono and MPICH Install ###############################
#*********************************** MONO ***********************************
if [ $LD_LIBRARY_PATH ]; then # Case LD_LIBRARY_PATH
        export LD_LIBRARY_PATH=":$LD_LIBRARY_PATH"
else
        export LD_LIBRARY_PATH=""
fi
export MONO_HOME=/opt/mono-6.12.0.90
export LD_LIBRARY_PATH=${MONO_HOME}/lib$LD_LIBRARY_PATH
export PATH=${MONO_HOME}/bin:${PATH}
#*********************************** MPICH **********************************
export MPI_HOME=/opt/mpich2-1.5rc3
export PATH=${MPI_HOME}/bin:${PATH}
export LD_LIBRARY_PATH=${MPI_HOME}/lib:$LD_LIBRARY_PATH
####################### END: Mono and MPICH Install ############################</code></pre>
- Go to **instruction number 7**.

3. Case mono_mpi_environment.tar.gz, make:
     - 1.1 Dependencies: You need install: libxml2-dev and git
     - 1.1.1 Ex: Ubuntu or Debian: 
     *      sudo apt update
     *      sudo apt install git libxml2-dev -y
4. Copy the downloaded file mono_mpi_environment.tar.gz to /opt folder:
     *      sudo cp mono_mpi_environment.tar.gz /opt/
     *      cd /opt/
     * Optional (without sudo) - change permission on /opt:
     *      sudo chown your_user:your_group /opt
5. "Unzip" the file:
     *      sudo tar -xzf mono_mpi_environment.tar.gz
6. Open the folder to install:
     *      cd mono_mpi_environment/
     *      ./sudo_install.sh # When used sudo permission
     *      ./root_install.sh # When used root permission
     - 4.1 Read the enviromment variables of mono and mpi:
     *      source to_end.bashrc
     - 4.2 **Or close the terminal and open again**
     - 4.3 Test your enviromment:
     *      mpiexec -n 4 /bin/hostname
7. Create the workspace:
     *      mkdir -p /opt/MPI
     *      sudo chown $USER:$GROUPS /opt/MPI
     *      cd /opt/MPI
8. Clone the git Project:
     *      git clone https://github.com/UFC-Jaguar/jaguar-lang
     *      cd jaguar-lang/cluster_environment_install/AulaMPI2
9. Compile the sources:
     *      chmod +x compile.sh
     *      ./compile.sh
10. To Run (4 process):
     *      cd bin/
     *      mpirun -np 4 mono AulaMPI2.exe
     *      cd ../
11. To Run Using Script (4 process):
     *      ./compile_run.sh 4
     - **OR just run, if you already compile:**
     *      ./run.sh 4

12. **The print is anything like:**
     * allToal: node 0 -> 0:0 0:1 0:2 0:3
     * allToal: node 1 -> 1:0 1:1 1:2 1:3
     * allToal: node 2 -> 2:0 2:1 2:2 2:3
     * allToal: node 3 -> 3:0 3:1 3:2 3:3

----
