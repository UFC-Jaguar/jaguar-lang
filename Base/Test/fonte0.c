# comment
print("Have a good day")
def tester(p) -> p + "|||||"

def merge1(list, separa)
  let ret = ""
  let n = len(list)
  for i = 0 to n do
    let ret = ret + list/i
    let ret = ret + separa
  end
  return ret
end
def merge2(list, separa)
  let ret = ""
  let n = len(list)
  for i = 0 to n do
    let ret = ret + list/i
    if (i != n - 1) do let ret = ret + separa
  end
  return ret
end
def map(list, func)
  let new_list = []
  for i = 0 to len(list) do
    append(new_list, func(list/i))
  end
  return new_list
end
for i = 0 to 5 do
  let x = i
  #print(merge1([tester("Joao"), tester("Silva")], ", "))
  #print(merge2(map(["l", "sp"], tester), ", "))
end

def fibo(n)
  if (n<2) do return n
  let a = 0
  let b = 1
  for i = 1 to n do
    let c = a+b
    let a = b
    let b = c
  end
  return c
end

let n = 10
print("Fibo de ")
print(n)
print("Eh: ")
#print(fibo(10))

