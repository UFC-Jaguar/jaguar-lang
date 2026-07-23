#!/bin/bash
cd jaguar_bin/
mpirun -np 32 mono Jaguar.exe matrix_par.c
cd ../
