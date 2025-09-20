using System;
using System.Collections.Generic;
using Common.Data;

/* TODO: NoIF precisa ser refatorado. O ideal é que tenhamos algo assim, usando visitor para cada noVisitor:
  <IfExp> ::= KEY:IF <Exp> KEY:DO (<Stm> [(<Elif> | <Else>)]
                                   | NEWLINE <Statements> (KEY:END | <Elif> | <Else>)
                                  )
  <Elif>  ::= KEY:ELIF <Exp> KEY:DO (<Stm> [(<Elif> | <Else>)]
                                     | NEWLINE <Statements> (KEY:END | <Elif> | <Else>)
                                    )
  <Else>  ::= KEY:ELSE (<Stm>
                        | (NEWLINE <Statements> KEY:END)
                       )
  Note que a primeira opção é inline. A segunda envolve NEWLINE
*/
namespace Common.Nodes {
    public class NoIF: Visitor {
        private readonly List<NoDataIFs> __QIFs = null;
        private readonly NoElse __ELSE = null;

        public List<NoDataIFs> QueueIFs { get { return __QIFs; } }
        public NoElse Else { get { return __ELSE; } }
        public NoIF(List<NoDataIFs> _queueIfs, NoElse _else){ // TODO: este construtor pode ser melhorado?
            this.__QIFs = _queueIfs;
            this.__ELSE = _else;
            if (this.QueueIFs.Count > 0) {
                this.NOIni = this.QueueIFs[0].Item1.NOIni;
                this.NOEnd = this.QueueIFs[this.QueueIFs.Count - 1].Item1.NOEnd;
            }
            if (this.Else != null)
                this.NOEnd = this.Else.Item1.NOEnd;
        }
        public override string ToString() {
            string sb = "["; string space = ", "; int c = 0;
            foreach (var tp in QueueIFs) {
                sb = sb+"("+tp.Item1.ToString()+", "+tp.Item2.ToString()+")";
                sb = ++c == QueueIFs.Count ? sb : sb + space;
            }
            sb = sb + "]";
            return sb;
        }
        public static NoElse ElseInstance(Visitor n, bool b) { return new NoElse(n, b); }
        public static NoDataIFs DataIFInstance(Visitor n1, Visitor n2, bool b) { return new NoDataIFs(n1, n2, b); }
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            foreach (var tp in this.QueueIFs) {
                Visitor condition = tp.Item1;
                Visitor exp = tp.Item2;
                bool needReturnNull = tp.Item3;
                TValue conditionValue = manager.Registry(condition.Visit(memory)); //TODO: Ver isso
                if (manager.NeedReturn) 
                    return manager;

                if (conditionValue.IsTrue()) {
                    TValue expValue = manager.Registry(exp.Visit(memory));
                    if (manager.NeedReturn) 
                        return manager;
                    TValue v = needReturnNull ? Consts.Number.Null : expValue;
                    return manager.Success(v);
                }
            }
            if (this.Else != null) {
                Visitor exp = this.Else.Item1;
                bool needReturnNull = this.Else.Item2;
                TValue expValue = manager.Registry(exp.Visit(memory));
                if (manager.NeedReturn) 
                    return manager;
                TValue v = needReturnNull ? Consts.Number.Null : expValue;
                return manager.Success(v);
            }
            return manager.Success(Consts.Number.Null);
        }
        //################################################ VAL ELSE:
        public class NoElse : Visitor {
            private Tuple<Visitor, bool> __VISITOR_TUPLE = null;
            public Tuple<Visitor, bool> VisitorTuple { get { return __VISITOR_TUPLE; } }
            public Visitor Item1 { get { return VisitorTuple.Item1; } }
            public bool Item2 { get { return VisitorTuple.Item2; } }
            public NoElse(Visitor node, bool ret) {
                __VISITOR_TUPLE = new Tuple<Visitor, bool>(node, ret);
            }
            public override MemoryManager Visit(JMemory memory) { // TODO: Ver isso. Podemos tratar algo aqui no visit?
                throw new NotImplementedException();
            }
        }
        //################################################ VAL ELIF:
        public class NoDataIFs : Visitor {
            private Tuple<Visitor, Visitor, bool> __VISITOR_TUPLE = null;
            
            public Visitor Item1 { get { return __VISITOR_TUPLE.Item1; } }
            public Visitor Item2 { get { return __VISITOR_TUPLE.Item2; } }
            public bool Item3 { get { return __VISITOR_TUPLE.Item3; } }
            public NoDataIFs(Visitor no1, Visitor no2, bool ret) {
                __VISITOR_TUPLE = new Tuple<Visitor, Visitor, bool>(no1, no2, ret);
            }
            public override MemoryManager Visit(JMemory memory) { // TODO: Ver isso. Podemos tratar algo aqui no visit?
                throw new NotImplementedException();
            }
        }
    }
}
