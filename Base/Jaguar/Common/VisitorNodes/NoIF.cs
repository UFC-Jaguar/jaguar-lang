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
        
        public static NoElse ElseInstance(Visitor n) { return new NoElse(n); }
        
        public static NoDataIFs DataIFInstance(Visitor n1, Visitor n2) { return new NoDataIFs(n1, n2); }
        
        public override DataFlow Visit(JMemory memory) {
            DataFlow manager = new DataFlow();
            foreach (var tp in this.IFCases) {
                Visitor condition = tp.Condition;
                Visitor exp = tp.Body;
                TValue conditionValue = manager.update_and_get_value(condition.Visit(memory)); //TODO: Ver isso
                if (manager.ReFlow()) 
                    return manager;

                if (conditionValue.IsTrue()) {
                    TValue t_list_value = manager.update_and_get_value(exp.Visit(memory));
                    if (manager.NeedReturn) 
                        return manager;
                    //TValue v = needReturnNull ? Consts.Number.Null : t_list_value;
                    //v = t_list_value.value[len(t_list_value.value)-1] if t_list_value.value else TBase.NIL
                    TList t_list = ((TList)t_list_value);
                    int len = t_list.VAL.Count;
                    TValue v = len > 0 ? t_list.VAL[len-1] : Consts.Number.Null;
                    return manager.SetDefaultAndNewTValue(v);
                }
            }
            if (this.Else != null) {
                Visitor exp = this.Else.Body;
                TValue t_list_value = manager.update_and_get_value(exp.Visit(memory));
                if (manager.ReFlow()) 
                    return manager;
                //TValue v = needReturnNull ? Consts.Number.Null : t_list_value;
                TList t_list = ((TList)t_list_value);
                int len = t_list.VAL.Count;
                TValue v = len > 0 ? t_list.VAL[len - 1] : Consts.Number.Null;
                return manager.SetDefaultAndNewTValue(v);
            }
            return manager.SetDefaultAndNewTValue(Consts.Number.Null);
        }
        //################################################ MAT ELSE:
        public class NoElse {
            public Visitor Body { get; set; }
            public NoElse(Visitor body) {
                this.Body = body;
            }
        }
        //################################################ MAT ELIF:
        public class NoDataIFs : NoElse { 
            public Visitor Condition { get; set; } 
            public NoDataIFs(Visitor condition, Visitor body):base(body) { 
                this.Condition = condition;
            }
        }
    }
}
