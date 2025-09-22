Download address: https://www.microsoft.com/en-us/download/details.aspx?id=105289
Download the follow files:
  msmpisetup.exe
  msmpisdk.msi

install each (msmpisetup.exe, msmpisdk.msi).
To run MPI applications, use the MPI command line. 
Sample with 4 MPI process ("MyApp.exe"):
  mpiexec -n 4 .\MyApp.exe


## 🚀 For Run Parallel Environment on Windows:
##### Open the cmd or powershell.
##### Create a folder, where source code will cloned. For example: c:\MPI
<pre><code>cd c:\MPI</code></pre>
<pre><code>git clone https://github.com/UFC-Jaguar/jaguar-lang</code></pre>
<pre><code>cd jaguar-lang\Base\bin</code></pre>
##### The easy mode for compile and run:
1. Install [Visual Studio Community](https://visualstudio.microsoft.com/pt-br/downloads/)
2. Compile using Visual Studio Community.
3. Run the run.bat:
<pre><code>mpiexec -n 4 ./Jaguar.exe</code></pre>
