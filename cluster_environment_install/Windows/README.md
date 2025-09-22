#### Go to Microsoft Site: https://www.microsoft.com/en-us/download/details.aspx?id=105289
1. Download the follow files:
  *   msmpisetup.exe
  *   msmpisdk.msi
  *   install each (msmpisetup.exe, msmpisdk.msi)

2. To run MPI applications, use the MPI command line: 
  *   mpiexec -n 4 .\MyApp.exe
  *   The previous sample will use 4 MPI process, for "MyApp.exe" application.

3. ## 🚀 Jaguar interative terminal (Parallel Environment mode) on Windows:
##### Open the cmd or powershell.
##### Create a folder, which source code will be cloned. For example: c:\MPI
<pre><code>cd c:\MPI</code></pre>
<pre><code>git clone https://github.com/UFC-Jaguar/jaguar-lang</code></pre>
<pre><code>cd jaguar-lang\Base\bin</code></pre>
##### The easy mode for compile and run:
1. Install [Visual Studio Community](https://visualstudio.microsoft.com/pt-br/downloads/)
2. Compile using Visual Studio Community.
3. Run:
<pre><code>mpiexec -n 4 ./Jaguar.exe</code></pre>
