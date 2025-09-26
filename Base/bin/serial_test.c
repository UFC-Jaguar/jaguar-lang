#// Isto eh um comentario
#// Funcao interna "print":
  def ola()
    print("Teste Funcao ola()!!!")
  end
#// Funcao iterativo de fibonacci:
  def ifibo(n)
    if (n < 2) do return n
    let a = 0
    let b = 1
    let c = a + b
    for i = 1 to n do
      let c = a + b
      let a = b
      let b = c
    end
    return c
  end
#// Funcao recursivo de fibonacci:
  def rfibo(n)
    if (n < 2) do return n
    return rfibo(n - 1) + rfibo(n - 2)
  end
#// Funcao inline e lambda:
  def soma(a, b) -> a + b
  let lambda = def (a, b) -> a + b
#// Testes:
let n = 10
 let lista = ["Joao", "Maria", "Ana", 10, 20, 30.1]
 print("Executando arquivo!!!")
 print("Valor de n: " + n)
 ola()
 print(lista)
 print("Funcao soma: " + soma(n, 7))
 print("Lambda: " + lambda(100, 7))
 let x = lambda; print("LambdaX: " + x(100, 7))
 print("iFibo(" + n + ") = " + ifibo(n))
 print("Recursivo, Fibo(" + n + ")=" + ifibo(n))
 print("Oi!!!")

def fibo(n); let a = 0; let b = 1; let c = a + b; for i = 1 to n do; let c = a + b; let a = b; let b = c; end; return c; end
print(fibo(n))
def test(); let num = 5; return num; end
print(test())
let a = []
for i = 0 to 13 do; if i == 5 do continue elif i == 9 do break; let a = a + i; end
print(a)
let a = []
let i = 0
while i < 13 do; let i = i + 1; if i == 5 do continue; if i == 9 do break; let a = a + i; end
print(i)
print(a)

