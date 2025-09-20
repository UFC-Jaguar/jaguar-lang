using Common.Errors;

namespace Common.Data {
    /* Runtime Gerente */
    public class MemoryManager {
        public TValue Value { get; set; }
        public TError Error { get; set; } 

        public TValue FuncReturn { get; set; }
        public bool LoopContinue { get; set; }
        public bool LoopBreak { get; set; }

        public bool NeedReturn { get { return EvaluateReturn(); } }

        public MemoryManager() {
            this.Reset();
        }
        public void Reset() { 
            this.Value = null;
            this.Error = null;
            this.FuncReturn = null;
            this.LoopContinue = false;
            this.LoopBreak = false;
        }
        public TValue Registry(MemoryManager _runTime) {
            this.Error = _runTime.Error;
            this.FuncReturn = _runTime.FuncReturn;
            this.LoopContinue = _runTime.LoopContinue;
            this.LoopBreak = _runTime.LoopBreak;
            return _runTime.Value;
        }
        public MemoryManager Success(TValue _value) {
            this.Reset();
            this.Value = _value;
            return this;
        }
        public MemoryManager SuccessReturn(TValue _value) {
            this.Reset();
            this.FuncReturn = _value;
            return this;
        }
        public MemoryManager SuccessContinue(){
            this.Reset();
            this.LoopContinue = true;
          return this;
        }
        public MemoryManager SuccessBreak(){
            this.Reset();
            this.LoopBreak = true;
          return this;
        }
        public MemoryManager Fail(TError _error) {
            this.Reset();
            this.Error = _error;
            return this;
        }
        private bool EvaluateReturn(){                  //OBS: Isso irá permitir "return", "continue" e "break" externo ao bloco atual ou function
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
