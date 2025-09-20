
REM "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\msbuild.exe"
REM msbuild.exe .\Jaguar.sln
@echo off

"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Jaguar.sln

cd bin
mpiexec -n 4 Jaguar.exe
cd ../

echo "############################# NOTE: ##############################"
echo "::::: This script run if you have msbuild.exe on windows! ::::::::"
echo "::::: Edit the .bat and change de path case any error appeir! ::::"
echo "##################################################################"

