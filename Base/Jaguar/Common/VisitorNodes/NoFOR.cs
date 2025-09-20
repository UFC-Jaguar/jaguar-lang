using System;
using System.Collections.Generic;
using FrontEnd.Lexing;
using Common.Data;

namespace Common.Nodes {
    public class NoFOR: Visitor {
		private Token TokenVar { get; set; } // TODO: Ver se podemos remover daqui. Como pegaríamos da tabela de símbolos?
        private Visitor StartValue { get; set; } //= null;
        private Visitor EndValue { get; set; } //= null;
        private Visitor StepValue { get; set; } //= null;
        private Visitor Body { get; set; } //= null;
        private bool NeedReturnNull { get; set; } //= false;
        public NoFOR(Token tokenVar, Visitor startValueNode, Visitor endValueNode, Visitor stepValueNode, Visitor bodyNode, bool needReturnNull) {
            this.TokenVar = tokenVar;
            this.StartValue = startValueNode;
            this.EndValue = endValueNode;
            this.StepValue = stepValueNode;
            this.Body = bodyNode;
            this.NeedReturnNull = needReturnNull;

            this.NOIni = this.TokenVar.NOIni;
            this.NOEnd = this.Body.NOEnd;
        }
        public override string ToString() {
            string step = StepValue != null ? StepValue.ToString() + ", ": "";
            return "("+TokenVar.ToString() + ", " + 
                StartValue.ToString() + ", " +
                EndValue.ToString() + ", " +
                step + 
                Body.ToString()+")";
        }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            var elements = new List<TValue>();
            TValue startValue = manager.Registry(this.StartValue.Visit(memory));
            if (startValue.GetType() != typeof(TNumber)) {  // TODO: Verificar depois. Casar tipos?
                new Exception("visit ForNode: Interpreter identified exception on startValue"); 
            }
            if (manager.NeedReturn) 
                return manager;
            TValue endValue = manager.Registry(this.EndValue.Visit(memory));
            if (endValue.GetType() != typeof(TNumber)) { 
                new Exception("visit ForNode: Interpreter identified exception on endValue"); 
            }
            if (manager.NeedReturn) 
                return manager;

            TValue stepValue = new TNumber(1);

            if (this.StepValue != null) {
                stepValue = manager.Registry(this.StepValue.Visit(memory));
                if (manager.NeedReturn) 
                    return manager;
            }

            var i = ((TNumber)startValue).VAL;
            Func<bool> condition = () => i > ((TNumber)endValue).VAL;

            if (((TNumber)stepValue).VAL >= 0) {
                condition = () => i < ((TNumber)endValue).VAL;
            }

            while (condition()) {
                memory.SymbolTable.Set(this.TokenVar.Value, new TNumber(i));
                i += ((TNumber)stepValue).VAL;

                TValue value = manager.Registry(this.Body.Visit(memory));
                if (manager.NeedReturn && manager.LoopContinue == false && manager.LoopBreak == false) 
                    return manager;
                if (manager.LoopContinue) 
                    continue;
                if (manager.LoopBreak) 
                    break;
                elements.Add(value);
            }
            TList l = new TList(elements);
            TValue v = this.NeedReturnNull ? Consts.Number.Null : l.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd);
            this.Value = v;
            return manager.Success(v); 
        }
    }
}
