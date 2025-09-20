using Common.Errors;
using System;
//using System.Collections.Generic;

namespace Common.Data {
    [Serializable()]
    public class TNumber : TValue {
        public float? VAL = null;//default;
        public override object Value { get { return this.VAL; } set { this.VAL = (float?)value; } }

        public TNumber(float? value):base() {
            this.VAL = value;
        }
        public TNumber(float? value, JMemory _memory) : this(value) {
            this.memory = _memory;
        }
        public TNumber(TError e):base() {
            this.VAL = null;
            this.Error = e;
        }
        public TNumber(TError e, JMemory _memory):this(e) {
            this.memory = _memory;
        }
        public override TValue Add(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                return new TNumber(this.VAL + ((TNumber)other).VAL, this.memory);
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Sub(TValue other) {
            if (other.GetType() == typeof(TNumber))
                return new TNumber(this.VAL - ((TNumber)other).VAL, this.memory);
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Multiply(TValue other) {
            if (other.GetType() == typeof(TNumber))
                return new TNumber(this.VAL * ((TNumber)other).VAL, this.memory);
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Divide(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                if (((TNumber)other).VAL==0) {
                    TError e = new TRunTimeError(other.NOIni, other.NOEnd, "Division by zero: class TNumber.div", this.memory);
                    return new TNumber(e, this.memory);
                }
                return new TNumber(this.VAL / ((TNumber)other).VAL, this.memory);
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Pow(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                double a = (double)this.VAL;
                double b = (double)((TNumber)other).VAL;
                return new TNumber((float) Math.Pow(a,b), this.memory);
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonEq(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL == ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonNe(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL != ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonLt(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL < ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonGt(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL > ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonLte(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL <= ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue ComparisonGte(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                TNumber num = new TNumber(this.VAL >= ((TNumber)other).VAL ? 1 : 0);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue And(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                float? test = this.VAL!=null && ((TNumber)other).VAL!=null ? 1 : 0;
                test = test==1 && (this.VAL != 0 && ((TNumber)other).VAL != 0) ? ((TNumber)other).VAL : 0;
                TNumber num = new TNumber(test);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        public override TValue Or(TValue other) {
            if (other.GetType() == typeof(TNumber)) {
                float? test = testing(this.VAL);
                test = test!=0? test : testing(((TNumber)other).VAL);
                TNumber num = new TNumber(test);
                num.SetMemory(this.memory);
                return num;
            }
            return new TNumber(this.IllegalOp(other), this.memory);
        }
        private float? testing(float? f) { return f != null && f != 0 ? f : 0; }

	    public override TValue Not(){
            TNumber num = new TNumber(this.VAL == 0 ? 1 : 0);
            num.SetMemory(this.memory);
            return num;
        }
        public override bool IsTrue() {
            return this.VAL!=null && this.VAL != 0;
        }

        public override TValue Copy(){
		    TNumber clone = new TNumber(this.VAL);
		    clone.SetLocation(this.NOIni, this.NOEnd);
		    clone.SetMemory(this.memory);
		    return clone;
        }
        public override string ToString() {
            string sb = "";
            if (this.VAL!=null)
                sb = sb + this.VAL.ToString();
            if (this.Error != null) 
                new Exception(sb + this.Error.ToString());
            return sb;
        }
        public override string ToStr() {
            return this.ToString();
        }
    }
}
