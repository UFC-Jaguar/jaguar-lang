# 🚀 About and History <img src="./img/img_b_300.jpeg" style="width:128px" alt="Jaguar" title="Jaguar">
### This repository include a parallel version of **UFC-Lang** Project (programming language project). 
### The parallel version is named "Jaguar".
### **Jaguar** is a project of general-purpose language, where the resources aims to:
1. #### Parallel programming;
2. #### Teory of graphs suport;
3. #### Expressive syntactic.
## Around this issue, we are looking for:
1. #### Otmizations;
2. #### Best machine codes;
3. #### Good practices of programming.
## Jaguar version is building by project members. It's a institucional research project, started in [ufc cadproj](https://cadproj.ufc.br/projects/592), but all people can contributed (become a member). For that, use the email informed in end of page.
# 📃 Features
## The **main** Jaguar **features** (target) are:
- ### Multi-paradigm: functional, imperative, concurrent, parallel and distributed, process-oriented;
- ### Typing discipline: dynamic, duck, weak typing;
- ### Component oriented (by compiled library).

# 🚀 Technical description and/or dependencies MPI:
### For Run Parallel Environment:
1. #### [MPI environment config on Windows](https://github.com/UFC-Jaguar/jaguar-lang/blob/main/cluster_environment_install/Windows/Instrucoes.txt);
2. #### [MONO/MPI environment config on Linux](https://github.com/UFC-Jaguar/jaguar-lang/tree/main/cluster_environment_install);
3. #### [Config a HPC Cluster on Linux](https://github.com/UFC-Jaguar/jaguar-lang/tree/main/cluster_environment_install/Linux)
4. A basic running schema:
     *      sudo mkdir -p /opt/MPI
     *      sudo chown $USER:$GROUPS /opt/MPI
     *      cd /opt/MPI
     *      git clone https://github.com/UFC-Jaguar/jaguar-lang
     * NOTE: Before run, you are need remember that [Mono/MPI](https://github.com/UFC-Jaguar/jaguar-lang/tree/main/cluster_environment_install) is used.
     *      cd jaguar-lang/Base/
     *      xbuild Jaguar.sln
     *      cd bin/
     *      mpiexec -n 4 mono ./Jaguar.exe
     * NOTE: Running a sample cluster, with "/opt/MPI"  folder common among master and workers:
     *      vi /opt/MPI/mpi_hosts # Insert on /opt/MPI/mpi_hosts the follow content:
<pre><code>n0:1
n1:1</code></pre>
     *      cd /opt/MPI
     *      git clone https://github.com/UFC-Jaguar/jaguar-lang
     *      cd jaguar-lang/Base/
     *      xbuild Jaguar.sln
     *      cd bin/
     *      mpiexec -n 4 -f /opt/MPI/mpi_hosts mono ./Jaguar.exe

## Basic scheme:
<p align="center">
  <img src="./img/DiagramaClasse.png" alt="Class Diagram" width="650">
</p>

## Serial interative Repl example:
<p align="center">
  <img src="./img/Ex1.png" alt="Class Diagram" width="400">
</p>

## Parallell interative Repl example:
<p align="center">
  <img src="./img/Ex2.png" alt="Class Diagram" width="400">
</p>

# 🗨️ Contact by Email:
## computabilidade@gmail.com. Subject "Jaguar-Lang".

