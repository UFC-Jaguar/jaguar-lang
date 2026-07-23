load=65536
ini = 0
fim = (load)/size-1
lin = 100
col = 100
matrix = for i = ini : fim #[lin, col] end
m = #[lin, col]

tipo = if size == 1 "Serial" else "Paralelo" end

print("####################### Tipo: " + tipo + " ####################")
print("########## Quantidade de processos rodando: " + size + " ###########")
print("######## Multiplicacao de " + (fim + 1) * size + " matrizes " + lin + " x " + col + " ########")
t0 = now()
for e in matrix m = m * e end
gather = || .gather(m)

m = gather / 0
for e in gather m = m * e end

t1 = now()
print("Tempo: " + (t1 - t0) + " ms")
print("#########################################################")

