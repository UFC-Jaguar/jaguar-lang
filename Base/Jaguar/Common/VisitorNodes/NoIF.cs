using System;
using System.Collections.Generic;
using Common.Data;

namespace Common.Nodes {
    public class NoIF: Visitor {
        private readonly List<NoDataIFs> __Cases = null;
        private readonly NoElse __ELSE = null;

        public List<NoDataIFs> IFCases { get { return __Cases; } }
        public NoElse Else { get { return __ELSE; } }
        public NoIF(List<NoDataIFs> _queueIfs, NoElse _else){ // TODO: este construtor pode ser melhorado?
            this.__Cases = _queueIfs;
            this.__ELSE = _else;
            if (this.IFCases.Count > 0) {
                this.NOIni = this.IFCases[0].Condition.NOIni;
                this.NOEnd = this.IFCases[this.IFCases.Count - 1].Condition.NOEnd;
            }
            if (this.Else != null)
                this.NOEnd = this.Else.Body.NOEnd;
        }
        public override string ToString() {
            string sb = "["; string space = ", "; int c = 0;
            foreach (var tp in IFCases) {
                sb = sb+"("+tp.Condition.ToString()+", "+tp.Body.ToString()+")";
                sb = ++c == IFCases.Count ? sb : sb + space;
            }
            sb = sb + "]";
            return sb;
        }
        
        public static NoElse ElseInstance(Visitor n, bool b) { return new NoElse(n, b); }
        
        public static NoDataIFs DataIFInstance(Visitor n1, Visitor n2, bool b) { return new NoDataIFs(n1, n2, b); }
        
        public override MemoryManager Visit(JMemory memory) {
            MemoryManager manager = new MemoryManager();
            foreach (var tp in this.IFCases) {
                Visitor condition = tp.Condition;
                Visitor exp = tp.Body;
                bool needReturnNull = tp.NeedReturn;
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
                Visitor exp = this.Else.Body;
                bool needReturnNull = this.Else.NeedReturn;
                TValue expValue = manager.Registry(exp.Visit(memory));
                if (manager.NeedReturn) 
                    return manager;
                TValue v = needReturnNull ? Consts.Number.Null : expValue;
                return manager.Success(v);
            }
            return manager.Success(Consts.Number.Null);
        }
        //################################################ VAL ELSE:
        public class NoElse {
            public Visitor Body { get; set; } 
            public bool NeedReturn { get; set; } 

            public NoElse(Visitor body, bool need_return) {
                this.Body = body;
                this.NeedReturn = need_return;
            }
        }
        //################################################ VAL ELIF:
        public class NoDataIFs : NoElse { 
            public Visitor Condition { get; set; } 
            public NoDataIFs(Visitor condition, Visitor body, bool need_return):base(body, need_return) { 
                this.Condition = condition;
            }
        }
    }
}
