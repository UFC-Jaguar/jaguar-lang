using Common.Errors;
using System;

namespace Common.Data {
    /* Runtime Gerente */
    public sealed class DataFlow { 
    //public class DataFlow {
        public TValue Value { get; set; }
        public TError Error { get; set; } 

        public TValue FuncReturn { get; set; }
        public bool LoopContinue { get; set; }
        public bool LoopBreak { get; set; }

        public bool NeedReturn { get { return ReFlow(); } }

        // Lazy<T> handles thread-safety and lazy initialization automatically
        private static readonly Lazy<DataFlow> _lazy = new Lazy<DataFlow>(() => new DataFlow());

        // Public property to access the instance
        public static DataFlow Instance => _lazy.Value;

        public DataFlow() {
            this.defaults();
        }
        public void defaults() { 
            this.Value = null;
            this.Error = null;
            this.FuncReturn = null;
            this.LoopContinue = false;
            this.LoopBreak = false;
        }
        public TValue update_and_get_value(DataFlow _runTime) {
            this.Error = _runTime.Error;
            this.FuncReturn = _runTime.FuncReturn;
            this.LoopContinue = _runTime.LoopContinue;
            this.LoopBreak = _runTime.LoopBreak;
            return _runTime.Value;
        }
        public DataFlow SetDefaultAndNewTValue(TValue _value) {
            this.defaults();
            this.Value = _value;
            return this;
        }
        public DataFlow ReturnOk(TValue _value) {
            this.defaults();
            this.FuncReturn = _value;
            return this;
        }
        public DataFlow ContinueOk(){
            this.defaults();
            this.LoopContinue = true;
          return this;
        }
        public DataFlow BreakOk(){
            this.defaults();
            this.LoopBreak = true;
          return this;
        }
        public DataFlow Fail(TError _error) {
            this.defaults();
            this.Error = _error;
            return this;
        }
        public bool ReFlow(){                  //OBS: Isso irá permitir "return", "continue" e "break" externo ao bloco atual ou function
            if (this.Error != null) return true;        // Houve erro
            if (this.FuncReturn != null) return true;   // Deve acionar return
            if (this.LoopContinue) return true;         // Deve acionar continue
            if (this.LoopBreak) return true;            // Deve acionar break
            return false;
        }
        public override string ToString() {
            string sb = "";
            if (this.Value != null)
                sb = this.Value + "";
            else sb = "null";
            if (this.Error != null)
                sb = "MemoryManager Fail:\n" + Error.ToString();
            return sb;
        }
    }
}
