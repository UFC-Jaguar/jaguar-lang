using Common.Data;
using Common.Environment;
using Common.Helpers;
using Common.Nodes;
using Common.Errors;
using MPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

// TODO: Rever Tipos
namespace FrontEnd.Nodes { 
    public class NoPAtomCall : Visitor {
        private string[] msg_scatter = {
                "NOTE: scatter need a list length = number of proccess.\n" +
                "Exemplos (for 4 process):\n" +
                "   #> a = 10; b = 20; c=30; d=40;\n" +
                "   #> x = ||.scatter([a,b,c,d]);\n" + 
                "   #> allprint(\"PID=\" + pid + \" -> \" + x)\n" + 
                "   #> list = [a,b,c,d]\n" + 
                "   #> allprint(||.scatter(list))\n" + 
                "   #> allprint(||.scatter([10,20,30,40]))\n" + 
                "So, the print emite: 10, 20, 30, 40 (for 0,1,2,3 pid process)", 
                "\n      Parameter Error (Variable: NoVarAccess Null)",
                "\n      Parallel Method not found!!!!!!!!!!!!!!"};

        public NoVarAccess IDToCall = null;
        public Visitor[] ArgsVisitors = null;
        private readonly string metodo = "";
        public NoPAtomCall(NoVarAccess idToCall, Visitor[] args, string metodo) {
            this.IDToCall = idToCall;
            this.ArgsVisitors = args;

            this.NOIni = this.IDToCall.NOIni;
            this.metodo = metodo;

            if (this.ArgsVisitors.Length > 0)
                this.NOEnd = this.ArgsVisitors[this.ArgsVisitors.Length - 1].NOEnd;
            else
                this.NOEnd = this.IDToCall.NOEnd;
        }
        public override MemoryManager Visit(JMemory memory) {
            NoVarAccess node = this.IDToCall;
            string methodName = metodo;
            MethodInfo m = Util.SelectMethod(this, "visit_", methodName);

            object[] parameters = { memory };
            MemoryManager valor = new MemoryManager();
            if (m != null) {
                return (MemoryManager)m.Invoke(this, parameters);
            }
            MemoryManager manager = new MemoryManager();
            return this.DefaultError(manager, memory, msg_scatter[2]);
        }
        public MemoryManager visit_sum(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            float valor = 0, temp = 0;
            for (int i = 0; i < ArgsVisitors.Length; i++) {
                if (ArgsVisitors[i].GetType() == typeof(NoNumber)) {
                    NoNumber numero = (NoNumber)ArgsVisitors[i];
                    temp = float.Parse(numero.Tok.Value);
                } else {
                    NoVarAccess van = (NoVarAccess)ArgsVisitors[i];
                    TValue val = memory.SymbolTable.Get(van.VarNameTOK.Value);
                    temp = (float)((TNumber)val).Value;
                }
                valor += temp;
            }
            valor = MPIEnv.Comm_world.Reduce<float>(valor, Operation<float>.Add, MPIEnv.Root);
            MPIEnv.Comm_world.Barrier(); // Tarefas vao aguardar aqui
                                         //System.Threading.Thread.Sleep(10000);

            TValue return_value = new TNumber(valor, memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return manager.Success(return_value);
        }
        public MemoryManager visit_ip(JMemory memory) {
            MemoryManager rtr = new MemoryManager();
            TValue return_value = new TString(Util.GetLocalIPAddress(), memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return rtr.Success(return_value);
        }
        //******************************
        public MemoryManager visit_scatter(JMemory memory) {// scatter()
            MemoryManager manager = new MemoryManager();
            if (ArgsVisitors.Length != 1) 
                return this.DefaultError(manager, memory, msg_scatter[0]);

            object[] values = new object[MPIEnv.Size];
            int n = 0;
            if (MPIEnv.Rank == MPIEnv.Root) {
                List<object> dados = new List<object>();
                if (ArgsVisitors[0].GetType() == typeof(NoList)) {
                    NoList list = (NoList)ArgsVisitors[0];
                    foreach (Visitor v in list.Elements) {
                        TValue x = v.Value;
                        if (v.GetType() == typeof(NoVarAccess)) x = this.Valor((NoVarAccess)v, memory);
                        else if (v.Value.Null) x = manager.Registry(v.Visit(memory));
                        if (!x.Null) dados.Add(x);
                    }
                }
                else if (ArgsVisitors[0].GetType() == typeof(NoVarAccess)) {
                    TValue v = this.Valor((NoVarAccess)ArgsVisitors[0], memory);
                    if (v == null || (v.GetType() != typeof(TList))) 
                        return this.DefaultError(manager, memory, msg_scatter[1]);
                    TList l = (TList)v;
                    foreach (TValue x in l.VAL) {
                        dados.Add(x);
                    }
                }
                values = dados.ToArray();
                n = dados.Count;
            }
            n = MPIEnv.Comm_world.Allreduce<int>(n, Operation<int>.Max);
            if (n == MPIEnv.Size) { // Coleta de dados ok
                object valor = MPIEnv.Comm_world.Scatter<object>(values, MPIEnv.Root);
                MPIEnv.Comm_world.Barrier();
                TValue num = (TValue)valor; //new TString("....", memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
                return manager.Success(num);
            }
            TValue return_value = new TString(msg_scatter[0], memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return manager.Success(return_value);
        }
        private TValue Valor(NoVarAccess node, JMemory memory) {
            return memory.SymbolTable.Get(node.VarNameTOK.Value);
        }
        private MemoryManager DefaultError(MemoryManager manager, JMemory _memory, string mensagem) {
            var _NOIni = this.NOIni;
            if (this.ArgsVisitors.Length>1) _NOIni = this.ArgsVisitors[this.ArgsVisitors.Length-1].NOIni;
            return manager.Fail(new TRunTimeError(_NOIni, this.NOEnd, mensagem, _memory));
        }
    }
}

