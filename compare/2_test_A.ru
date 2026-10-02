t0 = now()
load = 256
ini = 0
fim = (load)/size-1
lin = 100
col = 100
matrix = for i = ini : fim #[lin, col] end
m = #[lin, col]

tipo = if size == 1 "Serial" else "Paralelo" end

for e in matrix m = m * e end
gather = || .gather(m)

m = gather / 0
for e in gather m = m * e end

t1 = now()
time_spent = (t1 - t0)/1000
print(""+tipo+" ; "+size+" ; "+((fim+1)*size)+" ("+lin+"x"+col+") ; "+time_spent+" seconds")

