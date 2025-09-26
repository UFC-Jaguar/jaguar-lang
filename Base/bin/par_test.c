if pid == 0 do
	dados = [[1,2,3],[4],[5,6],[7,8,9,10]]
else
	dados = []
end

#allprint("p"+pid+"("+||.ip() + ") -> " + dados)

x = ||.scatter(dados)
#allprint("p" + pid + "(" + || .ip() + ") -> " + x)

n = 0
for i=0 to len(x) do
	n = n + x/i
end

r = ||.sum(n)
allprint("p" + pid + "(" + || .ip() + ") -> " + n)
if pid == 0 do
	print("sum all and send to root -> " + r)
end
