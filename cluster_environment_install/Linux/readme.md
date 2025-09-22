# 📃 Steps:
1. Edit file "0-hosts".
   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Set the ipaddress and names. Sample:
<pre><code>127.0.0.1 localhost
10.0.2.10 n0
10.0.2.11 n1
10.0.2.12 n2
### ...
10.0.2.100 n90
### ...
10.0.2.i n(i-10)
</code></pre>
2. Execute the scripts, sorted first to last (on master node)
     *      ./1-install_master.sh
     *      ./2-cluster_master.sh
     *      ./3-mono_mpi.sh
2. Execute the scripts, sorted first to last (on Workers node)
     *      ./1-install_worker.sh
     *      ./2-cluster_worker.sh
     *      ./3-mono_mpi.sh
3. Execute on Master and Workers (yes for all questions). **NOTE: No password should be required**:
     *      ssh n0
     *      ssh n1
     * ...
     *      ssh ni
4. On Master, git clone the projet on "/opt/MPI". This folder is common on Master and Workers, shared by samba service.
     *      cd /opt/MPI
     *      git clone https://github.com/UFC-Jaguar/jaguar-lang
     * NOTE: Before run, you are need remember that [Mono/MPI](https://github.com/UFC-Jaguar/jaguar-lang/tree/main/cluster_environment_install) is used.
     *      cd jaguar-lang/Base/
     *      xbuild Jaguar.sln
     *      cd bin/
     *      mpiexec -n 4 mono ./Jaguar.exe

