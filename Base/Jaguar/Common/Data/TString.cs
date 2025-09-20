using System;
using Common.Errors;

namespace Common.Data {
    [Serializable()]
    public class TString : TValue {
        //public string Value { get; set; } //get { return _value; } set { _value = value; } } //private string _value = null;

        public string VAL = default;
        public override object Value { get { return this.VAL; } set { this.VAL = (string)value; } }

        public TString(string value):base() {
            if (value == null) Console.WriteLine("ERRO estranho");
            this.VAL = value;
        }
        public TString(string value, JMemory _memory) : this(value) {
            this.memory = _memory;
        }
        public TString(TError e):base() {
            this.VAL = null;
            this.Error = e;
        }
        public TString(TError e, JMemory _memory) : this(e) {
            this.memory = _memory;
        }
        public override TValue Add(TValue other) {
            if (other.GetType() == typeof(TString)) {
                return new TString(this.VAL + ((TString)other).VAL, this.memory);
            } else if (other.GetType() == typeof(TNumber)) {
                return new TString(this.VAL + ((TNumber)other).VAL, this.memory);
            } else if (other.GetType() == typeof(TList)) {
                return new TString(this.VAL + ((TList)other).ToStr(), this.memory);
            }
            return new TString(this.IllegalOp(other), this.memory);
        }
        public override TValue Multiply(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = (TNumber)other;
                if (num.VAL != null) {
                    string s = "";
                    int n = ((int)num.VAL); // Pegar a parte inteira
                    for (int i = 0; i < n; i++) s = s + this.VAL;
                    return new TString(s, this.memory);
                }
            }            
            return new TString(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonEq(TValue other) {
            if (other.GetType() == typeof(TString)) {
                TNumber num = new TNumber(this.VAL == ((TString)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override bool IsTrue() {
            return this.VAL.Length > 0;
        }
        public override TValue Copy() {
            TString copy = new TString(this.VAL);
            copy.SetLocation(this.NOIni, this.NOEnd);
            copy.SetMemory(this.memory);
            return copy;
        }
        public override string ToStr() {
            return "\""+this.VAL+"\"";
        }
        public override string ToString() {
            return this.VAL;
        }
        public override TValue Sub(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue Divide(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue Pow(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue ComparisonNe(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue ComparisonLt(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue ComparisonGt(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue ComparisonLte(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue ComparisonGte(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue And(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue Or(TValue other) { return new TString(this.IllegalOp(other), this.memory); }
        public override TValue Not() { return new TString(this.IllegalOp(this), this.memory); }
    }
}

