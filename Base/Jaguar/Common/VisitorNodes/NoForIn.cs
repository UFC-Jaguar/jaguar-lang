using System;
using System.Collections.Generic;
using FrontEnd.Lexing;
using Common.Data;
using Common.Errors;
using System.Linq;

namespace Common.Nodes {
    public class NoForIn : Visitor {
        private Token IDTypeToken { get; set; } // TODO: Ver se podemos remover daqui. Como pegaríamos da tabela de símbolos?
        private Visitor BodyStatementsVisitor { get; set; } //= null;
        private Visitor ListExpVisitor { get; set; } //= null;
        private bool ListComprehension { get; set; } //= false;
        public NoForIn(Token idToken, Visitor bodyVisitor, Visitor list_exp_visitor) {
            this.IDTypeToken = idToken;
            this.BodyStatementsVisitor = bodyVisitor;
            this.ListExpVisitor = list_exp_visitor;
            this.ListComprehension = true; // This flag allow for data return
            //this.Value = null;

            this.NOIni = this.IDTypeToken.NOIni;
            this.NOEnd = this.BodyStatementsVisitor.NOEnd;
        }
        public override string ToString() {
            return "(" + IDTypeToken.ToString() + ", " +
                BodyStatementsVisitor.ToString() + ", " +
                ListExpVisitor.ToString() + ")";
        }
        public override DataFlow Visit(JMemory memory) {
            DataFlow dataflow = new DataFlow();
            var elements = new List<TValue>();

            TValue relementsValue = dataflow.update_and_get_value(this.ListExpVisitor.Visit(memory));
            
            if (relementsValue.IsList() == false)
                return dataflow.Fail(new TRunTimeError(this.NOIni, this.NOEnd, "Its' not a list: for exception on the 'elements value' type.", memory));

            if (dataflow.ReFlow())
                return dataflow;

            string varName = this.IDTypeToken.Value;
            memory.SymbolTable.Set(varName, TNull.Get());

            TList elementsValue = (TList) relementsValue;
            foreach (TValue e in elementsValue.VAL){
                memory.SymbolTable.Set(this.IDTypeToken.Value, e.SetMemory(memory));
                TValue value = dataflow.update_and_get_value(this.BodyStatementsVisitor.Visit(memory));
                if (dataflow.ReFlow() && dataflow.LoopContinue == false && dataflow.LoopBreak == false) return dataflow;
                if (dataflow.LoopContinue) continue;
                if (dataflow.LoopBreak) break;
                elements.Add(((TList)value).VAL.Last());
            }
            TList l = new TList(elements);
            TValue v = this.ListComprehension ? l.SetMemory(memory).SetLocation(this.NOIni, this.NOEnd) : Consts.Number.Null;
            this.Value = v;
            return dataflow.SetDefaultAndNewTValue(v);
        }
    }
}
