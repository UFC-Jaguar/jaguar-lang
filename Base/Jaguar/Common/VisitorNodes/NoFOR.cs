using System;
using System.Collections.Generic;
using FrontEnd.Lexing;
using Common.Data;
using Common.Errors;
using System.Linq;

namespace Common.Nodes {
    public class NoFOR: Visitor {
		private Token IDTypeToken { get; set; } // TODO: Ver se podemos remover daqui. Como pegaríamos da tabela de símbolos?
        private Visitor StartExpVisitor { get; set; } //= null;
        private Visitor StopExpVisitor { get; set; } //= null;
        private Visitor IncrementalExpVisitor { get; set; } //= null;
        private Visitor BodyStatementsVisitor { get; set; } //= null;
        private bool ListComprehension { get; set; } //= false;
        public NoFOR(Token idToken, Visitor start_exp_visitor, Visitor stop_exp_visitor, Visitor incremental_exp_visitor, Visitor statements_body_visitor) {
            this.IDTypeToken = idToken;
            this.StartExpVisitor = start_exp_visitor;
            this.StopExpVisitor = stop_exp_visitor;
            this.IncrementalExpVisitor = incremental_exp_visitor;
            this.BodyStatementsVisitor = statements_body_visitor;
            this.ListComprehension = true; // This flag allow for data return

            this.NOIni = this.IDTypeToken.NOIni;
            this.NOEnd = this.BodyStatementsVisitor.NOEnd;
        }
        public override string ToString() {
            string step = IncrementalExpVisitor != null ? IncrementalExpVisitor.ToString() + ", ": "";
            return "("+IDTypeToken.ToString() + ", " + 
                StartExpVisitor.ToString() + ", " +
                StopExpVisitor.ToString() + ", " +
                step + 
                BodyStatementsVisitor.ToString()+")";
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow dataflow = new DataFlow();
            var elements = new List<TValue>();
            
            TValue startValue = dataflow.update_and_get_value(this.StartExpVisitor.Visit(memory));
            
            if (!startValue.IsInt())
                return dataflow.Fail(new TRunTimeError(this.NOIni, this.NOEnd, "'startValue' on for is not integer", memory));

            if (dataflow.NeedReturn) 
                return dataflow;

            //if (startValue.GetType() != typeof(TNumber)) {  // TODO: Verificar depois. Casar tipos?
            //    //new Exception("visit ForNode: Interpreter identified exception on startValue");
            //    return dataflow.Fail(new TRunTimeError(this.NOIni, this.NOEnd,"'startValue' on for is not number", memory));
            //}
            
            TValue endValue = dataflow.update_and_get_value(this.StopExpVisitor.Visit(memory));
            
            if (endValue.GetType() != typeof(TNumber)) { 
                new Exception("visit ForNode: Interpreter identified exception on endValue"); 
            }
            if (dataflow.NeedReturn) 
                return dataflow;

            TValue stepValue = new TNumber(1);

            if (this.IncrementalExpVisitor != null) {
                stepValue = dataflow.update_and_get_value(this.IncrementalExpVisitor.Visit(memory));
                if (dataflow.NeedReturn) 
                    return dataflow;
            }

            var i = ((TNumber)startValue).VAL;
            Func<bool> condition = () => i > ((TNumber)endValue).VAL;

            if (((TNumber)stepValue).VAL >= 0) {
                condition = () => i < ((TNumber)endValue).VAL;
            }

            while (condition()) {
                memory.SymbolTable.Set(this.IDTypeToken.Value, new TNumber(i));
                i += ((TNumber)stepValue).VAL;

                TValue value = dataflow.update_and_get_value(this.BodyStatementsVisitor.Visit(memory));
                if (dataflow.NeedReturn && dataflow.LoopContinue == false && dataflow.LoopBreak == false) 
                    return dataflow;
                if (dataflow.LoopContinue) 
                    continue;
                if (dataflow.LoopBreak) 
                    break;
                List<TValue> lista = ((TList)value).VAL;
                elements.Add(lista.Last());
            }
            TList l = new TList(elements);
            TValue v = this.ListComprehension ? l.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd): Consts.Number.Null;
            this.Value = v;
            return dataflow.SetDefaultAndNewTValue(v); 
        }
    }
}
