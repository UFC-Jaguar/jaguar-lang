using System;
using Common.Helpers;
using Common.Errors;
using System.Reflection;
using Common.Nodes;
using Common.Environment;
using MPI;

namespace Common.Data {
    [Serializable()]
    public class TEmbeddedFunction : TBaseFunction {
        public TEmbeddedFunction(string _functionName) : base(_functionName) { }

        public override MemoryManager Run(TValue[] args) {
            MemoryManager managerRunner = new MemoryManager();
            JMemory newMemory = this.GenerateNewMemory();

            MethodInfo m = Util.SelectMethod(this, Consts.EmbeddedFunction.INI, this.EmbeddedFunctionName);
            object[] parameters = { newMemory };
            managerRunner.Registry(this.CheckAndPopulateArgs(Consts.ArgNames.Get(this.EmbeddedFunctionName), args, newMemory));
            if (managerRunner.NeedReturn) return managerRunner;
            if (m != null) {
                TValue returnValue = managerRunner.Registry((MemoryManager)m.Invoke(this, parameters));
                if (managerRunner.NeedReturn) return managerRunner;
                this.Value = returnValue;
                return managerRunner.Success(returnValue);
            }
            return managerRunner.Fail(Util.NoVisitMethod(this, newMemory, Consts.EmbeddedFunction.INI, this.EmbeddedFunctionName));
        }
        public override TValue Copy() {
            TEmbeddedFunction copy = new TEmbeddedFunction(this.EmbeddedFunctionName);
            copy.SetMemory(this.memory);
            copy.SetLocation(this.NOIni, this.NOEnd);
            return copy;
        }
        public override string ToString() {
            return "<embedded function " + this.EmbeddedFunctionName + ">";
        }
        //#####################################
        public MemoryManager Run_print(JMemory memoryData) {
            if (MPIEnv.Rank == MPIEnv.Root) {
                System.Console.WriteLine(memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).ToString());
            }
            return new MemoryManager().Success(Consts.Number.Null);
        }
        public MemoryManager Run_allprint(JMemory memoryData) {
            System.Console.WriteLine(memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).ToString());
            MPIEnv.Comm_world.Barrier();
            return new MemoryManager().Success(Consts.Number.Null);
        }
        public MemoryManager Run_str(JMemory memoryData) {
            return new MemoryManager().Success( ( new Common.Data.TString(memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).ToString())  )  );
        }
        public MemoryManager Run_input(JMemory memoryData) {
            string text = Util.ReadText("### ");
            return new MemoryManager().Success(new Common.Data.TString(text));
        }
        public MemoryManager Run_input_int(JMemory memoryData) {
            int number;// = 0;
            while (true) {
                string text = Util.ReadText("### ");
                try {
                    number = int.Parse(text);
                    break;
                } catch {
                    System.Console.WriteLine("'" + text + "' must be an integer. Try again!");
                }
            }
            return new MemoryManager().Success(new TNumber(number));
        }
        public MemoryManager Run_clear(JMemory memoryData) {
            Console.Clear();
            return new MemoryManager().Success(Consts.Number.Null);
        }
        public MemoryManager Run_is_number(JMemory memoryData) {
            return (memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).GetType() == typeof(TNumber)) ?
                new MemoryManager().Success(Consts.Number.True) : new MemoryManager().Success(Consts.Number.False);
        }
        public MemoryManager Run_is_string(JMemory memoryData) {
            return (memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).GetType() == typeof(Common.Data.TString)) ?
                new MemoryManager().Success(Consts.Number.True) : new MemoryManager().Success(Consts.Number.False);
        }
        public MemoryManager Run_is_list(JMemory memoryData) {
            return (memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).GetType() == typeof(Common.Data.TList)) ?
                new MemoryManager().Success(Consts.Number.True) : new MemoryManager().Success(Consts.Number.False);
        }
        public MemoryManager Run_is_function(JMemory memoryData) {
            return (memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue).GetType() == typeof(TBaseFunction)) ?
                new MemoryManager().Success(Consts.Number.True) : new MemoryManager().Success(Consts.Number.False);
        }
        public MemoryManager Run_push(JMemory memoryData) {
            TValue lista = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KList);
            TValue value = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue);

            if (lista.GetType() != typeof(TList)) {
                return new MemoryManager().Fail(new TRunTimeError(
                  this.NOIni, this.NOEnd,
                  "The first argument must be a list",
                  memoryData
                ));
            }
          ((TList)lista).VAL.Add(value);
            return new MemoryManager().Success(lista);
        }
        public MemoryManager Run_get(JMemory memoryData) {
            TValue lista = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KList);
            TValue index = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KIndex);

            if (lista.GetType() != typeof(TList)) {
                return new MemoryManager().Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    "The first argument must be a list",
                    memoryData
                ));
            }
            if (index.GetType() != typeof(TNumber)) {
                return new MemoryManager().Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    "The second argument must be a integer number (index)",
                    memoryData
                ));
            }
            TValue element;// = null;
            try {
                TList l = (TList)lista;
                float? num = ((TNumber)index).VAL;
                int i = num != null ? ((int)num) : -1; //Notar que num pode ser float, de modo que a parte inteira eh usada
                element = (TValue)l.VAL[i];
                l.VAL.RemoveAt(i);
            } catch {
                return new MemoryManager().Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    "Element at this index could Not be removed from list because index is out of bounds",
                    memoryData
                ));
            }
            return new MemoryManager().Success(element);
        }
        public MemoryManager Run_len(JMemory memoryData) {
            TValue list_ = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KList);

            if (list_.GetType() != typeof(TList)) {
              return new MemoryManager().Fail(new TRunTimeError(
                this.NOIni, this.NOEnd,
                "Argument must be a list",
                memoryData
              ));
            }
            return new MemoryManager().Success(new TNumber(((TList)list_).VAL.Count));
        }
        public MemoryManager Run_include(JMemory memoryData) {
            TValue fn_ = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KFileName);

            if (fn_.GetType() != typeof(Common.Data.TString)) {
                return new MemoryManager().Fail(new TRunTimeError(
                  this.NOIni, this.NOEnd,
                  "The second argument must be a string",
                  memoryData
                ));
            }
            string fileName = ((Common.Data.TString)fn_).VAL;
            string contentFromFileName;
            try {
                //string current_folder = System.Environment.CurrentDirectory;
                contentFromFileName = System.IO.File.ReadAllText(fileName);
            } catch {
                return new MemoryManager().Fail(new TRunTimeError(
                  this.NOIni, this.NOEnd,
                  "Failed to load contentFromFileName \"" + fileName + "\"\n" + " :: File Not found",
                  memoryData
                ));
            }
            var result = Running.Run(fileName, contentFromFileName);

            if (result.Error != null) {
                return new MemoryManager().Fail(new TRunTimeError(
                  this.NOIni, this.NOEnd,
                  "Failed to finish executing contentFromFileName \"" + fileName + "\"\n" +
                  result.Error.ToString(),
                  memoryData
                ));
            }
            return new MemoryManager().Success(Consts.Number.Null); 
        }

        public MemoryManager Run_mpi_sum(JMemory memoryData) {
            float x = (float) ((TNumber) memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KValue)).Value;
            return new MemoryManager().Success(new TNumber(x));
        }
    }
}

/*public MemoryManager Run_concat_list(JMemory memoryData) {
    TValue listA = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KListA);
    TValue listB = memoryData.SymbolTable.Get(Consts.ArgNames.Keys.KListB);
    if (listA.GetType() != typeof(TList)) {
        return new MemoryManager().Fail(new TRunTimeError(
          this.NOIni, this.NOEnd,
          "The first argument must be a list",
          memoryData
        ));
    }
    if (listB.GetType() != typeof(TList)) {
        return new MemoryManager().Fail(new TRunTimeError(
          this.NOIni, this.NOEnd,
          "The second argument must be a list",
          memoryData
        ));
    }
  ((TList)listA).elements.AddRange(((TList)listB).elements);
    return new MemoryManager().Success(listA);
}*/
