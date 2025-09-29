def source()
	if manager do
		return [[1,2,3],[4],[5,6],[7,8,9,10]]
	end
	return []
end
dados = source()
#allprint("p"+pid+"("+||.ip() + ") -> " + dados)

print("################## Scatter #########################")
x = ||.scatter(dados)
allprint("p" + pid + "(" + || .ip() + ") -> " + x)
print("############# Node sum #############################")
n = 0
for i=0 to len(x) do
	# NOTE: get(x,i)==x/i. In the future release (on c++), the value of list will be x!!i, like haskell
	n = n + get(x,i)
end
allprint("p" + pid + "(" + || .ip() + ") -> " + n)

print("############## Using ||.gather(n) ##################")
r = ||.gather(n)
if manager do
    g = 0
	for i = 0 to len(r) do g = g + r/i
	print("sum all and send to root -> " + g)
end

print("############### Using ||.sum(n) ####################")
s = ||.sum(n)
if manager do
	print("sum all and send to root -> " + s)
end
