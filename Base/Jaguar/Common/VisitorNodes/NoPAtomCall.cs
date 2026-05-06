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
        private string[] msg_gather = { "NOTE: gather need one value object!" };

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
        public override DataFlow Visit(JMemory memory) {
            NoVarAccess node = this.IDToCall;
            string methodName = metodo;
            MethodInfo m = Util.SelectMethod(this, "visit_", methodName);

            object[] parameters = { memory };
            DataFlow valor = new DataFlow();
            if (m != null) {
                return (DataFlow)m.Invoke(this, parameters);
            }
            DataFlow manager = new DataFlow();
            return this.DefaultError(manager, memory, msg_scatter[2]);
        }
        public DataFlow visit_sum(JMemory memory) {
            DataFlow manager = new DataFlow();
            double valor = 0, temp = 0;
            for (int i = 0; i < ArgsVisitors.Length; i++) {
                if (ArgsVisitors[i].GetType() == typeof(NoNumber)) {
                    NoNumber numero = (NoNumber)ArgsVisitors[i];
                    temp = double.Parse(numero.Tok.Value);
                } else {
                    NoVarAccess van = (NoVarAccess)ArgsVisitors[i];
                    TValue val = memory.SymbolTable.Get(van.VarNameTOK.Value);
                    temp = (double)((TNumber)val).Value;
                }
                valor += temp;
            }
            valor = MPIEnv.Comm_world.Reduce<double>(valor, Operation<double>.Add, MPIEnv.Root);
            MPIEnv.Comm_world.Barrier(); // Tarefas vao aguardar aqui
                                         //System.Threading.Thread.Sleep(10000);

            TValue return_value = new TNumber(valor, memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return manager.SetDefaultAndNewTValue(return_value);
        }
        public DataFlow visit_ip(JMemory memory) {
            DataFlow rtr = new DataFlow();
            TValue return_value = new TString(Util.GetLocalIPAddress(), memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return rtr.SetDefaultAndNewTValue(return_value);
        }
        //******************************
        public DataFlow visit_gather(JMemory memory) {
            DataFlow manager = new DataFlow();
            if (ArgsVisitors.Length != 1)
                return this.DefaultError(manager, memory, msg_gather[0]);

            TValue gather_value = Consts.Number.Null;
            Visitor visitor = (Visitor)ArgsVisitors[0];
            TValue tval = visitor.Value;
            if (visitor.Value.Null)
                tval = manager.update_and_get_value(visitor.Visit(memory));
            if (!tval.Null)
                gather_value = tval;
            TValue[] mpi_gather_values = MPIEnv.Comm_world.Gather<TValue>(gather_value, MPIEnv.Root);
            TList result = new TList(new List<TValue>());
            if (MPIEnv.Rank == MPIEnv.Root)
                result = new TList(new List<TValue>(mpi_gather_values));
            MPIEnv.Comm_world.Barrier();
            return manager.SetDefaultAndNewTValue(result.SetLocation(this.NOIni, this.NOEnd).SetMemory(memory));
        }

        public DataFlow visit_scatter(JMemory memory) {// scatter()
            DataFlow manager = new DataFlow();
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
                        else if (v.Value.Null) x = manager.update_and_get_value(v.Visit(memory));
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
                return manager.SetDefaultAndNewTValue(num);
            }
            TValue return_value = new TString(msg_scatter[0], memory).SetLocation(this.NOIni, this.NOEnd).SetMemory(memory);
            return manager.SetDefaultAndNewTValue(return_value);
        }
        private TValue Valor(NoVarAccess node, JMemory memory) {
            return memory.SymbolTable.Get(node.VarNameTOK.Value);
        }
        private DataFlow DefaultError(DataFlow manager, JMemory _memory, string mensagem) {
            var _NOIni = this.NOIni;
            if (this.ArgsVisitors.Length>1) _NOIni = this.ArgsVisitors[this.ArgsVisitors.Length-1].NOIni;
            return manager.Fail(new TRunTimeError(_NOIni, this.NOEnd, mensagem, _memory));
        }
    }
}

