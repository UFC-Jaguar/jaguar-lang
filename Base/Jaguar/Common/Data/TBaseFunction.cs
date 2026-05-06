using Common.Errors;
using System;
namespace Common.Data {
    [Serializable()]
    public abstract class TBaseFunction : TValue {
        public string EmbeddedFunctionName { get; set; }
        public TBaseFunction(string _name) : base() {
            this.EmbeddedFunctionName = "<Not defined>";
            this.EmbeddedFunctionName = _name?? "<anonymous>"; /* memoryName != null ? memoryName : "<anonymous>"; */
        }
        public JMemory GenerateNewMemory(){
            JMemory newMemory = new JMemory(this.EmbeddedFunctionName, this.memory, this.NOIni);
            newMemory.SymbolTable = new JSymbolTable(newMemory.Parent.SymbolTable);
            return newMemory;
        }
        public DataFlow CheckArgs(string[] argNames, TValue[] args){
            DataFlow manager = new DataFlow();
            if (args.Length > argNames.Length) {
                return manager.Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    (args.Length-argNames.Length)+" many args " + this,
                    this.memory
                ));
            }
            if (args.Length < argNames.Length) {
                return manager.Fail(new TRunTimeError(
                    this.NOIni, this.NOEnd,
                    (argNames.Length-args.Length)+" few args " + this,
                    this.memory
                ));
            }
            return manager.SetDefaultAndNewTValue(null);
        }
        public void PopulateArgs(string[] argNames, TValue[] args, JMemory memoryData) {
            for (int i = 0; i < args.Length; i++) {
                string argName = argNames[i];
                TValue argValue = args[i];
                argValue.SetMemory(memoryData);
                memoryData.SymbolTable.Set(argName, argValue);
            }
        }
        public DataFlow CheckAndPopulateArgs(string[] argNames, TValue[] args, JMemory memoryData) {
            DataFlow manager = new DataFlow();
            manager.update_and_get_value(this.CheckArgs(argNames, args));
            if (manager.NeedReturn) return manager;
            this.PopulateArgs(argNames, args, memoryData);
            return manager.SetDefaultAndNewTValue(null);
        }
    }
}

