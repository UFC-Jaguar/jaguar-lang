using System;
using System.Collections.Generic;
using Common.Errors;

namespace Common.Data {
    [Serializable()]
    public class TList : TValue {
        public List<TValue> VAL = default;
        public override object Value { get { return this.VAL; } set { this.VAL = (List<TValue>) value;  } }
        public TList(List<TValue> elements): base() {
            this.VAL = elements;
        }
        public override TValue Add(TValue other) {
            TList copyOfList = (TList)this.Copy();
            if (other.GetType() == typeof(TList)) {
                TList o = (TList)other;
                foreach (TValue n in o.VAL)
                    copyOfList.VAL.Add(n);
                return copyOfList;
            }
            copyOfList.VAL.Add(other);
            copyOfList.Error = null;
            return copyOfList;
        }
        public override TValue Sub(TValue other){
            TList copyList = (TList) this.Copy();
            if (other.GetType() == typeof(TNumber)){
                TNumber num = (TNumber) other;
                try{
                    int i = num.VAL != null ? (int)num.VAL : 0;
                    if (i < 0) 
                        i += copyList.VAL.Count;
                    copyList.VAL.RemoveAt(i);
                    copyList.Error = null;
                    return copyList;
                } catch {
                    copyList.Error = new TRunTimeError(
                        other.NOIni, other.NOEnd,
                        "Fail on removed! Out of bounds index \"for this list\"",
                        this.memory
                    );
                    return copyList;// Falha: Nulo e Erro
                }
            }
            copyList.Error = this.IllegalOp(other);
            return copyList;
        }
        public override TValue Multiply(TValue other) {
            TList result_list = new TList(new List<TValue>());
            if (other.GetType() == typeof(TNumber)){
                foreach (TValue n in this.VAL)
                    result_list.VAL.Add(n.Multiply(other));
                return result_list;
            }
            result_list.Error = new TError(this.NOIni, this.NOEnd, TError.ERun, "TList.Multiply by {"+other+"} not defined!!!");
            return result_list; 
        }
        public override TValue Divide(TValue other) {
            if (other.GetType() == typeof(TNumber)){
                TNumber o = (TNumber) other;
                try{
                    int i = o.VAL != null ? (int)o.VAL : 0;
                    if (i < 0) i = i + this.VAL.Count;
                    return (TValue) this.VAL[i];// TODO: ideia [o.VAL], Nulo; Este Cast pode falhar? Avaliar a questão do TValue pai de todos
                } catch {
                    this.Error = new TRunTimeError(
                        o.NOIni, o.NOEnd,
                         "Fail on Retrieved! Out of bounds index \"for this list\"",
                        this.memory
                    );
                    return this; 
                }
            }
            this.Error = this.IllegalOp(other);
            return this;
        }
        public override TValue Copy(){
            TList copy = new TList(new List<TValue>(this.VAL));
            copy.SetLocation(this.NOIni, this.NOEnd);
            copy.SetMemory(this.memory);
            return copy;
        }
        public override string ToString() {
            string s = "[";
            for (int i = 0; i < this.VAL.Count; i++) {
                s += this.VAL[i].ToString() + (i + 1 < this.VAL.Count ? ", " : "");
            }
            return s + "]";
        }
        public override string ToStr() {
            return this.ToString();
        }
        public string Shell() {
            if (this.VAL.Count == 1) return this.VAL[0].ToStr();
            string s = "[";
            for (int i = 0; i < this.VAL.Count; i++) {
                s += this.VAL[i].ToString() + (i + 1 < this.VAL.Count ? "," : "");
            }
            return s + "]";
        }
        /*public override object Val() {
            return this.VAL;
        }*/
    }
}


